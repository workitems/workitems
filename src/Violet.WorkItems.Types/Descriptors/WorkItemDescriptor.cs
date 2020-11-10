using System;
using System.Collections.Generic;

namespace Violet.WorkItems.Types
{
    public record WorkItemDescriptor(string Name, IEnumerable<PropertyDescriptor> Properties, IEnumerable<StageDescriptor> Stages, LogDescriptor Log)
    {
        public static WorkItemDescriptor Create(string name, IEnumerable<PropertyDescriptor> properties, IEnumerable<StageDescriptor>? stages = null, LogDescriptor? log = null)
            => new WorkItemDescriptor(name, properties, stages ?? Array.Empty<StageDescriptor>(), log ?? new LogDescriptor(Array.Empty<LogEntryTypeDescriptor>()));
    }

    public record LogDescriptor(IEnumerable<LogEntryTypeDescriptor> Types);
    public record StageDescriptor(string Name, ConditionDescriptor Condition, IEnumerable<StagePropertyDescriptor> Properties, IEnumerable<CommandDescriptor> Commands);
    public record StagePropertyDescriptor(string Name, bool? IsVisible, bool? IsEditable, IEnumerable<ValidatorDescriptor> Validators);

    public record PropertyDescriptor(string Name, string DataType, string Label, string Hint, string Description, PropertyType PropertyType, bool IsVisible, bool IsEditable, string InitialValue, IEnumerable<ValidatorDescriptor> Validators, ValueProviderDescriptor? ValueProvider)
    {
        public static PropertyDescriptor Create(string name, string dataType, string? label = null, string hint = "", string description = "", PropertyType propertyType = PropertyType.SingleRaw, bool isVisible = true, bool isEditable = true, string initialValue = "", IEnumerable<ValidatorDescriptor>? validators = null, ValueProviderDescriptor? valueProvider = null)
            => new PropertyDescriptor(name, dataType, label ?? name, hint, description, propertyType, isVisible, isEditable, initialValue, validators ?? Array.Empty<ValidatorDescriptor>(), valueProvider);
    }
    public enum PropertyType
    {
        SingleRaw,
        SingleValueFromProvider,
        MultipleValueFromProvider,
    }
}