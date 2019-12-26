using System.Collections.Generic;

namespace Violet.WorkItems.Types
{
    public class EnumValueProviderDescriptor : ValueProviderDescriptor
    {
        public EnumValueProviderDescriptor(params EnumValue[] values)
            : this((IEnumerable<EnumValue>)values)
        { }

        public EnumValueProviderDescriptor(IEnumerable<EnumValue> values)
            : base("Enum")
        {
            Values = values;
        }

        public IEnumerable<EnumValue> Values { get; }
    }

}