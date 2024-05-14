
using System.ComponentModel;

namespace MomoAPI.Enumeration;

public enum SegmentType
{
    [Description("")]
    Unknown,
    /// <summary>
    /// 文本消息
    /// </summary>
    [Description("text")]
    Text,

    /// <summary>
    /// 图片
    /// </summary>
    [Description("image")]
    Image,

    /// <summary>
    /// 表情
    /// </summary>
    [Description("face")]
    Face,

    /// <summary>
    /// 语音
    /// </summary>
    [Description("record")]
    Record,

    /// <summary>
    /// 引用
    /// </summary>
    [Description("reply")]
    Reply,

    /// <summary>
    /// Json消息
    /// </summary>
    [Description("json")]
    Json,

    /// <summary>
    /// AT
    /// </summary>
    [Description("at")]
    At,

    /// <summary>
    /// 文件
    /// </summary>
    [Description("file")]
    File,

    /// <summary>
    /// 视频
    /// </summary>
    [Description("video")]
    Video,

    [Description("music")]
    Music,

    Ignore
}
