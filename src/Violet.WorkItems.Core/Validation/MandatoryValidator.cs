using Violet.WorkItems.Types;

namespace Violet.WorkItems.Validation
{
    public class MandatoryValidator : PropertyWithValidatorDescriptorValidatorBase<MandatoryValidatorDescriptor>, IValidator
    {
        public MandatoryValidator(PropertyDescriptor propertyDescriptor, MandatoryValidatorDescriptor validatorDescriptor)
            : base(propertyDescriptor, validatorDescriptor, nameof(MandatoryValidator))
        { }

        protected override bool ValidateProperty(Property property, out string code, out string message)
        {
            code = string.Empty;
            message = string.Empty;

            var hasValue = !string.IsNullOrWhiteSpace(property?.Value);

            if (!hasValue)
            {
                code = string.Empty;
                message = $"Property {PropertyDescriptor.Name} cannot be empty";
            }

            return hasValue;
        }
    }
}