using System.Text.Json;
using System.Text.Json.Serialization;
using ClassLibrary.Entities;

namespace ClassLibrary.Converter;

public class TextComponentConverter : JsonConverter<TextComponent>
{
    public override TextComponent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = doc.RootElement;

            string typeName = root.GetProperty("Type").GetString();
            Type type = Type.GetType(typeName) ?? throw new JsonException($"Unknown type: {typeName}");

            return (TextComponent)JsonSerializer.Deserialize(root.GetRawText(), type, options);
        }
    }

    public override void Write(Utf8JsonWriter writer, TextComponent value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Type", value.GetType().AssemblyQualifiedName);
        foreach (var property in value.GetType().GetProperties())
        {
            object? propValue = property.GetValue(value);
            writer.WritePropertyName(property.Name);
            JsonSerializer.Serialize(writer, propValue, property.PropertyType, options);
        }
        writer.WriteEndObject();
    }
}