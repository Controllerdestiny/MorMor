using MomoAPI.Utils;
using MorMor.Enumeration;
using MorMor.Terraria.Server.ApiRequestParam;
using MorMor.Utils;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MorMor.Terraria.Server;

public class ApiRequest
{
    private static HttpClient httpClient = new();
    public static async Task<T> Send<T>(TerrariaServer server, TerrariaApiType action, Dictionary<string, string> param = null) where T : BaseResultArgs, new()
    {
        var url = new StringBuilder($"http://{server.IP}:{server.RestPort}");
        url.Append(DescriptionUtil.GetFieldDesc(action));
        url.Append("?token=").Append(server.Token);
        if (param != null && param.Count > 0)
            param.ForEach(x => url.Append($"&{x.Key}={x.Value}"));
        try
        {
            var result = await httpClient.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url.ToString().TrimEnd('&'))
            });
            var str = await result.Content.ReadAsStringAsync();
            var args = JObject.Parse(str);
            return args.ToObject<T>();
        }
        catch
        {
            return new T()
            {
                Status = TerrariaApiStatus.DisposeConnect,
                ErrorMessage = "无法连接到服务器!"
            };
        }


    }
}
