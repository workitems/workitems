using System;
using System.Collections.Generic;

namespace Violet.WorkItems.Types
{
    public class StageDescriptor
    {
        public StageDescriptor(string name, ConditionDescriptor condition, IEnumerable<StagePropertyDescriptor> properties, IEnumerable<CommandDescriptor> commands)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("message", nameof(name));
            }

            Name = name;
            Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            Commands = commands ?? Array.Empty<CommandDescriptor>();
            Properties = properties ?? Array.Empty<StagePropertyDescriptor>();
        }

        public string Name { get; }
        public ConditionDescriptor Condition { get; }
        public IEnumerable<CommandDescriptor> Commands { get; }
        public IEnumerable<StagePropertyDescriptor> Properties { get; }
    }
}