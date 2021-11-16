using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Validation;

namespace Violet.WorkItems;

public partial class WorkItemManager
{
    private async Task<Context> ValidateMiddlewareAsync(Context context, bool internalEdit)
    {
        var workItemFeature = context.Get<WorkItemFeature>();
        var changesFeature = context.Get<ChangesFeature>();

        if (workItemFeature is not null && changesFeature is not null)
        {
            var errors = await ValidationManager.ValidateAsync(workItemFeature.WorkItem, changesFeature.Changes, internalEdit);

            context = context.AddErrors(errors);
        }

        return context;
    }
    private async Task<Context> PersistMiddlewareAsync(Context context)
    {
        var workItemFeature = context.Get<WorkItemFeature>();
        var createFeature = context.Get<CreateRequestFeature>();
        var errorFeature = context.Get<ErrorFeature>() ?? new ErrorFeature(ImmutableArray<ErrorMessage>.Empty);

        if (workItemFeature is not null && errorFeature is not null)
        {
            if (DataProvider is { Write: false })
            {
                throw new InvalidOperationException("DataProvider does not allow write operations");
            }

            if (!errorFeature.Errors.Any())
            {
                if (createFeature is null)
                {
                    await DataProvider.SaveUpdatedWorkItemAsync(workItemFeature.WorkItem);
                }
                else
                {
                    await DataProvider.SaveNewWorkItemAsync(workItemFeature.WorkItem);
                }
            }
        }

        return context;
    }
}