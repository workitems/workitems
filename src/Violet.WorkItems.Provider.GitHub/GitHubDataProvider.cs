using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

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

    public async Task<IEnumerable<WorkItem>> ListWorkItemsAsync(string projectCode, string workItemType = null)
        => (await _client.Issue.GetAllForRepository(projectCode.Split('/')[0], projectCode.Split('/')[1], new RepositoryIssueRequest()
        {
            State = ItemStateFilter.All
        }))
        .Select(i => ConvertToWorkItem(projectCode, i));

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
            },
            Array.Empty<LogEntry>());

        return result;
    }
}
