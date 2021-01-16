using System.Collections.Immutable;

namespace Violet.WorkItems.Provider
{
    public record BooleanClause();

    // property == value, Type == value, Id == value, Project == values
    public record PropertyEqualityClause(string PropertyName, string PropertyValue) : BooleanClause();
    public record ProjectCodeEqualityClause(string ProjectCode) : BooleanClause();
    public record IdEqualityClause(string Id) : BooleanClause();
    public record WorkItemTypeEqualityClause(string WorkItemType) : BooleanClause();

    // and, or, not
    public record BooleanMultiClause(ImmutableArray<BooleanClause> SubClauses) : BooleanClause();
    public record AndClause(ImmutableArray<BooleanClause> SubClauses) : BooleanMultiClause(SubClauses);
    public record OrClause(ImmutableArray<BooleanClause> SubClauses) : BooleanMultiClause(SubClauses);
    public record NotClause(BooleanClause SubClause) : BooleanClause();


    public record Query();
    public record ListQuery(BooleanClause Clause) : Query();
    public record TreeQuery(ImmutableArray<BooleanClause> ClauseHierarchy) : Query();
}