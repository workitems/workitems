namespace Violet.WorkItems.Types
{
    public abstract class CommandDescriptor
    {
        public CommandDescriptor(string name, string type, string displayName)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("message", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                throw new System.ArgumentException("message", nameof(type));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new System.ArgumentException("message", nameof(displayName));
            }

            Name = name;
            Type = type;
            DisplayName = displayName;
        }

        public string Name { get; }
        public string Type { get; }
        public string DisplayName { get; }
    }
}