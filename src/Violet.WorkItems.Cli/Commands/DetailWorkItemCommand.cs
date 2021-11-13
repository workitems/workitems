using System;
using System.IO;
using System.Threading.Tasks;
using Violet.WorkItems.Text;

namespace Violet.WorkItems.Cli;

public static class DetailWorkItemCommand
{
    public static async Task<int> ExecuteAsync(WorkItemManager manager, string? projectCode, string? id, TextWriter writer)
    {
        if (manager is null)
        {
            throw new ArgumentNullException(nameof(manager));
        }

        if (string.IsNullOrWhiteSpace(projectCode))
        {
            throw new ArgumentException("message", nameof(projectCode));
        }

        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("message", nameof(id));
        }

        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        var workItem = await manager.GetAsync(projectCode, id);

        if (workItem == null)
        {
            writer.WriteLine($"Could not find the specified workitem {id} in project {projectCode}");
        }
        else
        {
            await WorkItemFormatter.FormatAsync(manager.DescriptorManager, workItem, writer);
        }

        return (workItem != null) ? 0 : 1;
    }
}
