using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml;

namespace Violet.WorkItems.ValueTypes
{
    public class DateTimeOffsetConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            => (destinationType == typeof(string)) || base.CanConvertTo(context, destinationType);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            => (value is DateTimeOffset dateTimeOffset) ? XmlConvert.ToString(dateTimeOffset) : base.ConvertTo(context, culture, value, destinationType);


        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => (sourceType == typeof(string) || base.CanConvertFrom(context, sourceType));

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            => (value is string input && DateTimeOffset.TryParse(input, out var result)) ? result : base.ConvertFrom(context, culture, value);
    }
}