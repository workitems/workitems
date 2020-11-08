namespace Violet.WorkItems.Types
{
    public abstract class CommandDescriptor
    {
        public CommandDescriptor(string name, string type, string label)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("message", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                throw new System.ArgumentException("message", nameof(type));
            }

            if (string.IsNullOrWhiteSpace(label))
            {
                throw new System.ArgumentException("message", nameof(label));
            }

            Name = name;
            Type = type;
            Label = label;
        }

        public string Name { get; }
        public string Type { get; }
        public string Label { get; }
    }
}