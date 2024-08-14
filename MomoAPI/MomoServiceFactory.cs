using MomoAPI.Interface;
using MomoAPI.Net;
using MomoAPI.Net.Config;

namespace MomoAPI;

public class MomoServiceFactory
{
    public static IMomoService CreateService(ClientConfig Config)
    {
        Log.ConsoleInfo("欢迎使用MorMor机器人....");
        Log.ConsoleInfo("正在连接WebSocket...");
        return new MomoReceive(Config);
    }
}
