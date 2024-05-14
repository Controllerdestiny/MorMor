using System.ComponentModel;

namespace MomoAPI.Enumeration.EventParamType;

public enum MuteType
{
    [Description("ban")]
    Mute,

    [Description("lift_ban")]
    UnMute
}
