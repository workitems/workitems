using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Validation;

namespace Violet.WorkItems;

public partial class WorkItemManager
{
    private bool _initialized = false;

    private async Task<Context> InitMiddelwareAsync(Context context)
    {
        await InitAsync();

        return context;
    }

    public async Task InitAsync()
    {
        if (!_initialized) //TODO: make it thread-safe
        {
            await DescriptorManager.LoadAllAsync();

            _initialized = true;
        }
    }

    private static WorkItemChangedResult CreateChangedResult(Context context)
    {
        var errorFeature = context.Get<ErrorFeature>() ?? new ErrorFeature(ImmutableArray<ErrorMessage>.Empty);
        var workItemFeature = context.Get<WorkItemFeature>();

        return !errorFeature.Errors.Any()
            ? new WorkItemChangedResult(true, workItemFeature?.WorkItem, Array.Empty<ErrorMessage>())
            : new WorkItemChangedResult(false, workItemFeature?.WorkItem, errorFeature.Errors);
    }
}