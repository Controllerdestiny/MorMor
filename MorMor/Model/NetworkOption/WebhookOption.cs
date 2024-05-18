using Newtonsoft.Json;

namespace MorMor.Model.NetworkOption;

public class WebhookOption
{
    [JsonProperty("启用")]
    public bool Enable { get; set; } = true;

    [JsonProperty("路由")]
    public string Path { get; set; } = "/update/";

    [JsonProperty("端口")]
    public int Port { get; set; } = 7000;

    [JsonProperty("监听事件")]
    public Dictionary<string, List<long>> GithubActions { get; set; } = [];

    public void Add(string eventType, long groupid)
    {
        if (GithubActions.TryGetValue(eventType, out var groups))
        {
            if (groups == null)
                GithubActions[eventType] = [groupid];
            else
                GithubActions[eventType].Add(groupid);
        }
        else
        {
            GithubActions[eventType] = [groupid];
        }
    }

    public void Remove(string eventType, long groupid)
    {
        if (GithubActions.TryGetValue(eventType, out var groups))
        {
            if (groups != null)
                GithubActions[eventType].Remove(groupid);
        }
    }
}
