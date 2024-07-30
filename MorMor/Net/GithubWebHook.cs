using MomoAPI.Entities;
using MorMor.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit.Webhooks;
using Octokit.Webhooks.Events;
using Octokit.Webhooks.Events.PullRequest;
using Octokit.Webhooks.Events.Release;
using Octokit.Webhooks.Events.Star;
using System.Text;
namespace MorMor.Net;

public class GithubWebHook : WebhookEventProcessor
{

    public async Task SendGroupMsg(MessageBody body, IEnumerable<long> groups)
    {
        foreach (var group in groups)
        {
            await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, body, TimeSpan.FromSeconds(10));
        }
    }

    protected override async Task ProcessReleaseWebhookAsync(WebhookHeaders headers, ReleaseEvent releaseEvent, ReleaseAction action)
    {
        MorMorAPI.Setting.WebhookOption.GithubActions.TryGetValue(WebhookEventType.Release, out var group);

        if (!MorMorAPI.Setting.WebhookOption.GithubActions.TryGetValue(WebhookEventType.Release, out var groups)
            || groups == null
            || groups.Count == 0)
            return;
        if (action == ReleaseAction.Edited || action == ReleaseAction.Released)
        {
            try
            {
                HttpClient client = new();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Linux; Android 13; 22127RK46C Build/TKQ1.220905.001) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.97 Mobile Safari/537.36");
                var mdTemp = $"""<svg title="Merged" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true" style="fill:purple;"><path d="M5.45 5.154A4.25 4.25 0 0 0 9.25 7.5h1.378a2.251 2.251 0 1 1 0 1.5H9.25A5.734 5.734 0 0 1 5 7.123v3.505a2.25 2.25 0 1 1-1.5 0V5.372a2.25 2.25 0 1 1 1.95-.218ZM4.25 13.5a.75.75 0 1 0 0-1.5.75.75 0 0 0 0 1.5Zm8.5-4.5a.75.75 0 1 0 0-1.5.75.75 0 0 0 0 1.5ZM5 3.25a.75.75 0 1 0 0 .005V3.25Z"></path></svg>""";
                var repName = releaseEvent.Repository?.FullName;
                var sb = new StringBuilder($"# Release Github 仓库 {repName}");
                sb.AppendLine();
                sb.AppendLine("## 有新的版本更新");
                sb.AppendLine($"- 当前时间: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                var pullUrl = "https://api.github.com/repos/Controllerdestiny/TShockPlugin/pulls?state=closed&per_page=5";
                var pullList = await client.GetStringAsync(pullUrl);
                var list = JsonConvert.DeserializeObject<JArray>(pullList);
                if (list != null)
                { 
                     foreach (var pull in list)
                    {
                        if (pull.Value<string>("merged_at") != null)
                        {
                            var title = pull.Value<string>("title");
                            var number = pull.Value<string>("number");
                            var html_url = pull.Value<string>("html_url");
                            var create_time = pull.Value<string>("created_at")?.ToString();
                            sb.AppendLine(mdTemp + $"[{title} #{number}]({html_url}) ({DateTime.Parse(create_time!).ToString("yyyy.MM.dd")})");
                        }
                    }
                }
               
                var result = await client.GetStringAsync(releaseEvent.Release.AssetsUrl);
                var jobj = JsonConvert.DeserializeObject<JArray>(result);
                var download = jobj?[0]?["browser_download_url"]?.ToString();
                var update = jobj?[0]?["updated_at"]?.ToString();
                if (!string.IsNullOrEmpty(download))
                {
                    var file = await client.GetByteArrayAsync("https://github.moeyy.xyz/" + download);
                    await SendGroupMsg(new MessageBody().MarkdownImage(sb.ToString()), groups);
                    await SendGroupMsg(new MessageBody().File("base64://" + Convert.ToBase64String(file), $"({Guid.NewGuid().ToString()[..8]})Plugins.zip"), groups);
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }

        }
    }

    protected override async Task ProcessStarWebhookAsync(WebhookHeaders headers, StarEvent starEvent, StarAction action)
    {
        if (!MorMorAPI.Setting.WebhookOption.GithubActions.TryGetValue(WebhookEventType.Star, out var groups)
            || groups == null
            || groups.Count == 0)
            return;
        var msg = $"用户 {starEvent.Sender?.Login} Stared 仓库 {starEvent.Repository?.FullName} 共计({starEvent.Repository?.StargazersCount})个Star";
        await SendGroupMsg(msg, groups);
    }
    protected override async Task ProcessPushWebhookAsync(WebhookHeaders headers, PushEvent pushEvent)
    {
        if (!MorMorAPI.Setting.WebhookOption.GithubActions.TryGetValue(WebhookEventType.Push, out var groups)
            || groups == null
            || groups.Count == 0)
            return;
        if (pushEvent.Pusher.Name != "github-actions[bot]")
        {
            var repName = pushEvent.Repository?.FullName;
            var sb = new StringBuilder($"# Push Github 仓库 {repName}");
            foreach (var commit in pushEvent.Commits)
            {
                sb.AppendLine();
                sb.AppendLine($"### {commit.Message}");
                sb.AppendLine($"- 用户名: `{commit.Author.Username}`");
                sb.AppendLine($"- 添加文件: {(commit.Added.Any() ? string.Join(" ", commit.Added.Select(x => $"`{x}`")) : "无")}");
                sb.AppendLine($"- 删除文件: {(commit.Removed.Any() ? string.Join(" ", commit.Removed.Select(x => $"`{x}`")) : "无")}");
                sb.AppendLine($"- 更改文件: {(commit.Modified.Any() ? string.Join(" ", commit.Modified.Select(x => $"`{x}`")) : "无")}");
            }
            await SendGroupMsg(new MessageBody().MarkdownImage(sb.ToString()), groups);
        }
    }

    protected override async Task ProcessPullRequestWebhookAsync(WebhookHeaders headers, PullRequestEvent pullRequestEvent, PullRequestAction action)
    {
        if (!MorMorAPI.Setting.WebhookOption.GithubActions.TryGetValue(WebhookEventType.PullRequest, out var groups)
            || groups == null
            || groups.Count == 0)
            return;
        if (action == PullRequestAction.Opened)
        {
            var title = pullRequestEvent.PullRequest.Title;
            var userName = pullRequestEvent.PullRequest.User.Login;
            var repName = pullRequestEvent.Repository?.FullName;
            var sb = new StringBuilder($"# Pull Request Github 仓库 {repName}");
            sb.AppendLine();
            sb.AppendLine($"## {title}");
            sb.AppendLine($"- 发起者: `{userName}`");
            sb.AppendLine($"```");
            sb.AppendLine(pullRequestEvent.PullRequest.Body);
            sb.AppendLine($"```");
            await SendGroupMsg(new MessageBody().MarkdownImage(sb.ToString()), groups);
        }

    }
}
