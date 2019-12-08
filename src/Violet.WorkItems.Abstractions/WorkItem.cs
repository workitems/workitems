using System;
using System.Collections.Generic;
using System.Linq;

namespace Violet.WorkItems
{
    public class WorkItem
    {
        public string ProjectCode { get; set; }
        public string Id { get; set; }
        public string WorkItemType { get; set; }
        public IEnumerable<Property> Properties { get; set; }
        public IEnumerable<LogEntry> Log { get; set; }

        public WorkItem() { }
        public WorkItem(string projectCode, string id, string workItemType, IEnumerable<Property> properties, IEnumerable<LogEntry> log)
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

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            ProjectCode = projectCode;
            Id = id;
            WorkItemType = workItemType;
            Properties = properties;
            Log = log;
        }

        public Property this[string propertyName]
            => Properties.FirstOrDefault(property => property.Name == propertyName);
    }
}
