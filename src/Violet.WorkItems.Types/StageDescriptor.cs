using System.Collections.Generic;
using System.Collections.Immutable;

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
            Commands = commands?.ToImmutableArray() ?? ImmutableArray<CommandDescriptor>.Empty;
            Properties = properties?.ToImmutableArray() ?? ImmutableArray<StagePropertyDescriptor>.Empty;
        }

        public string Name { get; }
        public ConditionDescriptor Condition { get; }
        public ImmutableArray<CommandDescriptor> Commands { get; }
        public ImmutableArray<StagePropertyDescriptor> Properties { get; }
    }
}