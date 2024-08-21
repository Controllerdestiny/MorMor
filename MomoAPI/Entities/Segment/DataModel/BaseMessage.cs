

using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Segment.DataModel;

[JsonDerivedType(typeof(Image))]
[JsonDerivedType(typeof(At))]
[JsonDerivedType(typeof(Face))]
[JsonDerivedType(typeof(File))]
[JsonDerivedType(typeof(Json))]
[JsonDerivedType(typeof(Music))]
[JsonDerivedType(typeof(Record))]
[JsonDerivedType(typeof(Reply))]
[JsonDerivedType(typeof(Text))]
[JsonDerivedType(typeof(Video))]
public record BaseMessage
{
}
