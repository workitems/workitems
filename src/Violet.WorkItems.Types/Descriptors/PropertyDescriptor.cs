using System;
using System.Collections.Generic;

namespace Violet.WorkItems.Types
{
    public class PropertyDescriptor
    {
        public PropertyDescriptor(string name, string dataType, PropertyType propertyType, bool isVisible, bool isEditable, IEnumerable<ValidatorDescriptor> validators, ValueProviderDescriptor valueProvider)
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
            ValueProvider = valueProvider;
            Validators = validators;
        }

        public string Name { get; }
        public string DataType { get; }
        public PropertyType PropertyType { get; }
        public bool IsVisible { get; }
        public bool IsEditable { get; }
        public ValueProviderDescriptor ValueProvider { get; }
        public IEnumerable<ValidatorDescriptor> Validators { get; }
    }
}