using System;
using System.Collections.Generic;
using System.Collections.Immutable;

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
            Validators = validators.ToImmutableArray();
        }

        public string Name { get; }
        public string DataType { get; }
        public PropertyType PropertyType { get; }
        public bool IsVisible { get; }
        public bool IsEditable { get; }
        public ValueProviderDescriptor ValueProvider { get; }
        public ImmutableArray<ValidatorDescriptor> Validators { get; }
    }
}