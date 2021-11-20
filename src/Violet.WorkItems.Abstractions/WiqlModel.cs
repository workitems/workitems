using System.Collections.Immutable;

namespace Violet.WorkItems.Query;

public record QueryClause();

public record BooleanMultiClause(ImmutableArray<QueryClause> SubClauses)
    : QueryClause();
public record ProjectClause(string ProjectCode)
    : QueryClause();
public record WorkItemIdClause(string WorkItemId)
    : QueryClause();
public record WorkItemTypeClause(string WorkItemType)
    : QueryClause();
public record PropertyClause(string PropertyName)
    : QueryClause();

public record AndClause(ImmutableArray<QueryClause> SubClauses)
    : BooleanMultiClause(SubClauses);
public record OrClause(ImmutableArray<QueryClause> SubClauses)
    : BooleanMultiClause(SubClauses);
public record NotClause(QueryClause SubClause)
    : QueryClause();

public record StringMatchClause(string PropertyName, string Match)
    : PropertyClause(PropertyName);
public record ValueMatchClause(string PropertyName, string[] Values, bool Not)
    : PropertyClause(PropertyName);
public record JoinClause(string PropertyName, QueryClause[] Clauses, bool ReturnData)
    : PropertyClause(PropertyName);

public record WorkItemsQuery(ImmutableArray<QueryClause> Clauses)
{
    public WorkItemsQuery(params QueryClause[] Clauses)
        : this(Clauses.ToImmutableArray())
    { }
}

public static class CommonQueries
{
    public static WorkItemsQuery OfProjectCodeAndType(string projectCode, string workItemType)
        => new WorkItemsQuery(
            new ProjectClause(projectCode),
            new WorkItemTypeClause(workItemType)
        );
}