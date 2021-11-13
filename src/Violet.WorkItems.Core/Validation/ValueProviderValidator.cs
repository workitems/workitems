using System.Collections.Generic;
using System.Threading.Tasks;
using Violet.WorkItems.Types;
using Violet.WorkItems.ValueProvider;

namespace Violet.WorkItems.Validation;

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

        foreach (var value in MultiValues(PropertyDescriptor, context.Property))
        {
            if (value != Property.NullValue) // null values are not checked against a value provider. MandatoryValidator is used for that.
            {
                if (!ValueProvider.IsValidEncoding(value))
                {
                    success = false;
                    code = string.Empty;
                    message = $"Property {PropertyDescriptor.Name} value '{value}' does not match allowed encoding for ValueProvider '{ValueProvider.GetType().Name}'.";
                }
                else
                {
                    var exists = await ValueProvider.ValueExistsAsync(value);

                    if (!exists)
                    {
                        success = false;
                        code = string.Empty;
                        message = $"Property {PropertyDescriptor.Name} value '{value}' does not match one of the allowed values.";
                    }
                }
            }

        }

        return (success, code, message);
    }

    private IEnumerable<string> MultiValues(PropertyDescriptor propertyDescriptor, Property property)
    {
        if (propertyDescriptor.PropertyType == PropertyType.SingleValueFromProvider || propertyDescriptor.PropertyType == PropertyType.SingleRaw)
        {
            return new string[] { property.Value };
        }
        else
        {
            return property.Value.Split(',');
        }
    }
}
