namespace Test;

public class WebHookReceive : WebhookEventProcessor
{
    protected override async Task ProcessPullRequestWebhookAsync(WebhookHeaders headers, PullRequestEvent pullRequestEvent, PullRequestAction action)
    {
        await Console.Out.WriteLineAsync(pullRequestEvent.Action);
    }

}
