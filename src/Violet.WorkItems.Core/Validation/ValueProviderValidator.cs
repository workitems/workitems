using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;
using Violet.WorkItems.ValueProvider;

namespace Violet.WorkItems.Validation
{
    public class ValueProviderValidator : IValidator
    {
        public PropertyDescriptor PropertyDescriptor { get; }
        public IValueProvider ValueProvider { get; }

        public ValueProviderValidator(PropertyDescriptor propertyDescriptor, IValueProvider valueProvider)
        {
            PropertyDescriptor = propertyDescriptor;
            ValueProvider = valueProvider;
        }

        public async Task<IEnumerable<ErrorMessage>> ValidateAsync(WorkItem workItem, IEnumerable<PropertyChange> appliedChanges)
        {
            var result = Array.Empty<ErrorMessage>();

            var property = workItem.Properties.FirstOrDefault(p => p.Name == PropertyDescriptor.Name);

            if (property != null)
            {
                var valueMatchesEnumEntry = await ValueProvider.ValueExistsAsync(property.Value);

                if (!valueMatchesEnumEntry)
                {
                    result = new ErrorMessage[] {
                        new ErrorMessage(nameof(ValueProviderValidator), string.Empty, $"Property {PropertyDescriptor.Name} value '{property.Value}' does not match one of the allowed values.", workItem.ProjectCode, workItem.Id, PropertyDescriptor.Name),
                    };
                }
            }

            return result;
        }
    }
}