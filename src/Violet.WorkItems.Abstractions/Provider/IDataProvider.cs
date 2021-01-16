using System.Collections.Generic;
using System.Threading.Tasks;

namespace Violet.WorkItems.Provider
{
    public interface IDataProvider
    {
        bool Read { get; }
        bool Write { get; }
        Task SaveNewWorkItemAsync(WorkItem workItem);
        Task SaveUpdatedWorkItemAsync(WorkItem workItem);
        IEnumerable<QueryError> ValidateQuery(Query query);
        Task<QueryResult> QueryWorkItemsAsync(Query query);
        Task<int> NextNumberAsync(string projectCode);
        Task<WorkItem?> GetAsync(string projectCode, string id);
    }
}
