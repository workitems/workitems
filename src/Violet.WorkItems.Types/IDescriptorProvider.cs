using System.Collections.Generic;
using System.Threading.Tasks;

namespace Violet.WorkItems.Types
{
    public interface IDescriptorProvider
    {
        Task<IEnumerable<WorkItemDescriptor>> GetAllDescriptorsAsync();
    }
}