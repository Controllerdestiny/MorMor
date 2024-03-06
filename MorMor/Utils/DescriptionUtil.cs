using System.ComponentModel;
using System.Reflection;

namespace MorMor.Utils;

internal class DescriptionUtil
{
    internal static string GetFieldDesc<T>(T value)
    {
        FieldInfo fieldInfo = value.GetType().GetField(value.ToString()!);
        if (fieldInfo == null)
            return string.Empty;
        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
}
