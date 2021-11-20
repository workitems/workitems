using System.Collections.Generic;
using System.Threading.Tasks;
using Violet.WorkItems.Query;

namespace Violet.WorkItems.Provider;

public interface IDataProvider
{
    bool Read { get; }
    bool Write { get; }
    Task SaveNewWorkItemAsync(WorkItem workItem);
    Task SaveUpdatedWorkItemAsync(WorkItem workItem);
    Task<IEnumerable<QueryError>> ValidateQueryAsync(WorkItemsQuery query);
    Task<IEnumerable<WorkItem>> ListWorkItemsAsync(WorkItemsQuery query);
    Task<int> NextNumberAsync(string projectCode);
    Task<WorkItem?> GetAsync(string projectCode, string id);
}
