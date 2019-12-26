using System.Collections.Generic;
using System.Threading.Tasks;

namespace Violet.WorkItems.Validation
{
    public interface IValidator
    {
        Task<IEnumerable<ErrorMessage>> ValidatePropertyAsync(WorkItem workItem, IEnumerable<PropertyChange> appliedChanges);
    }
}