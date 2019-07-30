using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Violet.WorkItems
{
    public class WorkItem
    {
        public string ProjectCode { get; }
        public string Id { get; }
        public string WorkItemType { get; }
        public ImmutableArray<Property> Properties { get; }
        public ImmutableArray<LogEntry> LogEntries { get; set; }

        public WorkItem(string projectCode, string id, string workItemType, IEnumerable<Property> properties, IEnumerable<LogEntry> logEntries)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
            {
                throw new ArgumentException("null or empty", nameof(projectCode));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("null or empty", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(workItemType))
            {
                throw new ArgumentException("null or empty", nameof(workItemType));
            }

            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (logEntries == null)
            {
                throw new ArgumentNullException(nameof(logEntries));
            }

            ProjectCode = projectCode;
            Id = id;
            WorkItemType = workItemType;
            Properties = properties.ToImmutableArray();
            LogEntries = logEntries.ToImmutableArray();
        }

        public Property this[string propertyName]
            => Properties.FirstOrDefault(property => property.Name == propertyName);
    }
}
