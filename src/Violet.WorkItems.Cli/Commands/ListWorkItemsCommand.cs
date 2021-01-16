using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Violet.WorkItems.Text;
using Violet.WorkItems.Provider;

namespace Violet.WorkItems.Cli
{
    public static class ListWorkItemsCommand
    {
        public static async Task<int> ExecuteAsync(WorkItemManager workItemManager, string? project, string? type, TextWriter writer)
        {
            if (workItemManager is null)
            {
                throw new ArgumentNullException(nameof(workItemManager));
            }

            if (string.IsNullOrWhiteSpace(project))
            {
                throw new ArgumentException("message", nameof(project));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var conditions = ImmutableArray.Create<BooleanClause>(new ProjectCodeEqualityClause(project));
            if (type is not null)
            {
                conditions = conditions.Add(new WorkItemTypeEqualityClause(type));
            }

            var result = await workItemManager.DataProvider.QueryWorkItemsAsync(new ListQuery(new AndClause(conditions))) as ListQueryResult;

            var formatter = new WorkItemFormatter();

            foreach (var item in result.WorkItems)
            {
                writer.WriteLine(formatter.FormatShortLine(item));
            }

            return 0;
        }
    }
}