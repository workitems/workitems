using System;
using System.ComponentModel;
using System.Globalization;

namespace Violet.WorkItems
{
    public static class CommonDataTypesExtensions
    {
        private static bool IsTypeMatch(string propertyType, Type type)
            => type switch
            {
                { Name: "Nullable" } => IsTypeMatch(propertyType, type.GenericTypeArguments[0]),
                _ => propertyType == type.Name,
            };

        public static void Value<T>(this Property property, T value)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var type = typeof(T);
            if (!IsTypeMatch(property.DataType, type))
            {
                throw new InvalidOperationException($"Cannot serialize {type.Name} to property {property.Name} of data type {property.DataType}");
            }

            var tc = TypeDescriptor.GetConverter(type);
            var result = tc.ConvertToInvariantString(value);

            property.Value = result;
        }
        public static void Value<T>(this Property property, out T value)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var type = typeof(T);
            if (!IsTypeMatch(property.DataType, type))
            {
                throw new InvalidOperationException($"Cannot deserialize {type.Name} from property {property.Name} of data type {property.DataType}");
            }

            var tc = TypeDescriptor.GetConverter(type);
            if (tc.IsValid(property.Value))
            {
                var result = tc.ConvertFromInvariantString(property.Value);

                value = (T)result;
            }
            else
            {
                throw new ArgumentException($"Property value '{property.Value}' cannot be converted to {type.Name}");
            }
        }

        public static void NullableValue<T>(this Property property, T? value) where T : struct
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (!value.HasValue)
            {
                property.Value = Property.NullValue;
            }
            else
            {
                Value<T>(property, value.Value);
            }
        }

        public static void NullableValue<T>(this Property property, T? value) where T : class
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (value == null)
            {
                property.Value = Property.NullValue;
            }
            else
            {
                Value<T>(property, value);
            }
        }

        public static void NullableValue<T>(this Property property, out T? value) where T : struct
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (property.Value == null || property.Value == string.Empty)
            {
                value = default;
            }
            else
            {
                Value<T>(property, out var notNullableValue2);
                value = notNullableValue2;
            }
        }


        public static void NullableValue<T>(this Property property, out T? value) where T : class
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (property.Value == null || property.Value == string.Empty)
            {
                value = default;
            }
            else
            {
                Value<T>(property, out var notNullableValue2);
                value = notNullableValue2;
            }
        }
    }
}