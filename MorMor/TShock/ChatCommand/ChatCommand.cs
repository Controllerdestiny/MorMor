namespace MorMor.TShock.ChatCommand;

public class ChatCommand
{
    public delegate Task CommandCallBack(PlayerCommandArgs args);

    public List<string> Name { get; }

    public CommandCallBack CallBack { get; }

    public List<string> Permission { get; }

    public ChatCommand(List<string> name, CommandCallBack callBack, params string[] permission)
    {
        Name = name;
        CallBack = callBack;
        Permission = permission.ToList();
    }

    public ChatCommand(string name, CommandCallBack callBack, params string[] permission)
    {
        Name = [name];
        CallBack = callBack;
        Permission = [.. permission];
    }
}
