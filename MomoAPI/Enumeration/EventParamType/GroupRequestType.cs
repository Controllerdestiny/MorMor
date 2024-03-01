using System.ComponentModel;

namespace MomoAPI.Enumeration.EventParamType;

public enum GroupRequestType
{
    [Description("add")]
    Add,

    /// <summary>
    /// 邀请
    /// </summary>
    [Description("invite")]
    Invite
}
