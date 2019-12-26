using System;

namespace Violet.WorkItems.Types
{
    public class ProjectUserValueProviderDescriptor : ValueProviderDescriptor
    {
        public ProjectUserValueProviderDescriptor(string configuration)
            : base("ProjectUser")
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string Configuration { get; }
    }

}