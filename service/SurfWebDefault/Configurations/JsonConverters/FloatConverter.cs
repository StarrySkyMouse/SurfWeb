using System.Text.Json;
using System.Text.Json.Serialization;

namespace Configurations.JsonConverters;

public class FloatConverter : JsonConverter<float>
{
    public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return float.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
    {
        var ts = TimeSpan.FromSeconds(value);
        writer.WriteStringValue(ts.ToString(@"hh\:mm\:ss\.ffff"));
    }
}