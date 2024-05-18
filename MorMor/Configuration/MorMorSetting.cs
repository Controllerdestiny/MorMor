using MorMor.Model.NetworkOption;
using MorMor.Terraria;
using Newtonsoft.Json;

namespace MorMor.Configuration;

public class MorMorSetting
{
    [JsonProperty("指令前缀")]
    public List<string> CommamdPrefix { get; init; } = [];

    [JsonProperty("权限所有者")]
    public long OwnerId { get; set; } = 523321293;

    [JsonProperty("监听地址")]
    public string Host { get; init; } = "127.0.0.1";

    [JsonProperty("监听端口")]
    public int Port { get; init; } = 5000;

    [JsonProperty("OneBot令牌")]
    public string AccessToken { get; set; } = "";

    [JsonProperty("数据库类型")]
    public string DbType { get; set; } = "sqlite";

    [JsonProperty("Sqlite路径")]
    public string DbPath { get; set; } = "MorMor.sqlite";

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

    [JsonProperty("Bot用户默认权限组")]
    public string DefaultPermGroup { get; init; } = "default";

    [JsonProperty("邮箱STMP地址")]
    public string MailHost { get; init; } = "";

    [JsonProperty("STMP邮箱")]
    public string SenderMail { get; init; } = "";

    [JsonProperty("STMP授权码")]
    public string SenderPwd { get; init; } = "";

    [JsonProperty("TShockSocket通信端口")]
    public int SocketProt { get; init; } = 6000;

    [JsonProperty("Webhook配置")]
    public WebhookOption WebhookOption { get; init; } = new();

    [JsonProperty("服务器列表")]
    public List<TerrariaServer> Servers { get; } = new();

    public TerrariaServer? GetServer(string name)
    {
        return Servers.Find(x => x.Name == name);
    }

    public TerrariaServer? GetServer(string name, long groupid)
    {
        return Servers.Find(x => x.Name == name && x.Groups.Contains(groupid));
    }
}
