namespace Violet.WorkItems
{
    public class PropertyChange
    {
        public string Name { get; }
        public string OldValue { get; }
        public string NewValue { get; }

        public PropertyChange(string name, string oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("message", nameof(name));
            }

            Name = name;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}