using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Violet.WorkItems
{
    public class ChangeHistoryEntry : LogEntry
    {
        public ImmutableArray<PropertyChange> Changes { get; protected set; }

        public ChangeHistoryEntry(IEnumerable<PropertyChange> changes, DateTimeOffset date, string user)
            : base(date, user)
        {
            if (changes == null)
            {
                throw new ArgumentNullException(nameof(changes));
            }

            Changes = changes.ToImmutableArray();
        }
    }
}