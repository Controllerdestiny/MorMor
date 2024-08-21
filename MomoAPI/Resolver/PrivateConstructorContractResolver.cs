using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace MomoAPI.Resolver;

public class PrivateConstructorContractResolver : DefaultJsonTypeInfoResolver
{
   public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
   {
       JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

       if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object && jsonTypeInfo.CreateObject is null)
        {
            // CreateObject can be null either because we have no public constructor or because a parameterized constructor is being used internally
            if (jsonTypeInfo.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length == 0)
            {
                // The type doesn't have public constructors
                jsonTypeInfo.CreateObject = () => Activator.CreateInstance(jsonTypeInfo.Type, true)!;
            }
        }
       return jsonTypeInfo;
   }
}