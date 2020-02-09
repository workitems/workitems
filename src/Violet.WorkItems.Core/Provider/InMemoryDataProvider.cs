using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Violet.WorkItems.Provider
{
    public class InMemoryDataProvider : IDataProvider
    {
        public bool Read => true;
        public bool Write => true;

        private Dictionary<string, WorkItem> _data = new Dictionary<string, WorkItem>();
        private string GetKey(string projectCode, string id)
            => $"{projectCode}-{id}";

        public Task<WorkItem?> GetAsync(string projectCode, string id)
        {
            WorkItem? result = null;

            _data.TryGetValue(GetKey(projectCode, id), out result);

            return Task.FromResult<WorkItem?>(result);
        }

        public Task<IEnumerable<WorkItem>> ListWorkItemsAsync(string projectCode, string? type = null)
        {
            var result = _data.Values.Where(wi => wi.ProjectCode == projectCode && (type is null || wi.WorkItemType == type));

            return Task.FromResult(result);
        }

        public Task<int> NextNumberAsync(string projectCode)
            => Task.FromResult(_data.Select(wi => int.Parse(wi.Value.Id)).Union(new int[] { 0 }).Max() + 1);

        public Task SaveNewWorkItemAsync(WorkItem workItem)
        {
            _data.Add(GetKey(workItem.ProjectCode, workItem.Id), workItem);

            return Task.CompletedTask;
        }

        public Task SaveUpdatedWorkItemAsync(WorkItem workItem)
        {
            _data[GetKey(workItem.ProjectCode, workItem.Id)] = workItem;

            return Task.CompletedTask;
        }
    }
}