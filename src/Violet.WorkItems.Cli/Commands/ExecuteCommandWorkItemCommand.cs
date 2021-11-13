using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Violet.WorkItems.Cli;

public static class ExecuteCommandWorkItemCommand
{
    public static async Task<int> ExecuteAsync(WorkItemManager manager, string? projectCode, string? id, string? command)
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

        if (string.IsNullOrWhiteSpace(command))
        {
            throw new ArgumentNullException(nameof(command));
        }

        var result = await manager.ExecuteCommandAsync(projectCode, id, command);

        if (result.Success)
        {
            Console.WriteLine($"Updated WorkItem and execute command {command} in project {result.ChangedWorkItem?.ProjectCode} with id {result.ChangedWorkItem?.Id}");
        }
        else
        {
            Console.WriteLine($"Failed to update WorkItem and execute command {command}");

            foreach (var error in result.Errors)
            {
                Console.WriteLine($"{error.Property}: {error.Message}");
            }
        }

        return result.Success ? 0 : 1;
    }
}
