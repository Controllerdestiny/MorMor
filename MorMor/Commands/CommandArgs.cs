using MomoAPI.EventArgs;
using MorMor.DB.Manager;

namespace MorMor.Commands;

public class CommandArgs : System.EventArgs
{
    public string Name { get; }

    public GroupMessageEventArgs EventArgs { get; }

    public string CommamdPrefix { get; }

    public List<string> Parameters { get; }

    public Dictionary<string, string> CommamdLine { get; }

    public AccountManager.Account Account { get; }

    public bool Handler { get; set; }

    public CommandArgs(string name, GroupMessageEventArgs args, string commamdPrefix, 
        List<string> parameters, Dictionary<string, string> commamdLine, AccountManager.Account account)
    {
        Name = name;
        EventArgs = args;
        CommamdPrefix = commamdPrefix;
        Parameters = parameters;
        CommamdLine = commamdLine;
        Account = account;
    }
}
