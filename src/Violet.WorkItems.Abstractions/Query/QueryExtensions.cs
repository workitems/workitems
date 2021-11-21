using System.Collections.Immutable;
using System.Linq;

namespace Violet.WorkItems.Query;

public static class QueryExtensions
{
    public static QueryClause EnsureProjectCode(this QueryClause self, string projectCode)
    {
        if (self is AndClause a && a.SubClauses.OfType<ProjectClause>().Where(pc => pc.ProjectCode == projectCode).Any())
        {
            return self;
        }
        else
        {
            return new AndClause(new QueryClause[] { new ProjectClause(projectCode), self }.ToImmutableArray());
        }
    }

    public static TClause? GetTopLevel<TClause>(this QueryClause self) where TClause : QueryClause
        => self switch
        {
            AndClause a => a.SubClauses.OfType<TClause>().FirstOrDefault(),
            _ => null
        };
}