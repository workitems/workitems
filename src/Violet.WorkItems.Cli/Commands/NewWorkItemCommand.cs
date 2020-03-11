using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Cli
{
    public static class NewWorkItemCommand
    {
        public static async Task<int> ExecuteAsync(WorkItemManager manager, string? projectCode, string? workItemType)
        {
            if (manager is null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            if (string.IsNullOrWhiteSpace(projectCode))
            {
                throw new ArgumentException("message", nameof(projectCode));
            }

            if (string.IsNullOrWhiteSpace(workItemType))
            {
                throw new ArgumentException("message", nameof(workItemType));
            }

            var wi = await manager.CreateTemplateAsync(projectCode, workItemType);
            var propertyDescriptors = manager.DescriptorManager.GetCurrentPropertyDescriptors(wi);

            if (propertyDescriptors == null)
            {
                Console.WriteLine($"Cannot find type {workItemType} in project {projectCode}");

                return 1;
            }

            var propertiesDictionary = new Dictionary<string, string>();
            foreach (var propertyDescriptor in propertyDescriptors)
            {
                var value = await ValueProviderReadLineAutoCompletion.Readline(manager, wi, propertyDescriptor, string.Empty);

                propertiesDictionary.Add(propertyDescriptor.Name, value);
            }

            var properties = wi.Properties
                .Select(property =>
                {
                    if (propertiesDictionary.TryGetValue(property.Name, out var value))
                    {
                        return new Property(property.Name, property.DataType, value);
                    }
                    else
                    {
                        return property;
                    }
                })
                .Where(p => p != null)
                .ToArray();

            var result = await manager.CreateAsync(projectCode, workItemType, properties);

            if (result.Success)
            {
                Console.WriteLine($"Created WorkItem in project {result.CreatedWorkItem?.ProjectCode} with id {result.Id}");
            }
            else
            {
                Console.WriteLine($"Failed to create WorkItem");

                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"{error.Property}: {error.Message}");
                }
            }

            return result.Success ? 0 : 1;
        }
    }
}