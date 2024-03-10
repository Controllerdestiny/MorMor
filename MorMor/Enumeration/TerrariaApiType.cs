using System.ComponentModel;

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

    [Description("/reset")]
    Reset,

    [Description("/beanInvsee")]
    BeanInvsee,

    [Description("/deathrank")]
    DeathRank,

    [Description("/v2/server/status")]
    Status
}
