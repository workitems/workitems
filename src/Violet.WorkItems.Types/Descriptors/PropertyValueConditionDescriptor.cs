using System;

namespace Violet.WorkItems.Types
{
    public class PropertyValueConditionDescriptor : ConditionDescriptor
    {
        public PropertyValueConditionDescriptor(string propertyName, string expectedValue)
            : base("PropertyValue")
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("message", nameof(propertyName));
            }

            PropertyName = propertyName;
            ExpectedValue = expectedValue ?? throw new ArgumentNullException(nameof(expectedValue));
        }

        public string PropertyName { get; }
        public string ExpectedValue { get; }
    }
}