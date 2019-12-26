using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Validation
{
    public class MandatoryValidator : IValidator
    {
        public MandatoryValidatorDescriptor ValidatorDescriptor { get; }
        public PropertyDescriptor PropertyDescriptor { get; }

        public MandatoryValidator(PropertyDescriptor propertyDescriptor, MandatoryValidatorDescriptor validatorDescriptor)
        {
            PropertyDescriptor = propertyDescriptor;
            ValidatorDescriptor = validatorDescriptor;
        }

        public Task<IEnumerable<ErrorMessage>> ValidatePropertyAsync(WorkItem workItem, IEnumerable<PropertyChange> appliedChanges)
        {
            var result = Array.Empty<ErrorMessage>();

            var property = workItem.Properties.FirstOrDefault(p => p.Name == PropertyDescriptor.Name);

            var hasValue = !string.IsNullOrWhiteSpace(property?.Value);

            if (!hasValue)
            {
                result = new ErrorMessage[] {
                    new ErrorMessage(nameof(MandatoryValidator), string.Empty, $"Property {PropertyDescriptor.Name} cannot be empty", workItem.ProjectCode, workItem.Id, PropertyDescriptor.Name),
                };
            }

            return Task.FromResult((IEnumerable<ErrorMessage>)result);
        }
    }
}