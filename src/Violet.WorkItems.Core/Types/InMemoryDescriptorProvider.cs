using System.Collections.Generic;
using System.Threading.Tasks;

namespace Violet.WorkItems.Types;

public class InMemoryDescriptorProvider : IDescriptorProvider
{
    private readonly IEnumerable<WorkItemDescriptor> _descriptors;

    public InMemoryDescriptorProvider(params WorkItemDescriptor[] descriptors)
    {
        _descriptors = descriptors;
    }
    public InMemoryDescriptorProvider(IEnumerable<WorkItemDescriptor> descriptors)
    {
        _descriptors = descriptors;
    }
    public Task<IEnumerable<WorkItemDescriptor>> GetAllDescriptorsAsync()
        => Task.FromResult(_descriptors);
}
