namespace Violet.WorkItems.Types
{
    public class ChangePropertyValueCommandDescriptor : CommandDescriptor
    {
        public ChangePropertyValueCommandDescriptor(string name, string displayName, string propertyName, string targetValue)
            : base(name, "ChangePropertyValue", displayName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new System.ArgumentException("message", nameof(propertyName));
            }

            PropertyName = propertyName;
            TargetValue = targetValue;
        }

        public string PropertyName { get; }
        public string TargetValue { get; }
    }
}