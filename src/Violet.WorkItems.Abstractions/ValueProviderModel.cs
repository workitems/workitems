using System;

namespace Violet.WorkItems.ValueProvider;

public record ProvidedValue(string Value, string DisplayText, bool IsAllowed = true);