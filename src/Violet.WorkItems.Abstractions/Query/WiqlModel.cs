using System;
using System.Collections.Immutable;

namespace Violet.WorkItems.Query;

public abstract record QueryClause(Guid Id);

public abstract record BooleanMultiClause(ImmutableArray<QueryClause> SubClauses)
    : QueryClause(Guid.NewGuid());
public record ProjectClause(string? ProjectCode)
    : QueryClause(Guid.NewGuid());
public record WorkItemIdClause(string WorkItemId)
    : QueryClause(Guid.NewGuid());
public record WorkItemTypeClause(string? WorkItemType)
    : QueryClause(Guid.NewGuid());
public abstract record PropertyClause(string PropertyName)
    : QueryClause(Guid.NewGuid());

public record AndClause(ImmutableArray<QueryClause> SubClauses)
    : BooleanMultiClause(SubClauses)
{
    public static AndClause Create(params QueryClause[] subClauses)
        => new AndClause(subClauses.ToImmutableArray());
}

public record OrClause(ImmutableArray<QueryClause> SubClauses)
    : BooleanMultiClause(SubClauses);
public record NotClause(QueryClause SubClause)
    : QueryClause(Guid.NewGuid());

public record StringMatchClause(string PropertyName, string? Match, bool Not)
    : PropertyClause(PropertyName);
public record ValueMatchClause(string PropertyName, string[] Values, bool Not)
    : PropertyClause(PropertyName);
public record JoinClause(string PropertyName, QueryClause[] Clauses, bool ReturnData)
    : PropertyClause(PropertyName);

public record WorkItemsQuery(QueryClause Clause);

public record QueryError(string Message, QueryClause? Clause);