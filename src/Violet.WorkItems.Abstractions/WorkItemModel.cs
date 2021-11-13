using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Violet.WorkItems;

public record Property(string Name, string DataType, string Value)
{
    public static readonly string NullValue = string.Empty;
}
public record PropertyChange(string Name, string OldValue, string NewValue);
public record LogEntry(DateTimeOffset Date, string User, string Comment, ImmutableArray<PropertyChange> Changes);

public record WorkItem(string ProjectCode, string Id, string WorkItemType, ImmutableArray<Property> Properties, ImmutableArray<LogEntry> Log)
{
    public Property? this[string propertyName]
        => Properties.FirstOrDefault(property => property.Name == propertyName);
    public bool TryGetProperty(string propertyName, [NotNullWhen(returnValue: true)] out Property? property)
    {
        property = this[propertyName];

        return property is not null;
    }

    public WorkItem With(Property newProperty)
        => this with
        {
            Properties = TryGetProperty(newProperty.Name, out var oldProperty)
                ? this.Properties.Replace(oldProperty, newProperty)
                : this.Properties.Add(newProperty)
        };
}
