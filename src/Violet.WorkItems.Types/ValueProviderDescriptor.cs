using System;

namespace Violet.WorkItems.Types
{
    public class ValueProviderDescriptor
    {
        public ValueProviderDescriptor(string type, string configuration)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("message", nameof(type));
            }

            Type = type;
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string Type { get; }
        public string Configuration { get; }
    }
}