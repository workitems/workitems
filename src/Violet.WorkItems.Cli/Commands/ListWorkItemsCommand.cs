using System;
using System.IO;
using System.Threading.Tasks;
using Violet.WorkItems.Query;
using Violet.WorkItems.Text;

namespace Violet.WorkItems.Cli;

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

        var items = await workItemManager.DataProvider.ListWorkItemsAsync(new WorkItemsQuery(AndClause.Create(new ProjectClause(project), new WorkItemTypeClause(type))));

        foreach (var item in items)
        {
            writer.WriteLine(WorkItemFormatter.FormatShortLine(item));
        }

        return 0;
    }
}
