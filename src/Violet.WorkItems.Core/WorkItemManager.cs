using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types;
using Violet.WorkItems.Types.CommonSdlc;

namespace Violet.WorkItems
{
    public class WorkItemManager
    {
        private readonly IWorkItemProvider _provider;

        public WorkItemManager(IWorkItemProvider provider)
        {
            this._provider = provider;
        }

        public Task<(WorkItem, WorkItemDescriptor)> CreateTemplateAsync(string projectCode, string workItemType)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
            {
                throw new ArgumentException("message", nameof(projectCode));
            }

            if (string.IsNullOrWhiteSpace(workItemType))
            {
                throw new ArgumentException("message", nameof(workItemType));
            }

            var wid = GetWorkItemDescription(projectCode, workItemType);

            //TODO: Set default values

            var wi = new WorkItem(projectCode, "NEW", workItemType, wid.Properties.Select(pd => new Property(pd.Name, pd.DataType, string.Empty)), Array.Empty<LogEntry>());

            return Task.FromResult((wi, wid));
        }

        public async Task<bool> Create(string projectCode, string workItemType, IEnumerable<Property> properties)
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

            var result = false;

            if (properties.Count() > 0)
            {
                var newIdentifer = await _provider.NextNumberAsync(projectCode);

                //TODO: Check property set completeness to work item descriptor
                //TODO: Check with validator

                var wi = new WorkItem(projectCode, newIdentifer.ToString(), workItemType, properties.ToArray(), Array.Empty<LogEntry>());

                await _provider.SaveNewWorkItemAsync(wi);

                result = true;
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

            var workItem = await _provider.GetAsync(projectCode, id);

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

            await _provider.SaveUpdatedWorkItemAsync(workItem);
        }


        // TODO: Static for prototyping
        private static WorkItemDescriptor GetWorkItemDescription(string project, string type)
        {
            var types = new WorkItemDescriptor[] {
                BugTaskModel.Bug,
                BugTaskModel.Task,
                EpicFeatureUserStoryModel.Epic,
                EpicFeatureUserStoryModel.Feature,
                EpicFeatureUserStoryModel.UserStory,
             };

            return types.FirstOrDefault(wid => wid.Name == type);
        }
    }
}