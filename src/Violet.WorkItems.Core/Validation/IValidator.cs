using System.Collections.Generic;
using System.Threading.Tasks;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Validation
{
    public interface IValidator
    {
        bool Accept(WorkItem workItem, WorkItemDescriptor descriptor, IEnumerable<PropertyChange> changes);

        Task<IEnumerable<ErrorMessage>> ValidateAsync(WorkItem workItems, WorkItemDescriptor descriptor, IEnumerable<PropertyChange> changes);
    }

    public class ErrorMessage
    {
    }
}