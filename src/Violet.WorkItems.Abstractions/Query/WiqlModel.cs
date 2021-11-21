using System.Collections.Immutable;

namespace Violet.WorkItems.Query;

public abstract record QueryClause();

public abstract record BooleanMultiClause(ImmutableArray<QueryClause> SubClauses)
    : QueryClause();
public record ProjectClause(string ProjectCode)
    : QueryClause();
public record WorkItemIdClause(string WorkItemId)
    : QueryClause();
public record WorkItemTypeClause(string WorkItemType)
    : QueryClause();
public abstract record PropertyClause(string PropertyName)
    : QueryClause();

public record AndClause(ImmutableArray<QueryClause> SubClauses)
    : BooleanMultiClause(SubClauses)
{
    public static AndClause Create(params QueryClause[] subClauses)
        => new AndClause(subClauses.ToImmutableArray());
}

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

public record WorkItemsQuery(QueryClause Clause);

public record QueryError(string Message, QueryClause? Clause);