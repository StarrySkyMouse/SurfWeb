using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.JsonConverters;

/// <summary>
///     long id传入前端会丢失精度
/// </summary>
public class Int64ToStringConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return long.Parse(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}