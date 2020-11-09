using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types;
using Violet.WorkItems.Validation;

namespace Violet.WorkItems
{
    public class WorkItemManager
    {
        public IDataProvider DataProvider { get; set; }
        private bool _initialized = false;
        public DescriptorManager DescriptorManager { get; }
        public ValidationManager ValidationManager { get; }

        public static readonly string EmptyValue = string.Empty;

        public WorkItemManager(IDataProvider dataProvider, IDescriptorProvider descriptorProvider)
        {
            DataProvider = dataProvider;
            DescriptorManager = new DescriptorManager(descriptorProvider);
            ValidationManager = new ValidationManager(this, DescriptorManager);
        }

        private async Task InitAsync()
        {
            if (!_initialized) //TODO: make it thread-safe
            {
                await DescriptorManager.LoadAllAsync();

                _initialized = true;
            }
        }

        public async Task<WorkItem> CreateTemplateAsync(string projectCode, string workItemType)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
            {
                throw new ArgumentException("message", nameof(projectCode));
            }

            if (string.IsNullOrWhiteSpace(workItemType))
            {
                throw new ArgumentException("message", nameof(workItemType));
            }

            await InitAsync();

            IEnumerable<Property> properties;

            var (success, propertyDescriptors) = DescriptorManager.GetAllPropertyDescriptors(workItemType);

            if (success)
            {
                properties = propertyDescriptors.Select(pd => new Property(pd.Name, pd.DataType, pd.InitialValue ?? EmptyValue)).ToList();
            }
            else
            {
                properties = new List<Property>();
            }

            var wi = new WorkItem(projectCode, "NEW", workItemType, properties, new List<LogEntry>());

            return wi;
        }

        public async Task<WorkItemCreatedResult> CreateAsync(string projectCode, string workItemType, IEnumerable<Property> properties)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
            {
                throw new ArgumentException("message", nameof(projectCode));
            }

            if (string.IsNullOrWhiteSpace(workItemType))
            {
                throw new ArgumentException("message", nameof(workItemType));
            }

            if (properties is null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (DataProvider is { Write: false })
            {
                throw new InvalidOperationException("DataProvider does not allow write operation");
            }

            WorkItemCreatedResult result;

            await InitAsync();

            if (properties.Count() > 0)
            {
                var newIdentifer = (await DataProvider.NextNumberAsync(projectCode)).ToString();

                var wi = new WorkItem(projectCode, newIdentifer, workItemType, new List<Property>(properties), Array.Empty<LogEntry>());

                // property changes for all values not identical with an empty template.
                var propertyChanges = properties.Where(p => p.Value != EmptyValue).Select(p => new PropertyChange(p.Name, EmptyValue, p.Value));

                var validationResult = await ValidationManager.ValidateAsync(wi, propertyChanges, false);

                if (validationResult.Count() == 0)
                {
                    await DataProvider.SaveNewWorkItemAsync(wi);

                    result = new WorkItemCreatedResult(true, wi, Array.Empty<ErrorMessage>());
                }
                else
                {
                    result = new WorkItemCreatedResult(false, wi, validationResult);
                }

            }
            else
            {
                result = new WorkItemCreatedResult(false, null, Array.Empty<ErrorMessage>());
            }

            return result;
        }

        public async Task<WorkItem?> GetAsync(string projectCode, string id)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
            {
                throw new ArgumentException("message", nameof(projectCode));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("message", nameof(id));
            }

            if (DataProvider is { Read: false })
            {
                throw new InvalidOperationException("DataProvider does not allow read operations");
            }

            await InitAsync();

            var workItem = await DataProvider.GetAsync(projectCode, id);

            return workItem;
        }

        public async Task<WorkItemUpdatedResult> UpdateAsync(string projectCode, string id, IEnumerable<Property> properties)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
            {
                throw new ArgumentException("message", nameof(projectCode));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("message", nameof(id));
            }

            if (DataProvider is { Write: false })
            {
                throw new InvalidOperationException("DataProvider does not allow write operations");
            }

            WorkItemUpdatedResult result;

            await InitAsync();

            var workItem = await GetAsync(projectCode, id);

            if (workItem != null)
            {
                var changes = AnalyzeChanges(workItem.Properties, properties);
                var newProperties = MergePropertySet(workItem.Properties, properties);

                var (newWorkItem, errors) = await UpdateInternalAsync(workItem, newProperties, changes, false);

                if (errors.Count() == 0)
                {
                    result = new WorkItemUpdatedResult(true, newWorkItem, Array.Empty<ErrorMessage>());
                }
                else
                {
                    result = new WorkItemUpdatedResult(false, newWorkItem, errors);
                }
            }
            else
            {
                result = new WorkItemUpdatedResult(false, null, new ErrorMessage[] {
                    new ErrorMessage(nameof(WorkItemManager), string.Empty, $"The work item with id '{id}' in project '{projectCode}' cannot be found.", projectCode, string.Empty, string.Empty),
                });
            }

            return result;
        }

        private async Task<(WorkItem, IEnumerable<ErrorMessage>)> UpdateInternalAsync(WorkItem workItem, IEnumerable<Property> properties, IEnumerable<PropertyChange> changes, bool internalEdit)
        {
            properties = ApplyChangesToPropertySet(properties, changes);

            var newLog = workItem.Log.Union(new LogEntry[] { new LogEntry(DateTimeOffset.Now, "ABC", "Comment", changes.ToArray()) }).ToArray();

            var newWorkItem = new WorkItem(workItem.ProjectCode, workItem.Id, workItem.WorkItemType, properties.ToArray(), newLog);

            var errors = await ValidationManager.ValidateAsync(newWorkItem, changes, internalEdit);

            if (errors.Count() == 0)
            {
                await DataProvider.SaveUpdatedWorkItemAsync(newWorkItem);
            }

            return (newWorkItem, errors);
        }

        private IEnumerable<PropertyChange> AnalyzeChanges(IEnumerable<Property> properties, IEnumerable<Property> requested)
            => requested
                .Select(r => (properties.FirstOrDefault(p => p.Name == r.Name), r))
                .Where(old_new => old_new.Item1.Value != old_new.r.Value)
                .Select(old_new => old_new switch
                    {
                        (null, var newProperty) => new PropertyChange(newProperty.Name, EmptyValue, newProperty.Value),
                        (var oldProperty, var newProperty) => new PropertyChange(oldProperty.Name, oldProperty.Value, newProperty.Value),
                    });

        private IEnumerable<Property> ApplyChangesToPropertySet(IEnumerable<Property> properties, IEnumerable<PropertyChange> changes)
            => properties.Select(p => changes.FirstOrDefault(c => c.Name == p.Name) switch
            {
                PropertyChange change => new Property(p.Name, p.DataType, change.NewValue),
                null => p,
            });

        private IEnumerable<Property> MergePropertySet(IEnumerable<Property> properties, IEnumerable<Property> requested)
            => properties.Union(requested.Where(pr => !properties.Any(p => pr.Name == p.Name)));

        public async Task<WorkItemCommandExecutedResult> ExecuteCommandAsync(string projectCode, string id, string command)
        {
            WorkItemCommandExecutedResult result;
            var workItem = await GetAsync(projectCode, id);

            if (workItem != null)
            {
                var commands = DescriptorManager.GetCurrentCommands(workItem);
                var commandDescriptor = commands?.FirstOrDefault(c => c.Name == command || c.Label == command);

                if (commandDescriptor != null)
                {
                    var changes = await ExecutedCommandAsync(workItem, commandDescriptor);

                    var (newWorkItem, errors) = await UpdateInternalAsync(workItem, workItem.Properties, changes, true);

                    result = new WorkItemCommandExecutedResult(errors.Count() == 0, newWorkItem, errors);
                }
                else
                {
                    result = new WorkItemCommandExecutedResult(false, null, new ErrorMessage[] {
                        new ErrorMessage(nameof(WorkItemManager), string.Empty, $"The command with name '{command}' cannot be found in work item with id '{id}' in project '{projectCode}'.", projectCode, id, string.Empty),
                    });
                }
            }
            else
            {
                result = new WorkItemCommandExecutedResult(false, null, new ErrorMessage[] {
                    new ErrorMessage(nameof(WorkItemManager), string.Empty, $"The work item with id '{id}' in project '{projectCode}' cannot be found.", projectCode, string.Empty, string.Empty),
                });
            }

            return result;
        }

        private Task<IEnumerable<PropertyChange>> ExecutedCommandAsync(WorkItem workItem, CommandDescriptor commandDescriptor)
            => commandDescriptor switch
            {
                ChangePropertyValueCommandDescriptor cpvcd => Task.FromResult<IEnumerable<PropertyChange>>(new PropertyChange[] {
                    new PropertyChange(cpvcd.PropertyName, workItem[cpvcd.PropertyName].Value, cpvcd.TargetValue),
                }),
                _ => throw new ArgumentException(nameof(commandDescriptor)),
            };
    }
}