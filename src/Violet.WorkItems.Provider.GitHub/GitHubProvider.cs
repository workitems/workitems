using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace Violet.WorkItems.Provider
{
    public class GitHubProvider
    {
        public const string DefaultWorkItemType = "Issue";
        public const string TitleProperty = "title";
        public const string BodyProperty = "description";
        public const string StateProperty = "state";

        private readonly GitHubClient _client;
        private readonly string _owner;
        private readonly string _repositoryName;

        public GitHubProvider(string productName, string owner, string repositoryName, Uri uri = null)
        {
            if (string.IsNullOrWhiteSpace(owner))
            {
                throw new ArgumentException("message", nameof(owner));
            }

            if (string.IsNullOrWhiteSpace(repositoryName))
            {
                throw new ArgumentException("message", nameof(repositoryName));
            }

            var productHeaderValue = new ProductHeaderValue(productName);

            _client = new GitHubClient(productHeaderValue, uri ?? GitHubClient.GitHubApiUrl);
            _owner = owner;
            _repositoryName = repositoryName;
        }

        public async Task<IEnumerable<WorkItem>> AllWorkItemsAsync()
            => (await _client.Issue.GetAllForRepository(_owner, _repositoryName, new RepositoryIssueRequest()
            {
                State = ItemStateFilter.All
            }))
                .Select(i => ConvertToWorkItem(i));

        private WorkItem ConvertToWorkItem(Issue i)
        {
            var result = new WorkItem($"{_owner}/{_repositoryName}", i.Number.ToString(), DefaultWorkItemType, new Property[] {
                new Property(TitleProperty, "string", i.Title),
                new Property(BodyProperty, "string", i.Body),
                new Property(StateProperty, "state", i.State.StringValue),
           },
            new LogEntry[] {

            });

            return result;
        }
    }
}
