using System;
using System.Collections.Generic;

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
            Stages = stages ?? Array.Empty<StageDescriptor>();
            Properties = properties ?? Array.Empty<PropertyDescriptor>();
        }

        public string Name { get; }
        public LogDescriptor Log { get; }
        public IEnumerable<StageDescriptor> Stages { get; }
        public IEnumerable<PropertyDescriptor> Properties { get; }
    }
}