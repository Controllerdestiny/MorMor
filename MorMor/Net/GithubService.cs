
using Microsoft.Extensions.Primitives;
using MorMor.Model.NetworkOption;
using Octokit.Webhooks;
using System.Net;
using System.Text;

namespace MorMor.Net;


public class GithubService
{
    private WebhookEventProcessor WebhookEventProcessor;

    private HttpListener HttpListener;
    public GithubService()
    {
        HttpListener = new HttpListener();
    }

    public void Start<T>(WebhookOption option) where T : WebhookEventProcessor, new()
    {
        if (!option.Enable)
            return;
        HttpListener.Prefixes.Add($"http://*:{option.Port}{option.Path}");
        HttpListener.Start();
        WebhookEventProcessor = new T();
        HttpListener.BeginGetContext(OnContext, null);
    }

    private async void OnContext(IAsyncResult ar)
    {
        HttpListener.BeginGetContext(OnContext, null);
        var data = HttpListener.EndGetContext(ar);
        if (data.Request.HttpMethod == "POST")
        {
            var hearder = new Dictionary<string, StringValues>();
            foreach (var key in data.Request.Headers.AllKeys)
            {
                if (!string.IsNullOrEmpty(key))
                    hearder[key] = data.Request.Headers[key];
            }
            using StreamReader stream = new(data.Request.InputStream);
            var body = stream.ReadToEnd();
            await WebhookEventProcessor.ProcessWebhookAsync(hearder, body);
        }
        var result = Encoding.UTF8.GetBytes("response on github");
        data.Response.StatusCode = 200;
        data.Response.OutputStream.Write(result, 0, result.Length);
        data.Response.Close();
    }
}