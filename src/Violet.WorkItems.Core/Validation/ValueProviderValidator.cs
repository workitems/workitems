using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;
using Violet.WorkItems.ValueProvider;

namespace Violet.WorkItems.Validation
{
    public class ValueProviderValidator : PropertyWithPropertyDescriptorValidatorBase, IValidator
    {
        public IValueProvider ValueProvider { get; }

        public ValueProviderValidator(PropertyDescriptor propertyDescriptor, IValueProvider valueProvider)
            : base(propertyDescriptor, nameof(ValueProviderValidator))
        {
            ValueProvider = valueProvider;
        }

        protected override async Task<(bool success, string code, string message)> ValidatePropertyAsync(Property property)
        {
            var code = string.Empty;
            var message = string.Empty;

            var valueExists = await ValueProvider.ValueExistsAsync(property.Value);

            if (!valueExists)
            {
                code = string.Empty;
                message = $"Property {PropertyDescriptor.Name} value '{property.Value}' does not match one of the allowed values.";
            }

            return (valueExists, code, message);
        }
    }
}