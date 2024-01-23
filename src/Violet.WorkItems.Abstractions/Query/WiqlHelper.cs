using System;
using System.Linq;

namespace Violet.WorkItems.Query;

public static class WiqlHelper
{
    public static Func<WorkItem, bool> ConvertQueryClauseToPredicate(QueryClause clause)
        => clause switch
        {
            ProjectClause c => (WorkItem wi) => c.ProjectCode is null || wi.ProjectCode == c.ProjectCode,
            WorkItemTypeClause c => (WorkItem wi) => c.WorkItemType is null || wi.WorkItemType == c.WorkItemType,
            WorkItemIdClause c => (WorkItem wi) => wi.Id == c.WorkItemId,
            FullTextSearchClause c => (WorkItem wi) => c.Text is null || (wi.Properties.Any(p => p.Value?.Contains(c.Text) ?? false)),

            StringMatchClause c => (WorkItem wi) => c.Match is null || (c.Not
                ? !(wi[c.PropertyName]?.Value?.Contains(c.Match) ?? false)
                : wi[c.PropertyName]?.Value?.Contains(c.Match) ?? false),
            ValueMatchClause c => (WorkItem wi) => c.Not
                ? c.Values.All(v => wi[c.PropertyName]?.Value != v)
                : c.Values.Any(v => wi[c.PropertyName]?.Value == v),

            AndClause c => (WorkItem wi) => c.SubClauses.All(sc => ConvertQueryClauseToPredicate(sc)(wi)),
            OrClause c => (WorkItem wi) => c.SubClauses.Any(sc => ConvertQueryClauseToPredicate(sc)(wi)),
            NotClause c => (WorkItem wi) => !ConvertQueryClauseToPredicate(c.SubClause)(wi),

            JoinClause => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
}