using MomoAPI.EventArgs;
using MorMor.DB.Manager;

namespace MorMor.Commands;

public class CommandArgs(string name, GroupMessageEventArgs args, string commamdPrefix, List<string> parameters, Dictionary<string, string> commamdLine, AccountManager.Account account) 
    : BaseCommandArgs(name,commamdPrefix,parameters,commamdLine)
{
    public GroupMessageEventArgs EventArgs { get; } = args;

    public AccountManager.Account Account { get; } = account;

}
