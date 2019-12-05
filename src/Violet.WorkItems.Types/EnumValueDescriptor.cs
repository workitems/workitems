using System;

namespace Violet.WorkItems.Types
{
    public class EnumValueDescriptor
    {
        public EnumValueDescriptor(string value, string displayText)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            DisplayText = displayText ?? throw new ArgumentNullException(nameof(displayText));
        }

        public string Value { get; }
        public string DisplayText { get; }
    }
}