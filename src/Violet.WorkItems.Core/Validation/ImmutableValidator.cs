using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Validation
{
    public class ImmutableValidator : IValidator
    {
        public PropertyDescriptor PropertyDescriptor { get; }

        public ImmutableValidator(PropertyDescriptor propertyDescriptor)
        {
            PropertyDescriptor = propertyDescriptor ?? throw new System.ArgumentNullException(nameof(propertyDescriptor));
        }


        public Task<IEnumerable<ErrorMessage>> ValidateAsync(WorkItem workItem, IEnumerable<PropertyChange> appliedChanges)
        {
            if (workItem is null)
            {
                throw new System.ArgumentNullException(nameof(workItem));
            }

            if (appliedChanges is null)
            {
                throw new System.ArgumentNullException(nameof(appliedChanges));
            }

            var errors = new List<ErrorMessage>();
            var propertyChange = appliedChanges.FirstOrDefault(c => c.Name == PropertyDescriptor.Name);

            if (!PropertyDescriptor.IsEditable && !(propertyChange is null))
            {
                errors.Add(new ErrorMessage(nameof(ImmutableValidator), string.Empty, "The field is currently not editable", workItem.ProjectCode, workItem.Id, PropertyDescriptor.Name));
            }

            return Task.FromResult((IEnumerable<ErrorMessage>)errors);
        }
    }
}