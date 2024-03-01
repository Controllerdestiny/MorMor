using System.Reflection;

namespace MorMor.Commands;

internal static class CommandUtils
{
    public static bool ClassConstructParamIsZerp(this ConstructorInfo[] constructors)
    {
        return constructors.Any(ctor => ctor.GetParameters().Length == 0);
    }

    public static bool CommandParamPares(this MethodInfo method)
    {
        if (method != null)
        {
            var param = method.GetParameters();
            if (param.Length == 1)
            {
                return param[0].ParameterType == typeof(CommandArgs);
            }
        }
        return false;
    }
}
