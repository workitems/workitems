namespace Violet.WorkItems.Types;

public record CommandDescriptor(string Name, string Type, string Label);

public record ChangePropertyValueCommandDescriptor(string Name, string Label, string PropertyName, string TargetValue)
    : CommandDescriptor(Name, "ChangePropertyValue", Label);
