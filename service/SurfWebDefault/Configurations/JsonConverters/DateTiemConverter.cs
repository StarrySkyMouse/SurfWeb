﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Configurations.JsonConverters;

/// <summary>
///     日期时间格式化转换器
/// </summary>
public class DateTiemConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
}