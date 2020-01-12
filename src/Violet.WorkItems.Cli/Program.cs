using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Violet.WorkItems.Provider.SqlServer;
using Violet.WorkItems.Text;
using Violet.WorkItems.Types.CommonSdlc;

namespace Violet.WorkItems.Cli
{
    class Program
    {
        private static SqlServerDataProvider _provider;
        private static WorkItemManager _manager;
        static Task<int> Main(string[] args)
        {
            _provider = new SqlServerDataProvider(@"Server=localhost\SQLEXPRESS;Database=workitems;Trusted_Connection=True;");
            _manager = new WorkItemManager(_provider, new CommonSdlcDescriptorProvider());
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var app = new CommandLineApplication();
            app.HelpOption();

            app.Command("detail", command =>
            {
                command.HelpOption();
                command.Description = "List all Work Items of the specified type";

                var projectArgument = command.Argument("Project", "The project name in which the issue should be created", false);
                var idArgument = command.Argument("Id", "The work item id", false);

                command.OnExecuteAsync(async (ctx) =>
                {
                    var project = projectArgument.Value;
                    var id = idArgument.Value;

                    return await DetailWorkItemAsync(project, id);
                });
            });

            app.Command("list", command =>
            {
                command.HelpOption();
                command.Description = "List all Work Items of the specified type";

                var projectArgument = command.Argument("Project", "The project name in which the issue should be listed", false);
                var typeArgument = command.Argument("Type", "The type of work item which should be listed (optional)", false);

                command.OnExecuteAsync(async (ctx) =>
                {
                    var project = projectArgument.Value;
                    var type = typeArgument.Value;

                    return await ListWorkItemsAsync(project, type);
                });
            });

            app.Command("new", command =>
            {
                command.HelpOption();
                command.Description = "Creates a new Work Item of a specified type";

                var projectArgument = command.Argument("Project", "The project name in which the issue should be created", false);
                var typeArgument = command.Argument("Type", "The type of work item which should be created", false);

                command.OnExecuteAsync(async (ctx) =>
                {
                    var project = projectArgument.Value;
                    var type = typeArgument.Value;

                    return await NewWorkItemAsync(project, type);
                });
            });

            app.Command("edit", command =>
            {
                command.HelpOption();
                command.Description = "Edit a specified work item";

                var projectArgument = command.Argument("Project", "The project name in which the issue should be edited", false);
                var idArgument = command.Argument("Id", "The work item id", false);

                command.OnExecuteAsync(async (ctx) =>
                {
                    var project = projectArgument.Value;
                    var id = idArgument.Value;

                    return await EditWorkItemAsync(project, id);
                });
            });

            return app.ExecuteAsync(args);
        }

        private static async Task<int> DetailWorkItemAsync(string projectCode, string id)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
            {
                throw new ArgumentException("message", nameof(projectCode));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("message", nameof(id));
            }

            var workItem = await _manager.GetAsync(projectCode, id);
            var logEntryTypeDescriptors = _manager.DescriptorManager.GetCurrentLogEntryTypeDescriptors(workItem);

            if (workItem == null)
            {
                Console.WriteLine($"Could not find the specified workitem {id} in project {projectCode}");
            }
            else
            {
                var formatter = new WorkItemFormatter();

                await formatter.FormatAsync(logEntryTypeDescriptors, workItem, Console.Out);
            }

            return (workItem != null) ? 0 : 1;
        }

        private static async Task<int> ListWorkItemsAsync(string project, string type)
        {
            if (string.IsNullOrWhiteSpace(project))
            {
                throw new ArgumentException("message", nameof(project));
            }

            var items = await _provider.ListWorkItemsAsync(project, type);

            var formatter = new WorkItemFormatter();

            foreach (var item in items)
            {
                Console.WriteLine(formatter.FormatShortLine(item));
            }

            return 0;
        }

        private static async Task<int> NewWorkItemAsync(string projectCode, string workItemType)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
            {
                throw new ArgumentException("message", nameof(projectCode));
            }

            if (string.IsNullOrWhiteSpace(workItemType))
            {
                throw new ArgumentException("message", nameof(workItemType));
            }

            var wi = await _manager.CreateTemplateAsync(projectCode, workItemType);
            var propertyDescriptors = _manager.DescriptorManager.GetCurrentPropertyDescriptors(wi);

            if (propertyDescriptors == null)
            {
                Console.WriteLine($"Cannot find type {workItemType} in project {projectCode}");

                return 1;
            }

            var propertiesDictionary = new Dictionary<string, string>();
            foreach (var propertyDescriptor in propertyDescriptors)
            {
                Console.Write($"{propertyDescriptor.Name}: ");
                var value = Console.ReadLine();

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

            var result = await _manager.CreateAsync(projectCode, workItemType, properties);

            if (result.Success)
            {
                Console.WriteLine($"Created WorkItem in project {result.CreatedWorkItem.ProjectCode} with id {result.Id}");
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


        private static async Task<int> EditWorkItemAsync(string projectCode, string id)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
            {
                throw new ArgumentException("message", nameof(projectCode));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("message", nameof(id));
            }

            var wi = await _manager.GetAsync(projectCode, id);

            var changedProperties = new List<Property>();

            foreach (var property in wi.Properties)
            {
                Console.Write($"{property.Name} [{property.Value}]: ");
                var value = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    changedProperties.Add(new Property(property.Name, property.DataType, value));
                }
            }

            var result = await _manager.UpdateAsync(wi.ProjectCode, wi.Id, changedProperties);

            if (result.Success)
            {
                Console.WriteLine($"Updated WorkItem in project {result.UpdatedWorkItem.ProjectCode} with id {result.UpdatedWorkItem.Id}");
            }
            else
            {
                Console.WriteLine($"Failed to update WorkItem");

                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"{error.Property}: {error.Message}");
                }
            }

            return result.Success ? 0 : 1;
        }
    }
}
