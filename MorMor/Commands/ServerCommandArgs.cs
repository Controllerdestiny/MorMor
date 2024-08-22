using MomoAPI.EventArgs;
using MorMor.DB.Manager;
using MorMor.TShock.Server;

namespace MorMor.Commands;

public class ServerCommandArgs(string serverName, string userName, string cmdName, string commamdPrefix, List<string> parameters, Dictionary<string, string> commamdLine) 
    : BaseCommandArgs(cmdName, commamdPrefix, parameters, commamdLine)
{
    public string ServerName { get; } = serverName;

    public string UserName { get; } = userName;

    public TerrariaServer? Server => MorMorAPI.Setting.GetServer(ServerName);

    public TerrariaUserManager.User? User => MorMorAPI.TerrariaUserManager.GetUsersByName(UserName, ServerName);

    public AccountManager.Account Account => MorMorAPI.AccountManager.GetAccountNullDefault(User == null ? 0 : User.Id);

}
