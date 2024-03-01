using System.ComponentModel;

namespace MomoAPI.Enumeration.EventParamType;

public enum MemberChangeType
{
    [Description("leave")]
    Leave,

    [Description("kick")]
    Kick,

    [Description("kick_me")]
    KickMe,

    [Description("approve")]
    Approve,

    [Description("invite")]
    Invite
}
