using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Enumeration;

public enum TerrariaApiType
{
    [Description("/v3/server/rawcmd")]
    ExecCommand,

    [Description("/v2/players/list")]
    PlayerOnline,

    [Description("/v2/users/create")]
    Register,

    [Description("/onlineDuration")]
    OnlineRank,

    [Description("/Progress")]
    GameProgress,

    [Description("/generatemap")]
    GenerateMap,

    [Description("/bank")]
    EconomicsBank,

    [Description("/msg/public")]
    SendPublicMsg,

    [Description("/msg/private")]
    SendPrivateMsg,

    [Description("/beanInvsee")]
    BeanInvsee,

    [Description("/deathrank")]
    DeathRank,

    [Description("/v2/server/status")]
    Status
}
