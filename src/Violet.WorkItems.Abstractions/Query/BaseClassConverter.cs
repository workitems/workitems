using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Violet.WorkItems.Query;

public class BaseClassConverter<TBaseType> : JsonConverter<TBaseType> where TBaseType : class
{
    private readonly Type[] _types;
    private const string TypeProperty = "$type";

    public BaseClassConverter(params Type[] types)
    {
        _types = types;
    }

    public override bool CanConvert(Type type)
        => typeof(TBaseType) == type;

    public override TBaseType Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        TBaseType result;

        if (JsonDocument.TryParseValue(ref reader, out var doc))
        {
            if (doc.RootElement.TryGetProperty(TypeProperty, out var typeProperty))
            {
                var typeName = typeProperty.GetString();
                var type = _types.FirstOrDefault(t => t.Name == typeName) ?? throw new JsonException($"{TypeProperty} specifies an invalid type");

                var rootElement = doc.RootElement.GetRawText();

                result = JsonSerializer.Deserialize(rootElement, type, options) as TBaseType ?? throw new JsonException("target type could not be serialized");
            }
            else
            {
                throw new JsonException($"{TypeProperty} missing");
            }
        }
        else
        {
            throw new JsonException("Failed to parse JsonDocument");
        }

        return result;
    }

    public override void Write(
        Utf8JsonWriter writer,
        TBaseType value,
        JsonSerializerOptions options)
    {
        var type = value.GetType();

        if (_types.Any(t => type.Name == t.Name))
        {
            var jsonElement = JsonSerializer.SerializeToElement(value, type, options);

            var jsonObject = JsonObject.Create(jsonElement) ?? throw new JsonException();
            jsonObject[TypeProperty] = type.Name;

            jsonObject.WriteTo(writer, options);
        }
        else
        {
            throw new JsonException($"{type.Name} with matching base type {typeof(TBaseType).Name} is not registered.");
        }
    }
}