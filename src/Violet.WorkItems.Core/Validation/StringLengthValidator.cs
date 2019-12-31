using Violet.WorkItems.Types;

namespace Violet.WorkItems.Validation
{
    public class StringLengthValidator : PropertyWithValidatorDescriptorValidatorBase<StringLengthValidatorDescriptor>, IValidator
    {
        public StringLengthValidator(PropertyDescriptor propertyDescriptor, StringLengthValidatorDescriptor validatorDescriptor)
            : base(propertyDescriptor, validatorDescriptor, nameof(StringLengthValidator))
        { }

        protected override bool ValidateProperty(Property property, out string code, out string message)
        {
            code = string.Empty;
            message = string.Empty;

            bool result = true;

            if (property.DataType == "String")
            {
                property.Value(out string valueAsString);

                int length = valueAsString.Length;

                if (length < ValidatorDescriptor.Min || length > ValidatorDescriptor.Max)
                {
                    code = string.Empty;
                    message = $"Property {PropertyDescriptor.Name} cannot be empty";

                    result = false;
                }
            }

            return result;
        }
    }
}