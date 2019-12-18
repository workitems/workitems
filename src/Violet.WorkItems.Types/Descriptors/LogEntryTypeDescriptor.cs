using System;

namespace Violet.WorkItems.Types
{
    public class LogEntryTypeDescriptor
    {
        public LogEntryTypeDescriptor(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("message", nameof(name));
            }

            Name = name;
        }

        public string Name { get; }
    }
}