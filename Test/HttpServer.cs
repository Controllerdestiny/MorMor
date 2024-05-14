namespace Test;


public class HttpServer
{
    public delegate void CallBack(Dictionary<string, string> args);

    public static event CallBack? OnRequest;

    public WebhookEventProcessor WebhookEventProcessor { get; private set; }

    private HttpListener HttpListener;
    public HttpServer()
    {
        HttpListener = new HttpListener();
    }

    public void Start<T>() where T : WebhookEventProcessor, new()
    {
        HttpListener.Prefixes.Add("http://*:7000/update/");
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