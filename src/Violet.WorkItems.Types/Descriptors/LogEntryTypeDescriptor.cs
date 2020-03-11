using System;

namespace Violet.WorkItems.Types
{
    public class LogEntryTypeDescriptor
    {
        public LogEntryTypeDescriptor(string name, string type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("message", nameof(name));
            }

            Name = name;
            Type = type;
        }

        public string Name { get; }
        public string Type { get; }
    }
}