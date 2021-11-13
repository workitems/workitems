using System;

namespace Violet.WorkItems.Types;

public record LogEntryTypeDescriptor(string Name, string Type);

public record PropertyChangeLogEntryTypeDescriptor(string Name, string PropertyName, string Emoji, string Message, string? TargetValue = null)
    : LogEntryTypeDescriptor(Name, "PropertyChange");
