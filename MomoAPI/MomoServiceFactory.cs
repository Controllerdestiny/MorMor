using MomoAPI.Interface;
using MomoAPI.Log;
using MomoAPI.Net;
using MomoAPI.Net.Config;

namespace MomoAPI;

public class MomoServiceFactory
{
    public static LogWriter Log { get; set; }

    public static IMomoService CreateService(ClientConfig Config)
    {
        if (Config.Log == null)
            Log = new LogWriter(Path.Combine(Environment.CurrentDirectory, "log", $"{DateTime.Now:yyyy-MM-dddd HH-mm-ss}.log"));
        else
            Log = Config.Log;
        Log.ConsoleInfo("欢迎使用MorMor机器人....");
        Log.ConsoleInfo("正在连接WebSocket...");
        return new MomoReceive(Config);
    }
}
