namespace Violet.WorkItems.Types
{
    public class PropertyChangeLogEntryTypeDescriptor : LogEntryTypeDescriptor
    {
        public PropertyChangeLogEntryTypeDescriptor(string name, string propertyName, string emoji, string message, string? targetValue = null)
            : base(name)
        {
            PropertyName = propertyName;
            Emoji = emoji;
            Message = message;
            TargetValue = targetValue;
        }

        public string PropertyName { get; }
        public string Emoji { get; }
        public string Message { get; }
        public string? TargetValue { get; }
    }
}