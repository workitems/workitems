using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types;

namespace Violet.WorkItems
{
    public class WorkItemManager
    {
        private readonly IDataProvider _dataProvider;
        private bool _initialized = false;
        public DescriptorManager DescriptorManager { get; }

        public WorkItemManager(IDataProvider dataProvider, IDescriptorProvider descriptorProvider)
        {
            _dataProvider = dataProvider;
            DescriptorManager = new DescriptorManager(descriptorProvider);
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

            await InitAsync();

            if (properties.Count() > 0)
            {
                var newIdentifer = (await _dataProvider.NextNumberAsync(projectCode)).ToString();

                //TODO: Check property set completeness to work item descriptor
                //TODO: Check with validator

                var wi = new WorkItem(projectCode, newIdentifer, workItemType, properties.ToArray(), Array.Empty<LogEntry>());

                await _dataProvider.SaveNewWorkItemAsync(wi);

                return new WorkItemCreatedResult(true, newIdentifer);
            }

            return new WorkItemCreatedResult(false, null);
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

        public async Task UpdateAsync(string projectCode, string id, IEnumerable<Property> properties)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
            {
                throw new ArgumentException("message", nameof(projectCode));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("message", nameof(id));
            }

            await InitAsync();

            //TODO: Check property set completeness to work item descriptor
            //TODO: Check with validator

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

            await _dataProvider.SaveUpdatedWorkItemAsync(workItem);
        }
    }
}