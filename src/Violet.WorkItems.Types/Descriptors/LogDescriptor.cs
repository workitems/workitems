using System;
using System.Collections.Generic;

namespace Violet.WorkItems.Types
{
    public class LogDescriptor
    {

        public LogDescriptor(IEnumerable<LogEntryTypeDescriptor>? types = null)
        {
            Types = types ?? Array.Empty<LogEntryTypeDescriptor>();
        }

        public IEnumerable<LogEntryTypeDescriptor> Types { get; }
    }
}