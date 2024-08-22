

using MorMor.DB.Manager;

namespace MorMor.Commands;

public class BaseCommandArgs(string name, string prefix, List<string> param, Dictionary<string,string> cmdLine) : System.EventArgs
{
    public string Name { get; } = name;

    public string CommamdPrefix { get; } = prefix;

    public List<string> Parameters { get; } = param;

    public Dictionary<string, string> CommamdLine { get; } = cmdLine;

    public bool Handler { get; set; }
}
