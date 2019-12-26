using System;

namespace Violet.WorkItems.Types
{
    public class ProjectCollectionValueProviderDescriptor : ValueProviderDescriptor
    {
        public ProjectCollectionValueProviderDescriptor(string collectionName)
            : base("ProjectCollection")
        {
            CollectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
        }

        public string CollectionName { get; }
    }

}