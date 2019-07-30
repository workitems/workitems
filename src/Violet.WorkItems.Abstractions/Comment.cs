using System;

namespace Violet.WorkItems
{
    public class Comment : LogEntry
    {
        public string Text { get; }

        public Comment(string text, DateTimeOffset date, string user)
            : base(date, user)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("parameter cannot be null or empty", nameof(text));
            }

            Text = text;
        }
    }
}