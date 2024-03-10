using MomoAPI;
using MomoAPI.Interface;
using MorMor.Commands;
using MorMor.Configuration;
using MorMor.DB.Manager;
using MorMor.Event;
using MorMor.Plugin;
using MorMor.Terraria.Server;
using MySql.Data.MySqlClient;
using System.Data;

namespace MorMor;

public class MorMorAPI
{
    public static IDbConnection DB { get; internal set; }

    public static SignManager SignManager { get; internal set; }

    public static GroupMananger GroupManager { get; internal set; }

    public static AccountManager AccountManager { get; internal set; }

    public static CurrencyManager CurrencyManager { get; internal set; }

    public static TerrariaUserManager TerrariaUserManager { get; internal set; }

    public static string PATH => Environment.CurrentDirectory;

    public static string SAVE_PATH => Path.Combine(PATH, "Config");

    internal static string ConfigPath => Path.Combine(SAVE_PATH, "MorMor.Json");

    internal static string UserLocationPath => Path.Combine(SAVE_PATH, "UserLocation.Json");

    public static MorMorSetting Setting { get; internal set; }

    public static UserLocation UserLocation { get; internal set; }

    public static IMomoService Service { get; internal set; }

    public static async Task Star()
    {
        if (!Directory.Exists(SAVE_PATH))
            Directory.CreateDirectory(SAVE_PATH);
        //读取Config
        LoadConfig();
        //初始化数据库
        InitDb();
        //扩展程序集
        AppDomain.CurrentDomain.AssemblyResolve += MappingPlugin.Resolve;
        //加载插件
        MappingPlugin.Initializer();
        //启动服务
        Service = await MomoServiceFactory.CreateService(new()
        {
            Host = Setting.Host,
            Port = Setting.Port,
        }).Start();
        //socket服务器启动
        SocketServer.Start();
        //Socket信息适配器
        SocketServer.SocketMessage += TerrariaMsgReceiveHandler.Adapter;
        //群消息转发适配器
        Service.Event.OnGroupMessage += TerrariaMsgReceiveHandler.GroupMessageForwardAdapter;
        //监听指令
        Service.Event.OnGroupMessage += e => CommandManager.Hook.CommandAdapter(e);
    }

    internal static void LoadConfig()
    {
        Setting = Config.LoadConfig(ConfigPath, Setting);
        UserLocation = Config.LoadConfig(UserLocationPath, UserLocation);
    }

    private static void InitDb()
    {
        DB = new MySqlConnection()
        {
            ConnectionString = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4}",
            Setting.DbHost, Setting.DbPort, Setting.DbName, Setting.DbUserName, Setting.DbPassword)
        };
        GroupManager = new();
        AccountManager = new();
        CurrencyManager = new();
        SignManager = new();
        TerrariaUserManager = new();
    }
}
