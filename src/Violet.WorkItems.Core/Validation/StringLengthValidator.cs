using Violet.WorkItems.Types;

namespace Violet.WorkItems.Validation;

public class StringLengthValidator : PropertyWithValidatorDescriptorValidatorBase<StringLengthValidatorDescriptor>, IValidator
{
    public StringLengthValidator(PropertyDescriptor propertyDescriptor, StringLengthValidatorDescriptor validatorDescriptor)
        : base(propertyDescriptor, validatorDescriptor, nameof(StringLengthValidator))
    { }

    protected override (bool success, string code, string message) ValidateProperty(PropertyValidationContext context)
    {
        var code = string.Empty;
        var message = string.Empty;

        bool result = true;

        var property = context.Property;

        if (property.DataType == "String")
        {
            property.As(out string valueAsString);

            int length = valueAsString.Length;

            if (length < ValidatorDescriptor.Min || length > ValidatorDescriptor.Max)
            {
                code = string.Empty;
                message = $"Property {PropertyDescriptor.Name} cannot be empty";

                result = false;
            }
        }

        return (result, code, message);
    }
}
