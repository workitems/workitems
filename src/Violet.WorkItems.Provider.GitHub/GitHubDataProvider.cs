using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using Violet.WorkItems.Query;

namespace Violet.WorkItems.Provider;

public class GitHubDataProvider : IDataProvider
{
    public const string DefaultWorkItemType = "Issue";
    public const string TitleProperty = "title";
    public const string BodyProperty = "description";
    public const string StateProperty = "state";

    private readonly GitHubClient _client;

    public GitHubDataProvider(string connectionString)
        : this("workitems-cli", string.IsNullOrEmpty(connectionString) ? null : new Uri(connectionString))
    { }
    public GitHubDataProvider(string productName, Uri uri = null)
    {
        var productHeaderValue = new ProductHeaderValue(productName);

        _client = new GitHubClient(productHeaderValue, uri ?? GitHubClient.GitHubApiUrl);
    }

    public bool Read => true;
    public bool Write => false;

    public Task<IEnumerable<QueryError>> ValidateQueryAsync(WorkItemsQuery query)
    {
        var errors = new List<QueryError>();

        if (query.Clause.GetTopLevel<ProjectClause>() is ProjectClause pc && pc?.ProjectCode is null)
        {
            errors.Add(new QueryError("Project Code needs to be set", pc));
        }

        if (query.Clause is AndClause a)
        {
            errors.AddRange(a.SubClauses
                .Where(c => !(c is ProjectClause))
                .Select(c => new QueryError($"{nameof(GitHubDataProvider)} does not support clauses of type {c.GetType().Name}", c)));
        }
        else
        {
            errors.Add(new QueryError($"Top Level Query needs to be an {nameof(AndClause)}", query.Clause));
        }

        return Task.FromResult<IEnumerable<QueryError>>(errors);
    }

    public async Task<IEnumerable<WorkItem>> ListWorkItemsAsync(WorkItemsQuery query)
    {
        string projectCode = query.Clause.GetTopLevel<ProjectClause>()?.ProjectCode ?? throw new ArgumentException($"{nameof(GitHubDataProvider)} requires a top level project clause", nameof(query));
        string? workItemType = query.Clause.GetTopLevel<WorkItemTypeClause>()?.WorkItemType;


        return (await _client.Issue.GetAllForRepository(projectCode.Split('/')[0], projectCode.Split('/')[1], new RepositoryIssueRequest()
        {
            State = ItemStateFilter.All
        }))
        .Select(i => ConvertToWorkItem(projectCode, i));
    }

    public async Task<WorkItem> GetAsync(string projectCode, string id)
        => ConvertToWorkItem(projectCode, (await _client.Issue.Get(projectCode.Split('/')[0], projectCode.Split('/')[1], int.Parse(id))));
    public Task<int> NextNumberAsync(string projectCode) => throw new NotImplementedException();
    public Task SaveNewWorkItemAsync(WorkItem workItem) => throw new NotImplementedException();
    public Task SaveUpdatedWorkItemAsync(WorkItem workItem) => throw new NotImplementedException();

    private static WorkItem ConvertToWorkItem(string projectCode, Issue issue)
    {
        if (issue is null)
        {
            return null;
        }

        var result = new WorkItem(projectCode, issue.Number.ToString(), DefaultWorkItemType, new Property[] {
                new Property(TitleProperty, "string", issue.Title),
                new Property(BodyProperty, "string", issue.Body),
                new Property(StateProperty, "state", issue.State.StringValue),
            }.ToImmutableArray(),
            ImmutableArray<LogEntry>.Empty);

        return result;
    }
}
