using System.Collections.Generic;
using System.Threading.Tasks;

namespace Violet.WorkItems.Provider;

public interface IDataProvider
{
    bool Read { get; }
    bool Write { get; }
    Task SaveNewWorkItemAsync(WorkItem workItem);
    Task SaveUpdatedWorkItemAsync(WorkItem workItem);
    Task<IEnumerable<WorkItem>> ListWorkItemsAsync(string projectCode, string? workItemType = null);
    Task<int> NextNumberAsync(string projectCode);
    Task<WorkItem?> GetAsync(string projectCode, string id);
}
