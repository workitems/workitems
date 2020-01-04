using System;
using System.Collections.Generic;

namespace Violet.WorkItems.Types
{
    public class WorkItemDescriptor
    {
        public WorkItemDescriptor(string name, IEnumerable<PropertyDescriptor> properties, IEnumerable<StageDescriptor>? stages = null, LogDescriptor? log = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("message", nameof(name));
            }

            Name = name;
            Log = log ?? new LogDescriptor(Array.Empty<LogEntryTypeDescriptor>());
            Stages = stages ?? Array.Empty<StageDescriptor>();
            Properties = properties ?? Array.Empty<PropertyDescriptor>();
        }

        public string Name { get; }
        public LogDescriptor Log { get; }
        public IEnumerable<StageDescriptor> Stages { get; }
        public IEnumerable<PropertyDescriptor> Properties { get; }
    }
}