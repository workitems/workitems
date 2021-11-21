namespace Violet.WorkItems.Query;

public static class CommonQueries
{
    public static WorkItemsQuery OfProjectCode(string projectCode)
        => new WorkItemsQuery(
            AndClause.Create(
                new ProjectClause(projectCode)
            )
        );
    public static WorkItemsQuery OfProjectCodeAndType(string projectCode, string workItemType)
        => new WorkItemsQuery(
            AndClause.Create(
                new ProjectClause(projectCode),
                new WorkItemTypeClause(workItemType)
            )
        );
}
