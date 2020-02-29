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

        protected override async Task<(bool success, string code, string message)> ValidatePropertyAsync(PropertyValidationContext context)
        {
            var success = true;
            var code = string.Empty;
            var message = string.Empty;

            var property = context.Property;

            if (property.Value != Property.NullValue) // null values are not checked against a value provider. MandatoryValidator is used for that.
            {
                success = await ValueProvider.ValueExistsAsync(property.Value);

                if (!success)
                {
                    code = string.Empty;
                    message = $"Property {PropertyDescriptor.Name} value '{property.Value}' does not match one of the allowed values.";
                }
            }

            return (success, code, message);
        }
    }
}