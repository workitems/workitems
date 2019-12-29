using System;

namespace Violet.WorkItems.ValueProvider
{
    public class ProvidedValue
    {
        public string Value { get; }
        public string DisplayText { get; }
        public bool IsAllowed { get; }

        public ProvidedValue(string value, string displayText, bool isAllowed = true)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            DisplayText = displayText ?? throw new ArgumentNullException(nameof(displayText));
            IsAllowed = isAllowed;
        }
    }
}