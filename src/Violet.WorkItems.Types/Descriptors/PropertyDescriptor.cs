using System;
using System.Collections.Generic;

namespace Violet.WorkItems.Types
{
    public class PropertyDescriptor
    {
        public PropertyDescriptor(string name, string dataType, PropertyType propertyType = PropertyType.SingleRaw, bool isVisible = true, bool isEditable = true, string initialValue = "", IEnumerable<ValidatorDescriptor>? validators = null, ValueProviderDescriptor? valueProvider = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("message", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(dataType))
            {
                throw new ArgumentException("message", nameof(dataType));
            }

            Name = name;
            DataType = dataType;
            PropertyType = propertyType;
            IsVisible = isVisible;
            IsEditable = isEditable;
            InitialValue = initialValue;
            Validators = validators ?? Array.Empty<ValidatorDescriptor>();
            ValueProvider = valueProvider;
        }

        public string Name { get; }
        public string DataType { get; }
        public PropertyType PropertyType { get; }
        public bool IsVisible { get; }
        public bool IsEditable { get; }
        public string InitialValue { get; }
        public ValueProviderDescriptor? ValueProvider { get; }
        public IEnumerable<ValidatorDescriptor> Validators { get; }
    }
}