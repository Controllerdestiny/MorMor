
using MorMor.DB.Manager;

namespace MorMor.Terraria.ChatCommand;

public class PlayerCommandArgs : System.EventArgs
{
    public string Name { get; set; }

    public string ServerName { get; set; }

    public string CommamdPrefix { get; set; }

    public List<string> Parameters { get; set; }

    public bool Handler { get; set; }

    public TerrariaServer? Server => MorMorAPI.Setting.GetServer(ServerName);

    public TerrariaUserManager.User? User => MorMorAPI.TerrariaUserManager.GetUsersByName(ServerName, Name);

    public AccountManager.Account Account => MorMorAPI.AccountManager.GetAccountNullDefault(User == null ? 0 : User.Id);

    public PlayerCommandArgs(string name, string server, List<string> parameters)
    {
        Name = name;
        ServerName = server;
        Parameters = parameters;

    }
}
