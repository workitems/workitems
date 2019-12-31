using System;
using System.Collections.Generic;

namespace Violet.WorkItems
{
    public class LogEntry
    {
        public DateTimeOffset Date { get; set; }
        public string User { get; set; }
        public string Comment { get; set; }
        public IEnumerable<PropertyChange> Changes { get; set; }

        public LogEntry()
        {
            Date = DateTimeOffset.MinValue;
            User = string.Empty;
            Comment = string.Empty;
            Changes = Array.Empty<PropertyChange>();
        }

        public LogEntry(DateTimeOffset date, string user, string comment, IEnumerable<PropertyChange> changes)
        {
            Date = date;
            User = user ?? throw new ArgumentNullException(nameof(user));
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
            Changes = changes ?? throw new ArgumentNullException(nameof(changes));
        }
    }
}