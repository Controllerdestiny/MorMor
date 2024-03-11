using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomoAPI.Enumeration.EventParamType;

public enum MuteType
{
    [Description("ban")]
    Mute,

    [Description("lift_ban")]
    UnMute
}
