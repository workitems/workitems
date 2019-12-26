using System;

namespace Violet.WorkItems.Types
{
    public class EnumValue
    {
        public EnumValue(string value, string displayText)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("message", nameof(value));
            }

            if (string.IsNullOrWhiteSpace(displayText))
            {
                throw new ArgumentException("message", nameof(displayText));
            }

            Value = value;
            DisplayText = displayText;
        }

        public string Value { get; }
        public string DisplayText { get; }
    }

}