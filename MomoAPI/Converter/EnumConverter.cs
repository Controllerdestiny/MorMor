using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MomoAPI.Converter;

public class EnumConverter<T> : JsonConverter<T> where T : Enum
{

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var fields = typeToConvert.GetFields();
        string readValue = reader.GetString() ?? string.Empty;
        foreach (var field in fields)
        {
            var obj = field.GetCustomAttributes<DescriptionAttribute>();
            if (obj.Any(item => item?.Description == readValue))
                return (T)Convert.ChangeType(field.GetValue(-1), typeToConvert)!;
        }
        return typeToConvert.IsEnum ? (T)Activator.CreateInstance(typeToConvert)! : default;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (string.IsNullOrEmpty(value?.ToString()))
        {
            writer.WriteStringValue("");
            return;
        }
        writer.WriteStringValue(GetFieldDesc(value));
    }


    public static string GetFieldDesc(T value)
    {
        if (value == null)
            return string.Empty;
        FieldInfo? fieldInfo = value?.GetType().GetField(value.ToString()!);
        if (fieldInfo == null)
            return string.Empty;
        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }

    public static T GetFieldByDesc(string value)
    {
        var fields = typeof(T).GetFields();
        foreach (var field in fields)
        {
            var obj = field.GetCustomAttribute<DescriptionAttribute>();
            if (obj?.Description == value)
                return (T)field.GetValue(null)!;
        }
        return default!;
    }
}
