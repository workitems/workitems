using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Text;
using Violet.WorkItems.Types.CommonSdlc;

namespace Violet.WorkItems.Cli
{
    class Program
    {
        static Task<int> Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var app = new CommandLineApplication();
            app.HelpOption();
            var sourceNameOption = app.Option("--source", "Specify the data source used for the commands.", CommandOptionType.SingleOrNoValue, true);

            app.Command("detail", command =>
            {
                command.HelpOption();
                command.Description = "List all Work Items of the specified type";

                var projectArgument = command.Argument("Project", "The project name in which the issue should be created", false);
                var idArgument = command.Argument("Id", "The work item id", false);

                command.OnExecuteAsync(async (ctx) =>
                {
                    var manager = SetupManager(sourceNameOption.Value());

                    var project = projectArgument.Value;
                    var id = idArgument.Value;

                    return await DetailWorkItemAsync(manager, project, id, Console.Out);
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
                    var manager = SetupManager(sourceNameOption.Value());

                    var project = projectArgument.Value;
                    var type = typeArgument.Value;

                    return await ListWorkItemsAsync(manager, project, type, Console.Out);
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
                    var manager = SetupManager(sourceNameOption.Value());

                    var project = projectArgument.Value;
                    var type = typeArgument.Value;

                    return await NewWorkItemAsync(manager, project, type);
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
                    var manager = SetupManager(sourceNameOption.Value());

                    var project = projectArgument.Value;
                    var id = idArgument.Value;

                    return await EditWorkItemAsync(manager, project, id);
                });
            });

            return app.ExecuteAsync(args);
        }

        private static WorkItemManager SetupManager(string sourceName)
        {
            sourceName ??= "default";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("wi.config.json", true)
                .Build();

            var options = configuration.Get<CliOptions>() ?? throw new InvalidOperationException("No Configuration can be read.");

            var provider = BuildProvider(options.Sources.FirstOrDefault(s => s.Name == sourceName));

            var manager = new WorkItemManager(provider, new CommonSdlcDescriptorProvider());

            return manager;
        }

        private static IDataProvider BuildProvider(DataSourceDescriptor dataSourceDescriptor)
        {
            var type = Type.GetType(dataSourceDescriptor.Type);

            var result = Activator.CreateInstance(type, dataSourceDescriptor.ConnectionString) as IDataProvider;

            return result;
        }

        private static async Task<int> DetailWorkItemAsync(WorkItemManager manager, string projectCode, string id, TextWriter writer)
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
            var logEntryTypeDescriptors = manager.DescriptorManager.GetCurrentLogEntryTypeDescriptors(workItem);

            if (workItem == null)
            {
                writer.WriteLine($"Could not find the specified workitem {id} in project {projectCode}");
            }
            else
            {
                var formatter = new WorkItemFormatter();

                await formatter.FormatAsync(logEntryTypeDescriptors, workItem, writer);
            }

            return (workItem != null) ? 0 : 1;
        }

        private static async Task<int> ListWorkItemsAsync(WorkItemManager workItemManager, string project, string type, TextWriter writer)
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

            var items = await workItemManager.DataProvider.ListWorkItemsAsync(project, type);

            var formatter = new WorkItemFormatter();

            foreach (var item in items)
            {
                writer.WriteLine(formatter.FormatShortLine(item));
            }

            return 0;
        }

        private static async Task<int> NewWorkItemAsync(WorkItemManager manager, string projectCode, string workItemType)
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

            var result = await manager.CreateAsync(projectCode, workItemType, properties);

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


        private static async Task<int> EditWorkItemAsync(WorkItemManager manager, string projectCode, string id)
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

            var wi = await manager.GetAsync(projectCode, id);

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

            var result = await manager.UpdateAsync(wi.ProjectCode, wi.Id, changedProperties);

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
