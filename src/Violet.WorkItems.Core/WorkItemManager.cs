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
        private readonly IDataProvider _dataProvider;
        private bool _initialized = false;
        public DescriptorManager DescriptorManager { get; }
        public ValidationManager ValidationManager { get; }

        public WorkItemManager(IDataProvider dataProvider, IDescriptorProvider descriptorProvider)
        {
            _dataProvider = dataProvider;
            DescriptorManager = new DescriptorManager(descriptorProvider);
            ValidationManager = new ValidationManager(DescriptorManager);
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

            var propertyDescriptors = DescriptorManager.GetAllPropertyDescriptors(workItemType);
            var properties = propertyDescriptors.Select(pd => new Property(pd.Name, pd.DataType, string.Empty));

            //TODO: Set default values

            var wi = new WorkItem(projectCode, "NEW", workItemType, properties, Array.Empty<LogEntry>());

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

            WorkItemCreatedResult result = null;

            await InitAsync();

            if (properties.Count() > 0)
            {
                var newIdentifer = (await _dataProvider.NextNumberAsync(projectCode)).ToString();

                //TODO: Check property set completeness to work item descriptor

                var wi = new WorkItem(projectCode, newIdentifer, workItemType, properties.ToArray(), Array.Empty<LogEntry>());

                var validationResult = await ValidationManager.ValidateAsync(wi, Array.Empty<PropertyChange>());

                if (validationResult.Count() == 0)
                {
                    await _dataProvider.SaveNewWorkItemAsync(wi);

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

        public async Task<WorkItem> GetAsync(string projectCode, string id)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
            {
                throw new ArgumentException("message", nameof(projectCode));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("message", nameof(id));
            }

            var workItem = await _dataProvider.GetAsync(projectCode, id);

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

            WorkItemUpdatedResult result = null;

            await InitAsync();

            //TODO: Check property set completeness to work item descriptor

            var workItem = await GetAsync(projectCode, id);

            var changes = new List<PropertyChange>();

            var newProperties = workItem.Properties
                .Select(p =>
                {
                    var changedValue = properties.FirstOrDefault(changedProperty => changedProperty.Name == p.Name && changedProperty.Value != p.Value);

                    if (changedValue is null)
                    {
                        return p;
                    }
                    else
                    {
                        changes.Add(new PropertyChange(p.Name, p.Value, changedValue.Value));

                        return new Property(p.Name, p.DataType, changedValue.Value);
                    }
                })
                .ToArray();

            var newLog = workItem.Log.Union(new LogEntry[] { new LogEntry(DateTimeOffset.Now, "ABC", "Comment", changes) }).ToArray();

            workItem = new WorkItem(workItem.ProjectCode, workItem.Id, workItem.WorkItemType, newProperties, newLog);

            var errors = await ValidationManager.ValidateAsync(workItem, changes);

            if (errors.Count() == 0)
            {
                await _dataProvider.SaveUpdatedWorkItemAsync(workItem);

                result = new WorkItemUpdatedResult(true, workItem, Array.Empty<ErrorMessage>());
            }
            else
            {
                result = new WorkItemUpdatedResult(false, workItem, errors);
            }

            return result;
        }
    }
}