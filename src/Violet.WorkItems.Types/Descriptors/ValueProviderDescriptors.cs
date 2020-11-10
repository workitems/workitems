using System;
using System.Collections.Generic;

namespace Violet.WorkItems.Types
{
    public record ValueProviderDescriptor(string Type);

    public record RelationshipValueProviderDescriptor(string RelationshipType, string Target) : ValueProviderDescriptor("Relationship");
    public record ProjectUserValueProviderDescriptor(string Configuration) : ValueProviderDescriptor("ProjectUser");
    public record ProjectCollectionValueProviderDescriptor(string CollectionName) : ValueProviderDescriptor("ProjectCollection");


    public record EnumValue(string Value, string DisplayText);
    public record EnumValueProviderDescriptor(IEnumerable<EnumValue> Values) : ValueProviderDescriptor("Enum")
    {
        public EnumValueProviderDescriptor(params EnumValue[] values)
            : this((IEnumerable<EnumValue>)values)
        { }
    }
}