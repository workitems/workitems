using System.Collections.Generic;

namespace Violet.WorkItems.Provider
{
    public record QueryResult(Query Query);
    public record ListQueryResult(ListQuery ListQuery, IEnumerable<WorkItem> WorkItems) : QueryResult(ListQuery);
    public record TreeQueryResult(TreeQuery TreeQuery, IEnumerable<TreeQueryNode> WorkItems) : QueryResult(TreeQuery);
    public record TreeQueryNode(WorkItem workItem, IEnumerable<TreeQueryNode> ChildWorkItems);
}