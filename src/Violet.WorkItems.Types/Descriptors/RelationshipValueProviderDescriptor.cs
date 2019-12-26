using System;

namespace Violet.WorkItems.Types
{
    public class RelationshipValueProviderDescriptor : ValueProviderDescriptor
    {
        public RelationshipValueProviderDescriptor(string relationshipType, string target)
            : base("Relationship")
        {
            RelationshipType = relationshipType ?? throw new ArgumentNullException(nameof(relationshipType));
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public string RelationshipType { get; }
        public string Target { get; }
    }

}