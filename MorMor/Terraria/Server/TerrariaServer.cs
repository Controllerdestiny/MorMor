using MomoAPI.Entities;
using MomoAPI.Net;
using MorMor.Enumeration;
using MorMor.Model.Terraria.SocketMessageModel;
using MorMor.Terraria.Server;
using MorMor.Terraria.Server.ApiRequestParam;
using MorMor.Terraria.Server.ApResultArgs;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace MorMor.Terraria;

public class TerrariaServer
{
    [JsonProperty("服务器名称")]
    public string Name { get; set; } = "服务器1";

    [JsonProperty("服务器IP")]
    public string IP { get; set; } = "";

    [JsonProperty("服务器端口号")]
    public ushort Port { get; set; } = 7777;

    [JsonProperty("服务器转发端口号")]
    public ushort NatProt { get; set; } = 7777;

    [JsonProperty("服务器Rest端口号")]
    public ushort RestPort { get; set; } = 8888;

    [JsonProperty("服务器令牌")]
    public string Token { get; set; } = "";

    [JsonProperty("注册默认组")]
    public string DefaultGroup { get; set; } = "default";

    [JsonProperty("货币兑换比例")]
    public int ExchangeRate { get; set; } = 40000;

    [JsonProperty("是否开启商店")]
    public bool EnabledShop { get; set; }

    [JsonProperty("是否开启抽奖")]
    public bool EnabledPrize { get; set; }

    [JsonProperty("Tshock路径")]
    public string TShockPath { get; set; } = "C:/Users/Administrator/Desktop/tshock/";

    [JsonProperty("地图存放路径")]
    public string MapSavePath { get; set; } = "C:/Users/Administrator/Desktop/tshock/world/地图.wld";

    [JsonProperty("服务器说明")]
    public string Describe { get; set; } = "正常玩法服务器";

    [JsonProperty("服务器版本")]
    public string Version { get; set; } = "1.4.4.9";


    [JsonProperty("所属群")]
    public HashSet<long> Groups { get; set; } = new();

    [JsonProperty("消息转发群")]
    public HashSet<long> ForwardGroups { get; set; } = new();

    [JsonIgnore]
    public Socket Client { get; internal set; }

    public async Task<OnlineRankArgs> QueryOnlines()
    {
        return await ApiRequest.Send<OnlineRankArgs>(this, TerrariaApiType.OnlineRank);
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

    public async Task<ExecuteCommamdArgs> ExecCommand(string cmd)
    {
        var param = new Dictionary<string, string>()
        {
            { "cmd", HttpUtility.UrlEncode(cmd) }
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


    public async Task<BaseResultArgs> SendMessage(TerrariaMessageContext context)
    {
        if (Client != null)
        {
            if (!Client.Poll(100, SelectMode.SelectRead) || Client.Available > 0)
            {
                await Client.SendAsync(Encoding.UTF8.GetBytes(context.ToJson()), SocketFlags.None);
                return new BaseResultArgs()
                {
                    Status = TerrariaApiStatus.Success
                };
            }
            return new BaseResultArgs()
            {
                Status = TerrariaApiStatus.Error,
                ErrorMessage = "无法连接到服务器;"
            };
        }
        return new BaseResultArgs()
        {
            Status = TerrariaApiStatus.Error,
            ErrorMessage = "Socket对象为空无法发送!;"
        };
    }

    public async Task<BaseResultArgs> SendPrivateMsg(string name, string msg, byte R, byte G, byte B)
    {
        var context = new TerrariaMessageContext()
        {
            Type = SocketMessageType.PrivateMsg,
            Color = new byte[] { R, G, B },
            Message = msg,
            Name = name,
        };
        return await SendMessage(context);
    }

    public async Task<BaseResultArgs> SendPrivateMsg(string name, string msg, Color color)
    {
        return await SendPrivateMsg(name, msg, color.R, color.G, color.B);
    }

    public async Task<BaseResultArgs> SendPublicMsg(string msg, byte R, byte G, byte B)
    {
        var context = new TerrariaMessageContext()
        {
            Type = SocketMessageType.PublicMsg,
            Color = new byte[] { R, G, B },
            Message = msg
        };
        return await SendMessage(context);
    }

    public async Task<BaseResultArgs> SendPublicMsg(string msg, Color color)
    {
        return await SendPublicMsg(msg, color.R, color.G, color.B);
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

    public async Task SendGroupMsg(MessageBody body)
    {
        foreach (var id in Groups)
        {
            await OneBotAPI.Instance.SendGroupMessage(id, body);
        }
    }

    public async Task Reset(Dictionary<string, string> args)
    {
        var result = await ApiRequest.Send<BaseResultArgs>(this, TerrariaApiType.Reset);
        if (result.IsSuccess)
        {
            await Task.Delay(5000);
            MorMorAPI.TerrariaUserManager.RemoveByServer(Name);
            if (!Start(args))
            {
                await SendGroupMsg("[重置] 服务器启动失败，请检查后台!");
                return;
            }
            await SendGroupMsg($"[重置] {Name}重置成功，正在启动并创建地图!");
        }
        else
        {
            await SendGroupMsg($"[重置]服务器重置出错:{result.ErrorMessage}!");
        }
    }

    public bool Start(Dictionary<string, string> startArgs)
    {
        if (!startArgs.ContainsKey("-autocreate"))
            startArgs["-autocreate"] = "3";
        if (!startArgs.ContainsKey("-world"))
            startArgs["-world"] = MapSavePath;
        if (!startArgs.ContainsKey("-port"))
            startArgs["-port"] = Port.ToString();
        if (!startArgs.ContainsKey("-lang"))
            startArgs["-lang"] = "7";
        if (!startArgs.ContainsKey("-difficulty"))
            startArgs["-mode"] = "2";
        if (!startArgs.ContainsKey("-players"))
            startArgs["-players"] = "50";
        var startArgsLine = "";
        foreach (var (key, value) in startArgs)
        {
            startArgsLine += $" {key} {value}";
        }
        Process process = new();
        process.StartInfo.WorkingDirectory = TShockPath;
        process.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "TShock.Server.exe" : "TShock.Server";
        process.StartInfo.Arguments = startArgsLine;
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.RedirectStandardInput = false;
        process.StartInfo.RedirectStandardOutput = false;
        process.StartInfo.RedirectStandardError = false;
        process.StartInfo.CreateNoWindow = true;
        if (process.Start())
        {
            process.Close();
            return true;
        }
        return false;
    }
}
