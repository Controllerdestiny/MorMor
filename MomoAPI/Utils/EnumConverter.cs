using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;

namespace MomoAPI.Utils;

public class EnumConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Enum);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var fields = objectType.GetFields();
        string readValue = reader.Value?.ToString() ?? string.Empty;
        foreach (var field in fields)
        {
            var obj = field.GetCustomAttributes<DescriptionAttribute>();
            if (obj.Any(item => item?.Description == readValue))
                return Convert.ChangeType(field.GetValue(-1), objectType);
        }
        return objectType.IsEnum ? Activator.CreateInstance(objectType) : null;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (string.IsNullOrEmpty(value.ToString()))
        {
            writer.WriteValue("");
            return;
        }
        writer.WriteValue(GetFieldDesc(value));
    }

    public static string GetFieldDesc<T>(T value)
    {
        FieldInfo fieldInfo = value.GetType().GetField(value.ToString()!);
        if (fieldInfo == null)
            return string.Empty;
        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }

    public static T GetFieldByDesc<T>(string value) where T : Enum
    {
        var fields = typeof(T).GetFields();
        foreach (var field in fields)
        {
            var obj = field.GetCustomAttribute<DescriptionAttribute>();
            if (obj?.Description == value)
                return (T)field.GetValue(null);
        }
        return default(T);
    }
}
