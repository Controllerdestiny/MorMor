using MorMor.Enumeration;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;

namespace MorMor.Utils;

internal class TerrariaApiStatusConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(TerrariaApiStatus);
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
        return TerrariaApiStatus.Error;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }


}
