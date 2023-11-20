using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Violet.WorkItems;

namespace Violet.WorkItems.Service;

public class WorkItemHost : IHostedService
{
    private readonly WorkItemManager _workItemManager;

    public WorkItemHost(WorkItemManager workItemManager)
    {
        _workItemManager = workItemManager;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _workItemManager.InitAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}