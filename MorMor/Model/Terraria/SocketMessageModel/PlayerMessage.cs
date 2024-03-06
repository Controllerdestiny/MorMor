using MorMor.DB.Manager;
using MorMor.Terraria;
using Newtonsoft.Json;

namespace MorMor.Model.Terraria.SocketMessageModel;

public class PlayerMessage : BaseMessage
{
    [JsonProperty("player_name")]
    public string Name { get; init; }

    [JsonProperty("player_group")]
    public string Group { get; init; }

    [JsonProperty("player_prefix")]
    public string Prefix { get; init; }

    [JsonProperty("player_login")]
    public bool IsLogin { get; init; }

    [JsonIgnore]
    private TerrariaUserManager.User? User
    {
        get
        {
            if (TerrariaServer != null!)
            {
                return MorMorAPI.TerrariaUserManager.GetUsersByName(Name, TerrariaServer.Name);
            }
            return null;
        }
    }

    [JsonIgnore]
    public AccountManager.Account? Account
    {
        get
        {
            if (User != null)
            {
                return MorMorAPI.AccountManager.GetAccount(User.Id);
            }
            return null;
        }
    }
}
