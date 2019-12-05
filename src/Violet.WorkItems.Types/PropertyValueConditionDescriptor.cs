using System;

namespace Violet.WorkItems.Types
{
    public class PropertyValueConditionDescriptor : ConditionDescriptor
    {
        public PropertyValueConditionDescriptor(string propertyName, string value)
            : base("PropertyValue")
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("message", nameof(propertyName));
            }

            PropertyName = propertyName;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string PropertyName { get; }
        public string Value { get; }
    }
}