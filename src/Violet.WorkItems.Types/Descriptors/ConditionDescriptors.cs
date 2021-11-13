using System;

namespace Violet.WorkItems.Types;

public abstract record ConditionDescriptor(string Type);

public record PropertyValueConditionDescriptor(string PropertyName, string ExpectedValue) : ConditionDescriptor("PropertyValue");
