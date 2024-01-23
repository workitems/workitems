using System;
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
    public static QueryClause? GetTopLevel(this QueryClause self, Guid id)
        => self switch
        {
            AndClause a => a.SubClauses.FirstOrDefault(c => c.Id == id),
            _ => null
        };
    public static WorkItemsQuery AddTopLevelClause(this WorkItemsQuery self, QueryClause clause)
        => self switch
        {
            null => new WorkItemsQuery(AndClause.Create(clause)),
            { Clause: AndClause andClause } => new WorkItemsQuery(new AndClause(andClause.SubClauses.Add(clause))),
            { Clause: var anyClause } => new WorkItemsQuery(AndClause.Create(anyClause, clause)),
        };

    public static WorkItemsQuery RemoveTopLevelClause(this WorkItemsQuery self, QueryClause? clause)
        => self switch
        {
            { Clause: AndClause andClause } => new WorkItemsQuery(new AndClause(andClause.SubClauses.RemoveAll(sc => sc.Id == clause?.Id))),
            _ => self,
        };
}