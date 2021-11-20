namespace Violet.WorkItems.Query;


public record QueryClause();

public record ProjectClause(string ProjectCode)
    : QueryClause();
public record WorkItemTypeClause(string WorkItemType)
    : QueryClause();
public record PropertyClause(string PropertyName)
    : QueryClause();

public record StringMatchClause(string PropertyName, string Match)
    : PropertyClause(PropertyName);
public record ValueMatchClause(string PropertyName, string[] Values, bool Not)
    : PropertyClause(PropertyName);
public record JoinClause(string PropertyName, QueryClause[] Clauses, bool ReturnData)
    : PropertyClause(PropertyName);

public record WorkItemsQuery(params QueryClause[] Clauses);

public static class WorkItemsQueryExtensions
{
    public WorkItemsQuery OfProjectCodeAndType(string projectCode, string workItemType)
        => new WorkItemQuery(
            new ProjectClause(projectCode),
            new WorkItemTypeClause(workItemType)
        );
}