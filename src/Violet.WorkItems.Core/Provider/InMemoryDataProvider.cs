using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;

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

        public Task<QueryResult> QueryWorkItemsAsync(Query query)
        {
            if (query is ListQuery listQuery)
            {
                var predicate = ConvertClauseToPredicate(listQuery.Clause);

                var result = _data.Values.Where(predicate);

                return Task.FromResult((QueryResult)new ListQueryResult(listQuery, result));
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private Func<WorkItem, bool> ConvertClauseToPredicate(BooleanClause clause)
            => clause switch
            {
                ProjectCodeEqualityClause c => (WorkItem wi) => wi.ProjectCode == c.ProjectCode,
                WorkItemTypeEqualityClause c => (WorkItem wi) => wi.WorkItemType == c.WorkItemType,
                IdEqualityClause c => (WorkItem wi) => wi.Id == c.Id,
                PropertyEqualityClause c => (WorkItem wi) => wi[c.PropertyName].Value == c.PropertyValue,

                AndClause c => (WorkItem wi) => c.SubClauses.All(sc => ConvertClauseToPredicate(sc)(wi)),
                OrClause c => (WorkItem wi) => c.SubClauses.Any(sc => ConvertClauseToPredicate(sc)(wi)),
                NotClause c => (WorkItem wi) => !ConvertClauseToPredicate(c.SubClause)(wi),

                _ => throw new NotImplementedException(),
            };

        public IEnumerable<QueryError> ValidateQuery(Query query)
            => query switch
            {
                ListQuery listQuery => ValidateClause(listQuery.Clause),
                _ => new QueryError[] { new QueryError("InMemoryDataProvider only supports ListQuery") },
            };

        private IEnumerable<QueryError> ValidateClause(BooleanClause clause)
        {
            switch (clause)
            {
                case ProjectCodeEqualityClause:
                case WorkItemTypeEqualityClause:
                case IdEqualityClause:
                case PropertyEqualityClause:
                    break;

                case BooleanMultiClause multiClause:
                    foreach (var c in multiClause.SubClauses)
                    {
                        foreach (var e in ValidateClause(c))
                        {
                            yield return e;
                        }
                    }
                    break;

                default:
                    yield return new QueryError($"{nameof(InMemoryDataProvider)} does not support this clause");
                    break;
            }
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