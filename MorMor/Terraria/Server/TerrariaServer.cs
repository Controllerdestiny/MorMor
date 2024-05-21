using MorMor.Enumeration;
using MorMor.Event;
using MorMor.Model.Socket.Action;
using MorMor.Model.Socket.Action.Receive;
using MorMor.Model.Socket.Action.Response;
using MorMor.Net;
using Newtonsoft.Json;
using ProtoBuf;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MorMor.Terraria;

public class TerrariaServer
{
    [JsonProperty("服务器名称")]
    public string Name { get; set; } = "服务器1";

    [JsonProperty("服务器IP")]
    public string IP { get; set; } = "";

    [JsonProperty("实际端口")]
    public ushort Port { get; set; } = 7777;

    [JsonProperty("显示端口")]
    public ushort NatProt { get; set; } = 7777;

    [JsonProperty("通信令牌")]
    public string Token { get; set; } = "";

    [JsonProperty("注册默认组")]
    public string DefaultGroup { get; set; } = "default";

    [JsonProperty("最大注册数量")]
    public int RegisterMaxCount { get; set; } = 1;

    [JsonProperty("注册名字大长度")]
    public int RegisterNameMax { get; set; } = 10;

    [JsonProperty("转发消息最大长度")]
    public int MsgMaxLength { get; set; } = 50;

    [JsonProperty("注册名称仅中文")]
    public bool RegisterNameLimit { get; set; } = true;

    [JsonProperty("是否开启商店")]
    public bool EnabledShop { get; set; }

    [JsonProperty("是否开启抽奖")]
    public bool EnabledPrize { get; set; }

    [JsonProperty("Tshock路径")]
    public string TShockPath { get; set; } = "C:/Users/Administrator/Desktop/tshock/";

    [JsonProperty("地图名称")]
    public string MapName { get; set; } = "玄荒.wld";

    [JsonProperty("服务器说明")]
    public string Describe { get; set; } = "正常玩法服务器";

    [JsonProperty("服务器版本")]
    public string Version { get; set; } = "1.4.4.9";



    [JsonProperty("所属群")]
    public HashSet<long> Groups { get; set; } = new();

    [JsonProperty("消息转发群")]
    public HashSet<long> ForwardGroups { get; set; } = new();

    [JsonIgnore]
    public TaskCompletionSource<byte[]>? WaitFile { get; set; }

    public async Task<ServerCommand> Command(string cmd)
    {
        var args = new ServerCommandArgs()
        {
            Text = cmd,
            ActionType = ActionType.Command,
        };
        return await RequestApi<ServerCommandArgs, ServerCommand>(args);
    }

    public async Task<ServerOnline> ServerOnline()
    {
        var args = new BaseAction()
        {
            ActionType = ActionType.ServerOnline,
        };
        return await RequestApi<BaseAction, ServerOnline>(args);
    }

    public async Task<BaseActionResponse> Register(string Name, string Password)
    {
        var args = new RegisterAccountArgs()
        {
            ActionType = ActionType.RegisterAccount,
            Name = Name,
            Group = DefaultGroup,
            Password = Password
        };
        return await RequestApi<RegisterAccountArgs, BaseActionResponse>(args);
    }

    public async Task<GameProgress> QueryServerProgress()
    {
        var args = new BaseAction()
        {
            ActionType = ActionType.GameProgress,
        };
        return await RequestApi<BaseAction, GameProgress>(args);
    }

    public async Task<PlayerInventory> PlayerInventory(string name)
    {
        var args = new QueryPlayerInventoryArgs()
        {
            ActionType = ActionType.Inventory,
            Name = name
        };
        return await RequestApi<QueryPlayerInventoryArgs, PlayerInventory>(args);
    }

    public async Task<MapImage> MapImage(ImageType type)
    {
        var args = new MapImageArgs()
        {
            ActionType = ActionType.WorldMap,
            ImageType = type
        };
        return await RequestApi<MapImageArgs, MapImage>(args);
    }

    public async Task<BaseActionResponse> Broadcast(string text, byte R, byte G, byte B)
    {
        var args = new BroadcastArgs()
        {
            ActionType = ActionType.PluginMsg,
            Text = text,
            Color = [R, G, B]
        };
        return await RequestApi<BroadcastArgs, BaseActionResponse>(args);
    }

    public async Task<BaseActionResponse> Broadcast(string text, Color color)
    {
        return await Broadcast(text, color.R, color.G, color.B);
    }

    public async Task<BaseActionResponse> PrivateMsg(string name, string text, byte R, byte G, byte B)
    {
        var args = new PrivatMsgArgs()
        {
            ActionType = ActionType.PrivateMsg,
            Text = text,
            Name = name,
            Color = [R, G, B]
        };
        return await RequestApi<PrivatMsgArgs, BaseActionResponse>(args);
    }

    public async Task<BaseActionResponse> PrivateMsg(string name, string text, Color color)
    {
        return await PrivateMsg(name, text, color.R, color.G, color.B);
    }

    public async Task<PlayerOnlineRank> OnlineRank()
    {
        var args = new BaseAction()
        {
            ActionType = ActionType.OnlineRank
        };
        return await RequestApi<BaseAction, PlayerOnlineRank>(args);
    }

    public async Task<DeadRank> DeadRank()
    {
        var args = new BaseAction()
        {
            ActionType = ActionType.DeadRank
        };
        return await RequestApi<BaseAction, DeadRank>(args);
    }

    public async Task<UpLoadWorldFile> GetWorldFile()
    {
        var args = new BaseAction()
        {
            ActionType = ActionType.UpLoadWorld
        };
        return await RequestApi<BaseAction, UpLoadWorldFile>(args);
    }

    public async Task<ServerStatus> ServerStatus()
    {
        var args = new BaseAction()
        {
            ActionType = ActionType.ServerStatus
        };
        return await RequestApi<BaseAction, ServerStatus>(args);
    }

    public async Task<BaseActionResponse> ResetPlayerPwd(string name, string pwd)
    {
        var args = new PlayerPasswordResetArgs()
        {
            ActionType = ActionType.ResetPassword,
            Name = name,
            Password = pwd
        };
        return await RequestApi<PlayerPasswordResetArgs, BaseActionResponse>(args);
    }

    public async Task<BaseActionResponse> ReStartServer(Dictionary<string, string> startArgs)
    {
        var args = new ReStartServerArgs()
        {
            ActionType = ActionType.ReStartServer,
            StartArgs = SpawnStartArgs(startArgs)
        };
        return await RequestApi<ReStartServerArgs, BaseActionResponse>(args);
    }

    public string SpawnStartArgs(Dictionary<string, string> startArgs)
    {
        var status = ServerStatus().Result;
        var world = status.Status ? Path.Combine(status.TShockPath, "world", MapName) : Path.Combine(TShockPath, "world", MapName);
        var param = new Dictionary<string, string>
        {
            { "-autocreate", "3" },
            { "-world",  world },
            { "-port", Port.ToString() },
            { "-lang", "7" },
            { "-mode", "2" },
            { "-players", "50" }
        };
        return string.Join(" ", param
            .Concat(startArgs)
            .GroupBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Last().Value)
            .Select(x => $"{x.Key} {x.Value}"));
    }

    public static bool IsReWorld(byte[] buffer)
    {
        try
        {
            using MemoryStream stream = new MemoryStream(buffer);
            using BinaryReader reader = new BinaryReader(stream);
            if (reader.ReadInt32() >= 135)
            {
                ulong num = reader.ReadUInt64();
                if ((num & 0xFFFFFFFFFFFFFFL) == 27981915666277746L)
                    return true;
            }
        }
        catch
        {
            return false;
        }
        return false;
    }

    public async Task<BaseActionResponse> Reset(Dictionary<string, string> startArgs, Action<RestServerType> OnWait)
    {
        var args = new ResetServerArgs()
        {
            ActionType = ActionType.ResetServer,
            StartArgs = SpawnStartArgs(startArgs),
        };
        if (startArgs.TryGetValue("-upload", out var _))
        {
            var now = DateTime.Now;
            WaitFile = new();
            OnWait(RestServerType.WaitFile);
            try
            {
                var buffer = await WaitFile.Task.WaitAsync(TimeSpan.FromSeconds(60));
                while (!IsReWorld(buffer) && (DateTime.Now - now).TotalSeconds < 60)
                {
                    OnWait(RestServerType.UnLoadFile);
                    WaitFile = new();
                    buffer = await WaitFile.Task.WaitAsync(TimeSpan.FromSeconds((DateTime.Now - now).TotalSeconds));
                }
                args.UseFile = true;
                args.FileName = Name + ".wld";
                args.FileBuffer = buffer;
                OnWait(RestServerType.LoadFile);
            }
            catch
            {
                WaitFile = null;
                OnWait(RestServerType.TimeOut);
            }
            WaitFile = null;
        }
        OnWait(RestServerType.Success);
        return await RequestApi<ResetServerArgs, BaseActionResponse>(args);
    }

    public async Task<TResult> RequestApi<In, TResult>(In ApiParam, TimeSpan? timeout = null) where In : BaseAction where TResult : BaseActionResponse, new()
    {
        var Client = WebSocketConnectManager.GetConnent(Name);
        if (Client != null && Client.IsAvailable)
        {
            ApiParam.Echo = Guid.NewGuid().ToString();
            ApiParam.ServerName = Name;
            ApiParam.MessageType = PostMessageType.Action;
            ApiParam.Token = Token;
            using MemoryStream stream = new();
            Serializer.Serialize(stream, ApiParam);
            await Client.Send(stream.ToArray());
            return await TerrariaMsgReceiveHandler.GetResponse<TResult>(ApiParam.Echo, timeout) ?? new()
            {
                Status = false,
                ServerName = Name,
                Message = "服务器未连接!"
            };
        }
        return new TResult()
        {
            Status = false,
            ServerName = Name,
            Message = "服务器未连接!"
        };
    }

    public bool Start(Dictionary<string, string> startArgs)
    {
        Process process = new();
        process.StartInfo.WorkingDirectory = TShockPath;
        process.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "TShock.Server.exe" : "TShock.Server";
        process.StartInfo.Arguments = SpawnStartArgs(startArgs);
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
