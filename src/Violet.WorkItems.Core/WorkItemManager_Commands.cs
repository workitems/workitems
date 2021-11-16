using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;
using Violet.WorkItems.Validation;

namespace Violet.WorkItems;

public partial class WorkItemManager
{
    private async Task<Context> ApplyCommandMiddlewareAsync(Context context)
    {
        var executeFeature = context.Get<ExecuteCommandRequestFeature>() ?? throw new ArgumentException("feature missing", nameof(context));

        var workItem = await GetAsync(executeFeature.ProjectCode, executeFeature.Id);

        if (workItem != null)
        {
            var commands = DescriptorManager.GetCurrentCommands(workItem);
            var commandDescriptor = commands?.FirstOrDefault(c => c.Name == executeFeature.Command || c.Label == executeFeature.Command);

            if (commandDescriptor != null)
            {
                var changes = await ExecuteCommandToPropertyChangesAsync(workItem, commandDescriptor);

                context = context
                    .With(new ChangesFeature(workItem, changes.ToImmutableArray(), false))
                    .With(new UpdatePropertiesRequestFeature(executeFeature.ProjectCode, executeFeature.Id, workItem.Properties));
            }
            else
            {
                context = context.AddError(new ErrorMessage(nameof(WorkItemManager), string.Empty, $"The command with name '{executeFeature.Command}' cannot be found in work item with id '{executeFeature.Id}' in project '{executeFeature.ProjectCode}'.", executeFeature.ProjectCode, executeFeature.Id, string.Empty));
            }
        }
        else
        {
            context = context.AddError(new ErrorMessage(nameof(WorkItemManager), string.Empty, $"The work item with id '{executeFeature.Id}' in project '{executeFeature.ProjectCode}' cannot be found.", executeFeature.ProjectCode, string.Empty, string.Empty));
        }

        return context;
    }

    private static Task<IEnumerable<PropertyChange>> ExecuteCommandToPropertyChangesAsync(WorkItem workItem, CommandDescriptor commandDescriptor)
        => commandDescriptor switch
        {
            ChangePropertyValueCommandDescriptor cpvcd => Task.FromResult<IEnumerable<PropertyChange>>(new PropertyChange[] {
                new PropertyChange(cpvcd.PropertyName, workItem[cpvcd.PropertyName]?.Value ?? throw new InvalidOperationException("work item lost item meanwhile"), cpvcd.TargetValue),
            }),
            _ => throw new ArgumentException("Command not supported", nameof(commandDescriptor)),
        };
}