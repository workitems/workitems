using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Violet.WorkItems.Cli
{
    public static class EditWorkItemCommand
    {
        public static async Task<int> ExecuteAsync(WorkItemManager manager, string? projectCode, string? id)
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

            int resultCode = 1;

            var workItem = await manager.GetAsync(projectCode, id);

            if (workItem is null)
            {
                Console.WriteLine($"Could not find the specified workitem {id} in project {projectCode}");
            }
            else
            {
                var changedProperties = new List<Property>();

                foreach (var property in workItem.Properties)
                {
                    Console.Write($"{property.Name} [{property.Value}]: ");
                    var value = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        changedProperties.Add(new Property(property.Name, property.DataType, value));
                    }
                }

                var result = await manager.UpdateAsync(workItem.ProjectCode, workItem.Id, changedProperties);

                if (result.Success)
                {
                    Console.WriteLine($"Updated WorkItem in project {result.UpdatedWorkItem?.ProjectCode} with id {result.UpdatedWorkItem?.Id}");
                }
                else
                {
                    Console.WriteLine($"Failed to update WorkItem");

                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"{error.Property}: {error.Message}");
                    }
                }

                resultCode = result.Success ? 0 : 1;
            }

            return resultCode;
        }
    }
}