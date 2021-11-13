using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types.CommonSdlc;

namespace Violet.WorkItems.Cli;

class Program
{
    static Task<int> Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var app = new CommandLineApplication();
        app.HelpOption();
        var sourceNameOption = app.Option("--source", "Specify the data source used for the commands.", CommandOptionType.SingleOrNoValue, true);
        var configFileOption = app.Option("--config", "Specify the configuration file used for the commands", CommandOptionType.SingleOrNoValue, true);

        app.Command("detail", command =>
        {
            command.HelpOption();
            command.Description = "List all Work Items of the specified type";

            var projectArgument = command.Argument("Project", "The project name in which work item should be shown", false)
                                        .IsRequired();
            var idArgument = command.Argument("Id", "The work item id", false)
                                        .IsRequired();

            command.OnExecuteAsync(async (ctx) =>
            {
                var manager = await SetupManagerAsync(sourceNameOption.Value(), configFileOption.Value());

                var project = projectArgument.Value;
                var id = idArgument.Value;

                return await DetailWorkItemCommand.ExecuteAsync(manager, project, id, Console.Out);
            });
        });

        app.Command("list", command =>
        {
            command.HelpOption();
            command.Description = "List all Work Items of the specified type";

            var projectArgument = command.Argument("Project", "The project name in which the work item should be listed", false)
                                        .IsRequired();
            var typeArgument = command.Argument("Type", "The type of work item which should be listed (optional)", false);

            command.OnExecuteAsync(async (ctx) =>
            {
                var manager = await SetupManagerAsync(sourceNameOption.Value(), configFileOption.Value());

                var project = projectArgument.Value;
                var type = typeArgument.Value;

                return await ListWorkItemsCommand.ExecuteAsync(manager, project, type, Console.Out);
            });
        });

        app.Command("new", command =>
        {
            command.HelpOption();
            command.Description = "Creates a new Work Item of a specified type";

            var projectArgument = command.Argument("Project", "The project name in which the work item should be created", false)
                                        .IsRequired();
            var typeArgument = command.Argument("Type", "The type of work item which should be created", false);

            command.OnExecuteAsync(async (ctx) =>
            {
                var manager = await SetupManagerAsync(sourceNameOption.Value(), configFileOption.Value());

                var project = projectArgument.Value;
                var type = typeArgument.Value;

                return await NewWorkItemCommand.ExecuteAsync(manager, project, type);
            });
        });

        app.Command("edit", command =>
        {
            command.HelpOption();
            command.Description = "Edit a specified work item";

            var projectArgument = command.Argument("Project", "The project name in which the work item should be edited", false)
                                        .IsRequired();
            var idArgument = command.Argument("Id", "The work item id", false)
                                        .IsRequired();

            command.OnExecuteAsync(async (ctx) =>
            {
                var manager = await SetupManagerAsync(sourceNameOption.Value(), configFileOption.Value());

                var project = projectArgument.Value;
                var id = idArgument.Value;

                return await EditWorkItemCommand.ExecuteAsync(manager, project, id);
            });
        });

        app.Command("command", command =>
        {
            command.HelpOption();
            command.Description = "Execute a command on a givem work item.";

            var projectArgument = command.Argument("Project", "The project name in which the work item's command should be executed", false)
                                        .IsRequired();
            var idArgument = command.Argument("Id", "The work item id", false)
                                        .IsRequired();
            var commandArgument = command.Argument("Command", "The name or display name of the work item command")
                                        .IsRequired();

            command.OnExecuteAsync(async (ctx) =>
            {
                var manager = await SetupManagerAsync(sourceNameOption.Value(), configFileOption.Value());

                var project = projectArgument.Value;
                var id = idArgument.Value;
                var commandInputValue = commandArgument.Value;

                var result = await ExecuteCommandWorkItemCommand.ExecuteAsync(manager, project, id, commandInputValue);

                await DetailWorkItemCommand.ExecuteAsync(manager, project, id, Console.Out);

                return result;
            });
        });

        return app.ExecuteAsync(args);
    }

    private static Task<WorkItemManager> SetupManagerAsync(string? sourceName, string? configFile)
    {
        sourceName ??= "default";

        configFile = FindFile(Environment.CurrentDirectory, configFile ?? "wi.config.json");

        if (configFile is null)
        {
            throw new ArgumentException("configFile cannot be found", nameof(ArgumentException));
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile(configFile, true)
            .Build();

        var options = configuration.Get<CliOptions>(options =>
        {
            options.BindNonPublicProperties = true;
        }) ?? throw new InvalidOperationException("No Configuration can be read.");

        var selectedSource = options.Sources.FirstOrDefault(s => s.Name == sourceName) ?? throw new InvalidOperationException("No matching source configuration could be found.");

        var provider = BuildProvider(selectedSource);

        var descriptorProvider = new CommonSdlcDescriptorProvider();

        var manager = new WorkItemManager(provider, new CommonSdlcDescriptorProvider());

        return Task.FromResult(manager);
    }

    private static string? FindFile(string directory, string configFile)
    {
        var result = Path.Combine(directory, configFile);

        var fileInfo = new FileInfo(result);

        if (!fileInfo.Exists)
        {
            if (fileInfo.Directory?.Parent != null)
            {
                return FindFile(fileInfo.Directory.Parent.FullName, configFile);
            }
            else
            {
                return null;
            }
        }

        return result;
    }

    private static IDataProvider BuildProvider(DataSourceDescriptor? dataSourceDescriptor)
    {
        if (dataSourceDescriptor is null)
        {
            throw new ArgumentNullException(nameof(dataSourceDescriptor));
        }

        var type = Type.GetType(dataSourceDescriptor.Type);

        if (type is null)
        {
            throw new ArgumentException("type has to be a valid .NET type", nameof(dataSourceDescriptor));
        }

        var result = Activator.CreateInstance(type, dataSourceDescriptor.ConnectionString) as IDataProvider;

        if (result is null)
        {
            throw new ArgumentException($"source type is not a valid {nameof(IDataProvider)}");
        }

        return result;
    }
}
