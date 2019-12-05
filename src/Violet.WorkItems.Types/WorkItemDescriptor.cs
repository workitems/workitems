using System.Collections.Generic;
using System.Collections.Immutable;

namespace Violet.WorkItems.Types
{
    public class WorkItemDescriptor
    {
        public WorkItemDescriptor(string name, LogDescriptor log, IEnumerable<PropertyDescriptor> properties, IEnumerable<StageDescriptor> stages)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("message", nameof(name));
            }

            Name = name;
            Log = log ?? throw new System.ArgumentNullException(nameof(log));
            Stages = stages.ToImmutableArray();
            Properties = properties.ToImmutableArray();
        }

        public string Name { get; }
        public LogDescriptor Log { get; }
        public ImmutableArray<StageDescriptor> Stages { get; }
        public ImmutableArray<PropertyDescriptor> Properties { get; }
    }
}