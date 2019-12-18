using System;

namespace Violet.WorkItems.Types
{
    public abstract class ConditionDescriptor
    {
        public ConditionDescriptor(string type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public string Type { get; }
    }
}