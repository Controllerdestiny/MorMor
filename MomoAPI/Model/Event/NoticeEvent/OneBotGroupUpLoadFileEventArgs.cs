using MomoAPI.Entities.Info;
using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event.NoticeEvent;

public class OneBotGroupUpLoadFileEventArgs : BaseObNoticeEventArgs
{
    [JsonPropertyName("group_id")]
    public long GroupId { get; init; }

    [JsonPropertyName("file")]
    public GroupUpLoadInfo UpLoad { get; init; } = new();
}
