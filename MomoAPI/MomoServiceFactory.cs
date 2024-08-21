using MomoAPI.Interface;
using MomoAPI.Net;
using MomoAPI.Net.Config;

namespace MomoAPI;

public class MomoServiceFactory
{
    public static IMomoService CreateService(ClientConfig Config)
    {
        return new MomoReceive(Config);
    }
}
