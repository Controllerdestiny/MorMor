using MorMor.Attributes;
using MorMor.Commands;
using MorMor.Event;
using MorMor.Model.Socket.PlayerMessage;
using System.Drawing;
using System.Reflection;

namespace MorMor.TShock.ChatCommand;

internal class ChatCommandMananger
{
    public static readonly ChatCommandMananger Hook = new();

    internal readonly List<ChatCommand> commands = new();
    private ChatCommandMananger()
    {

    }

    public void Add(ChatCommand command)
    {
        commands.Add(command);
    }

    public async Task CommandAdapter(PlayerCommandMessage args)
    {
        var text = args.Command;
        var cmdParam = CommandManager.Hook.ParseParameters(text[args.CommandPrefix.Length..]);
        if (cmdParam.Count > 0)
        {
            var cmdName = cmdParam[0];
            cmdParam.RemoveAt(0);
            foreach (var command in commands)
            {
                if (command.Name.Contains(cmdName))
                {
                    try
                    {
                        await RunCommandCallback(new PlayerCommandArgs(args.Name, args.ServerName, cmdParam), command);
                    }
                    catch (Exception ex)
                    {
                        MorMorAPI.Log.ConsoleError(ex.ToString());
                        if (args.TerrariaServer != null)
                            await args.TerrariaServer.PrivateMsg(args.Name, ex.Message, Color.DarkRed);
                    }
                }
            }
        }
    }

    public void MappingCommands(Assembly assembly)
    {
        var flag = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public;
        Dictionary<Type, MethodInfo[]> mapping = assembly.GetExportedTypes()
            .Where(x => x.IsDefined(typeof(CommandSeries)))
            .Select(type => (type, type.GetMethods(flag)
            .Where(m => m.IsDefined(typeof(CommandMatch)) && m.CommandParamPares(typeof(PlayerCommandArgs)))
            .ToArray()))
            .ToDictionary(method => method.type, method => method.Item2);
        foreach (var (cls, methods) in mapping)
        {
            var instance = Activator.CreateInstance(cls);
            if (instance == null)
                continue;
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<CommandMatch>()!;
                if (method.IsStatic)
                {
                    Add(new(attr.Name, method.CreateDelegate<ChatCommand.CommandCallBack>(), attr.Permission));
                    continue;
                }
                var _method = instance.GetType().GetMethod(method.Name, flag)!;
                Add(new(attr.Name, _method.CreateDelegate<ChatCommand.CommandCallBack>(instance), attr.Permission));
            }
        }
    }

    private async Task RunCommandCallback(PlayerCommandArgs args, ChatCommand command)
    {
        foreach (var perm in command.Permission)
        {
            if (args.Account.HasPermission(perm))
            {
                if (!await OperatHandler.ServerUserCommand(args))
                {
                    await command.CallBack(args);
                    MorMorAPI.Log.ConsoleInfo($"Server:{args.ServerName} {args.Name} 使用命令: {command.Name.First()}", ConsoleColor.Cyan);
                }
                return;
            }
        }
    }
}
