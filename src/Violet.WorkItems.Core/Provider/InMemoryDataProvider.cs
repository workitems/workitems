using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Query;

namespace Violet.WorkItems.Provider;

public class InMemoryDataProvider : IDataProvider
{
    public bool Read => true;
    public bool Write => true;

    private readonly Dictionary<string, WorkItem> _data = new();
    private string GetKey(string projectCode, string id)
        => $"{projectCode}-{id}";

    public Task<WorkItem?> GetAsync(string projectCode, string id)
    {
        _data.TryGetValue(GetKey(projectCode, id), out WorkItem? result);

        return Task.FromResult(result);
    }

    public Task<IEnumerable<QueryError>> ValidateQueryAsync(WorkItemsQuery query)
    {
        var errors = new List<QueryError>();

        if (query.Clause.GetTopLevel<ProjectClause>() is ProjectClause pc && pc?.ProjectCode is null)
        {
            errors.Add(new QueryError("Project Code needs to be set", pc));
        }

        return Task.FromResult<IEnumerable<QueryError>>(errors);
    }

    public Task<IEnumerable<WorkItem>> ListWorkItemsAsync(WorkItemsQuery query)
    {
        string projectCode = query.Clause.GetTopLevel<ProjectClause>()?.ProjectCode ?? throw new ArgumentException($"{nameof(InMemoryDataProvider)} requires a top level project clause", nameof(query));
        string? workItemType = query.Clause.GetTopLevel<WorkItemTypeClause>()?.WorkItemType;

        var result = _data.Values.Where(wi => wi.ProjectCode == projectCode && (workItemType is null || wi.WorkItemType == workItemType));

        // secondary filtering
        var predicate = WiqlHelper.ConvertQueryClauseToPredicate(query.Clause);

        return Task.FromResult(result.Where(predicate));
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
