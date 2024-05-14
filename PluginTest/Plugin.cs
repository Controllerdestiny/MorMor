using Test;

namespace PluginTest;

public class Plugin : MorMorPlugin
{
    public override string Description => "这是一个测试插件";

    public override string Author => "少司命";

    public override string Name => Assembly.GetExecutingAssembly().GetName().Name!;

    public override Version Version => Assembly.GetExecutingAssembly().GetName().Version!;

    public override void Initialize()
    {
        HttpServer.OnRequest += HttpServer_OnRequest;
        var server = new HttpServer();
        server.Start();
    }

    private async void HttpServer_OnRequest(Dictionary<string, string> args)
    {
        if (args.TryGetValue("payload", out var data))
        {
            await Console.Out.WriteLineAsync("111");
            try
            {
                var jsonObj = JsonConvert.DeserializeObject<JObject>(data);
                if (jsonObj?["commits"] is JArray arr && arr.Count != 0)
                {
                    if (jsonObj?["pusher"]?["name"]?.ToString() == "github-actions[bot]")
                        return;
                    StringBuilder sb = new($"[{jsonObj?["repository"]?["full_name"]}]\n");
                    foreach (var commit in arr)
                    {
                        sb.AppendLine("Commit信息: " + commit["message"]?.ToString());
                        sb.AppendLine("发起者: " + commit["author"]?["name"]);
                        sb.AppendLine("发起时间: " + commit["timestamp"]);
                        sb.AppendLine("添加文件: " + commit["added"]?.Count() + "个");
                        sb.AppendLine("删除文件: " + commit["removed"]?.Count() + "个");
                        sb.AppendLine("修改文件: " + commit["modified"]?.Count() + "个");
                        sb.AppendLine("Commit链接: " + commit["url"]);
                        sb.AppendLine();
                        sb.AppendLine();
                    }

                    var msg = sb.ToString().Trim() + $"\n{jsonObj?["repository"]?["description"]}:{jsonObj?["repository"]?["html_url"]}";
                    var body = new MessageBody()
                    {
                        msg
                    };
                    var (_, grouplist) = await MomoAPI.Net.OneBotAPI.Instance.GetGroupList();
                    foreach (var group in grouplist)
                    {
                        await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group.ID, body, TimeSpan.FromSeconds(1));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(994731943, new MessageBody() { $"github监听出错: {ex.Message}" });
            }
        }
    }

    private async Task OperatHandler_OnCommand(MorMor.Commands.CommandArgs args)
    {
        //拦截签到指令
        if (args.Name == "签到")
            args.Handler = true;
    }

    private async Task Event_OnGroupMessage(MomoAPI.EventArgs.GroupMessageEventArgs args)
    {
        //复读机
        await args.Reply(args.MessageContext.Messages);
    }

    protected override void Dispose(bool dispose)
    {
        //注销
        MorMorAPI.Service.Event.OnGroupMessage -= Event_OnGroupMessage;
        //注销
        OperatHandler.OnCommand -= OperatHandler_OnCommand;
    }
}
