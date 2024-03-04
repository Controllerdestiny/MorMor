using MorMor.Enumeration;
using MorMor.Terraria.Server;
using MorMor.Terraria.Server.ApiRequestParam;
using MorMor.Terraria.Server.ApResultArgs;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace MorMor.Terraria;

public class TerrariaServer
{
    [JsonProperty("服务器名称")]
    public string Name { get; set; } = "玄荒";

    [JsonProperty("服务器IP")]
    public string IP { get; set; } = "1.15.157.111";

    [JsonProperty("服务器端口号")]
    public ushort Port { get; set; } = 7777;

    [JsonProperty("服务器转发端口号")]
    public ushort NatProt { get; set; } = 7777;

    [JsonProperty("服务器Rest端口号")]
    public ushort RestPort { get; set; } = 6767;

    [JsonProperty("服务器令牌")]
    public string Token { get; set; } = "Wzopaadeq1";

    [JsonProperty("注册默认组")]
    public string DefaultGroup { get; set; } = "default";

    [JsonProperty("货币兑换比例")]
    public int ExchangeRate { get; set; } = 40000;

    [JsonProperty("是否开启商店")]
    public bool EnabledShop { get; set; }

    [JsonProperty("是否开启抽奖")]
    public bool EnabledPrize { get; set; }

    [JsonProperty("Tshock路径")]
    public string TShockPath { get; set; } = "C:/Users/Administrator/Desktop/tshock/玄荒/";

    [JsonProperty("地图存放路径")]
    public string MapSavePath { get; set; } = "C:/Users/Administrator/Desktop/tshock/玄荒/world/玄荒.wld";

    [JsonProperty("服务器说明")]
    public string Describe { get; set; } = "正常玩法服务器";

    [JsonProperty("服务器版本")]
    public string Version { get; set; } = "1.4.4.9";

    [JsonProperty("数据库地址")]
    public string DBAddress { get; set; } = "1.15.157.111";

    [JsonProperty("数据库端口")]
    public ushort DBPort { get; set; } = 3306;

    [JsonProperty("数据库名称")]
    public string DBName { get; set; } = "Terraria";

    [JsonProperty("数据库用户名")]
    public string DBUserName { get; set; } = "Terraria";

    [JsonProperty("数据库密码")]
    public string DBPassword { get; set; } = "123456";

    [JsonProperty("重置禁止删除表")]
    public HashSet<string> NotRemoveTable { get; set; } = new HashSet<string>()
    {
        "grouplist",
        "userbans",
        "playerbans",
        "projectilebans",
        "itembans",
        "tilebans"
    };

    [JsonProperty("所属群")]
    public HashSet<long> Groups { get; set; } = new();

    [JsonProperty("消息转发群")]
    public HashSet<long> ForwardGroups { get; set; } = new();

    public async Task<OnlineRankArgs> QueryOnlines()
    {
        return await ApiRequest.Send<OnlineRankArgs>(this, TerrariaApiType.PlayerOnline);
    }

    public async Task<DeatRankArgs> DeatRank()
    {
        return await ApiRequest.Send<DeatRankArgs>(this, TerrariaApiType.DeathRank);
    }

    public async Task<PlayerInvseeArgs> QueryInventory(string name)
    {
        var param = new Dictionary<string, string>()
        {
            { "name", name }
        };
        return await ApiRequest.Send<PlayerInvseeArgs>(this, TerrariaApiType.BeanInvsee, param);
    }

    public async Task<ExecuteCommamdArgs> ExecCommamd(string cmd)
    {
        var param = new Dictionary<string, string>()
        {
            { "name", cmd }
        };
        return await ApiRequest.Send<ExecuteCommamdArgs>(this, TerrariaApiType.ExecCommand
            , param);
    }

    public async Task<PlayerListArgs> PlayerOnline()
    {
        return await ApiRequest.Send<PlayerListArgs>(this, TerrariaApiType.PlayerOnline);
    }

    public async Task<GenerateMapArgs> GeneareMap()
    {
        return await ApiRequest.Send<GenerateMapArgs>(this, TerrariaApiType.GenerateMap);
    }

    public async Task<ProgressQueryArgs> Progress()
    {
        return await ApiRequest.Send<ProgressQueryArgs>(this, TerrariaApiType.GameProgress);
    }

    public async Task<RegisterUserArgs> Register(string name, string password)
    {
        var param = new Dictionary<string, string>()
        {
            { "user", name },
            { "password", password },
            { "group", "default" }
        };
        return await ApiRequest.Send<RegisterUserArgs>(this, TerrariaApiType.Register, param);
    }



    public async Task<BaseResultArgs> SendPublicMsg(string name, string msg)
    {
        var param = new Dictionary<string, string>()
        {
            { "name", name },
            { "msg", msg }
        };
        return await ApiRequest.Send<BaseResultArgs>(this, TerrariaApiType.SendPublicMsg, param);
    }

    public async Task<BaseResultArgs> SendPrivateMsg(string name, string msg)
    {
        var param = new Dictionary<string, string>()
        {
            { "name", name },
            { "msg", msg }
        };
        return await ApiRequest.Send<BaseResultArgs>(this, TerrariaApiType.SendPrivateMsg, param);
    }

    public async Task<EconomicsBankArgs> QueryEconomicBank(string name)
    {
        var param = new Dictionary<string, string>()
        {
            { "name", name },
            { "cmd", "query" }
        };
        return await ApiRequest.Send<EconomicsBankArgs>(this, TerrariaApiType.EconomicsBank, param);
    }

    public async Task<EconomicsBankArgs> ClearEconomicBank(string name)
    {
        var param = new Dictionary<string, string>()
        {
            { "name", name },
            { "cmd", "query" }
        };
        return await ApiRequest.Send<EconomicsBankArgs>(this, TerrariaApiType.EconomicsBank, param);
    }

    public async Task<ServerStatusArgs> Status()
    {
        return await ApiRequest.Send<ServerStatusArgs>(this, TerrariaApiType.Status);
    }
}
