
namespace MomoAPI.Net.Config;

public class ClientConfig
{
    public string Host { get; init; } = "127.0.0.1";

    public int Port { get; init; }

    public string AccessToken { get; init; } = string.Empty;
}
