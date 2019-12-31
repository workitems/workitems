using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Violet.WorkItems
{
    public static class ValueTypesManager
    {
        private static Lazy<Dictionary<Type, Type>> s_lazyConverters = new Lazy<Dictionary<Type, Type>>(() => new Dictionary<Type, Type>
        {
            // Add the intrinsics
            //
            [typeof(bool)] = typeof(BooleanConverter),
            [typeof(byte)] = typeof(ByteConverter),
            [typeof(sbyte)] = typeof(SByteConverter),
            [typeof(char)] = typeof(CharConverter),
            [typeof(double)] = typeof(DoubleConverter),
            [typeof(string)] = typeof(StringConverter),
            [typeof(int)] = typeof(Int32Converter),
            [typeof(short)] = typeof(Int16Converter),
            [typeof(long)] = typeof(Int64Converter),
            [typeof(float)] = typeof(SingleConverter),
            [typeof(ushort)] = typeof(UInt16Converter),
            [typeof(uint)] = typeof(UInt32Converter),
            [typeof(ulong)] = typeof(UInt64Converter),
            //[typeof(DateTime)] = typeof(DateTimeConverter),
            [typeof(DateTimeOffset)] = typeof(Violet.WorkItems.ValueTypes.DateTimeOffsetConverter),
            [typeof(decimal)] = typeof(DecimalConverter),
            //[typeof(TimeSpan)] = typeof(TimeSpanConverter),
            [typeof(Guid)] = typeof(GuidConverter),
            //[typeof(Uri)] = typeof(UriTypeConverter),
            //[typeof(Version)] = typeof(VersionConverter),
            //[typeof(Enum)] = typeof(EnumConverter),
        });

        public static bool IsTypeMatch(string dataTypeName, Type type)
            => type switch
            {
                { Name: "Nullable" } => IsTypeMatch(dataTypeName, type.GenericTypeArguments[0]),
                _ => dataTypeName == type.Name,
            };

        private static bool TryGetConverterType(Func<KeyValuePair<Type, Type>, bool> predicate, out Type converterType)
        {
            converterType = s_lazyConverters.Value.Where(predicate).Select(kv => kv.Value).FirstOrDefault();

            return !(converterType is null);
        }

        private static TypeConverter? CreateConverter(Type converterType)
            => Activator.CreateInstance(converterType) as TypeConverter;

        public static TypeConverter? GetConverter(Type type)
            => TryGetConverterType(kv => kv.Key == type, out var converterType) ? CreateConverter(converterType) : null;


        public static TypeConverter? GetConverter(string dataTypeName)
            => TryGetConverterType(kv => kv.Key.Name == dataTypeName, out var converterType) ? CreateConverter(converterType) : null;

        public static bool IsValidDataType(string dataTypeName)
            => TryGetConverterType(kv => kv.Key.Name == dataTypeName, out _);


        public static bool IsValidateData(string dataTypeName, string value)
            => IsDefaultData(dataTypeName, value) || (GetConverter(dataTypeName)?.IsValid(value) ?? false);

        public static bool IsDefaultData(string dataTypeName, string value)
            => string.IsNullOrEmpty(value);
    }
}