using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace Violet.WorkItems.Provider
{
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

        public IEnumerable<QueryError> ValidateQuery(Query query)
        {
            if (query is ListQuery listQuery)
            {
                if (listQuery.Clause is AndClause andClause)
                {
                    foreach (var clause in andClause.SubClauses)
                    {
                        var error = ValidateClause(clause);

                        if (error != null)
                        {
                            yield return error;
                        }
                    }
                }
                else
                {
                    var error = ValidateClause(listQuery.Clause);

                    if (error != null)
                    {
                        yield return error;
                    }
                }
            }
            else
            {
                yield return new QueryError("GitHub Provider only supports ListQueries");
            }
        }

        private QueryError ValidateClause(BooleanClause clause)
            => clause switch
            {
                ProjectCodeEqualityClause => null,
                WorkItemTypeEqualityClause => null,
                _ => new QueryError($"GitHub Provider does not support {clause.GetType().Name} clauses"),
            };

        public async Task<QueryResult> QueryWorkItemsAsync(Query query)
        {
            if (query is ListQuery listQuery)
            {
                var projectCode = (listQuery.Clause as AndClause).SubClauses.OfType<ProjectCodeEqualityClause>().FirstOrDefault()?.ProjectCode;
                var workItemType = (listQuery.Clause as AndClause).SubClauses.OfType<WorkItemTypeEqualityClause>().FirstOrDefault()?.WorkItemType;

                if (projectCode != null)
                {
                    var result = (await _client.Issue.GetAllForRepository(projectCode.Split('/')[0], projectCode.Split('/')[1], new RepositoryIssueRequest()
                    {
                        State = ItemStateFilter.All
                    }))
                    .Select(i => ConvertToWorkItem(projectCode, i));

                    return new ListQueryResult(listQuery, result);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<WorkItem> GetAsync(string projectCode, string id)
            => ConvertToWorkItem(projectCode, (await _client.Issue.Get(projectCode.Split('/')[0], projectCode.Split('/')[1], int.Parse(id))));
        public Task<int> NextNumberAsync(string projectCode) => throw new NotImplementedException();
        public Task SaveNewWorkItemAsync(WorkItem workItem) => throw new NotImplementedException();
        public Task SaveUpdatedWorkItemAsync(WorkItem workItem) => throw new NotImplementedException();

        private WorkItem ConvertToWorkItem(string projectCode, Issue issue)
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
            new LogEntry[] {

            });

            return result;
        }
    }
}
