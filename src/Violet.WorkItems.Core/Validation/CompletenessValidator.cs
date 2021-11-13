using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Validation;

public class CompletenessValidator : IValidator
{
    public IEnumerable<PropertyDescriptor> PropertyDescriptors { get; }

    public CompletenessValidator(IEnumerable<PropertyDescriptor> propertyDescriptors)
    {
        PropertyDescriptors = propertyDescriptors ?? throw new System.ArgumentNullException(nameof(propertyDescriptors));
    }

    public Task<IEnumerable<ErrorMessage>> ValidateAsync(ValidationContext context)
    {
        if (context is null || context.WorkItem is null)
        {
            throw new System.ArgumentNullException(nameof(context));
        }

        var workItem = context.WorkItem;

        var errors = new List<ErrorMessage>();
        foreach (var propertyDescriptor in PropertyDescriptors)
        {
            var property = context.WorkItem.Properties.FirstOrDefault(p => p.Name == propertyDescriptor.Name);

            if (property is null)
            {
                errors.Add(new ErrorMessage(nameof(CompletenessValidator), string.Empty, $"WorkItem does not have property '{propertyDescriptor.Name}' as described in type '{workItem.WorkItemType}.", workItem.ProjectCode, workItem.Id, propertyDescriptor.Name));
            }

            if (property is not null && property.DataType != propertyDescriptor.DataType)
            {
                errors.Add(new ErrorMessage(nameof(CompletenessValidator), string.Empty, $"DataType '{property.DataType}' on Property '{property.Name}' does not match description '{propertyDescriptor.DataType}'.", workItem.ProjectCode, workItem.Id, property.Name));
            }
        }

        return Task.FromResult((IEnumerable<ErrorMessage>)errors);
    }
}
