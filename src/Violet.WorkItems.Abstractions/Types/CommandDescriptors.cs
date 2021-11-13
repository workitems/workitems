namespace Violet.WorkItems.Types;

public abstract record CommandDescriptor(string Name, string Type, string Label);

public record ChangePropertyValueCommandDescriptor(string Name, string Label, string PropertyName, string TargetValue)
    : CommandDescriptor(Name, "ChangePropertyValue", Label);
