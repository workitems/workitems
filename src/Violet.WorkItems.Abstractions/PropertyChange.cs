namespace Violet.WorkItems
{
    public class PropertyChange
    {
        public string Name { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

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