using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Violet.WorkItems
{
    public class LogEntry
    {
        protected LogEntry(DateTimeOffset date, string user, string comment, IEnumerable<PropertyChange> changes)
        {
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentException("user cannot be null or empty", nameof(user));
            }

            if (changes is null)
            {
                throw new ArgumentNullException(nameof(changes));
            }

            Date = date;
            User = user;
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
            Changes = changes.ToImmutableArray();
        }
        public DateTimeOffset Date { get; }
        public string User { get; }
        public string Comment { get; }
        public ImmutableArray<PropertyChange> Changes { get; }
    }
}