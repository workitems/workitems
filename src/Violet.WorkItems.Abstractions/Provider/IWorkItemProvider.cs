using System.Collections.Generic;
using System.Threading.Tasks;

namespace Violet.WorkItems.Provider
{
    public interface IWorkItemProvider
    {
        Task SaveNewWorkItemAsync(WorkItem workItem);
        Task SaveUpdatedWorkItemAsync(WorkItem workItem);
        Task<IEnumerable<WorkItem>> ListWorkItemsAsync(string project, string type = null);
        Task<int> NextNumberAsync(string projectCode);
        Task<WorkItem> GetAsync(string projectCode, string id);
    }
}
