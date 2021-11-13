using System;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Validation;

public class MandatoryValidator : PropertyWithValidatorDescriptorValidatorBase<MandatoryValidatorDescriptor>, IValidator
{
    public MandatoryValidator(PropertyDescriptor propertyDescriptor, MandatoryValidatorDescriptor validatorDescriptor)
        : base(propertyDescriptor, validatorDescriptor, nameof(MandatoryValidator))
    { }

    protected override (bool success, string code, string message) ValidateProperty(PropertyValidationContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        string code = string.Empty;
        string message = string.Empty;

        var hasValue = !string.IsNullOrWhiteSpace(context.Property?.Value);

        if (!hasValue)
        {
            code = string.Empty;
            message = $"Property {PropertyDescriptor.Name} cannot be empty";
        }

        return (hasValue, code, message);
    }
}
