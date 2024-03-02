using Newtonsoft.Json;

namespace MorMor.Configuration;

public class MorMorSetting
{
    [JsonProperty("指令前缀")]
    public List<string> CommamdPrefix { get; init; } = new();

    [JsonProperty("监听地址")]
    public string Host { get; init; } = "127.0.0.1";

    [JsonProperty("监听端口")]
    public int Port { get; init; } = 5000;

    [JsonProperty("数据库地址")]
    public string DbHost { get; init; } = "127.0.0.1";

    [JsonProperty("数据库端口")]
    public int DbPort { get; init; } = 3306;

    [JsonProperty("数据库名称")]
    public string DbName { get; init; } = "Mirai";

    [JsonProperty("数据库用户名")]
    public string DbUserName { get; init; } = "Mirai";

    [JsonProperty("数据库密码")]
    public string DbPassword { get; init; } = "";

    [JsonProperty("用户默认权限组")]
    public string DefaultPermGroup { get; init; } = "default";
}
