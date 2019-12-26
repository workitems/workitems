using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Validation
{
    public class EnumValidator : IValidator
    {
        public EnumValueProviderDescriptor ValidatorDescriptor { get; }
        public PropertyDescriptor PropertyDescriptor { get; }

        public EnumValidator(PropertyDescriptor propertyDescriptor, EnumValueProviderDescriptor validatorDescriptor)
        {
            PropertyDescriptor = propertyDescriptor;
            ValidatorDescriptor = validatorDescriptor;
        }

        public Task<IEnumerable<ErrorMessage>> ValidatePropertyAsync(WorkItem workItem, IEnumerable<PropertyChange> appliedChanges)
        {
            var result = Array.Empty<ErrorMessage>();

            var propertyValue = workItem.Properties.FirstOrDefault(p => p.Name == PropertyDescriptor.Name)?.Value;

            var valueMatchesEnumEntry = ValidatorDescriptor.Values.Any(vd => vd.Value == propertyValue);

            if (!valueMatchesEnumEntry)
            {
                result = new ErrorMessage[] {
                    new ErrorMessage(nameof(EnumValidator), string.Empty, $"Property {PropertyDescriptor.Name} value '{propertyValue}' does not match one of the enum values.", workItem.ProjectCode, workItem.Id, PropertyDescriptor.Name),
                };
            }

            return Task.FromResult((IEnumerable<ErrorMessage>)result);
        }
    }
}