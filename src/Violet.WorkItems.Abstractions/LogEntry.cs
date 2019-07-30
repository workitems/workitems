using System;
using System.Collections.Immutable;

namespace Violet.WorkItems
{
    public abstract class LogEntry
    {
        protected LogEntry(DateTimeOffset date, string user)
        {
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentException("user cannot be null or empty", nameof(user));
            }

            Date = date;
            User = user;
        }
        public DateTimeOffset Date { get; protected set; }
        public string User { get; }
    }
}