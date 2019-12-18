using System;

namespace Violet.WorkItems.Types
{
    public abstract class ValidatorDescriptor
    {
        public ValidatorDescriptor(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("message", nameof(type));
            }

            Type = type;
        }

        public string Type { get; }
    }
}