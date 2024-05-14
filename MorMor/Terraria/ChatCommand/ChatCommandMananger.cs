using MomoAPI.Utils;
using MorMor.Attributes;
using MorMor.Commands;
using MorMor.Event;
using MorMor.Model.Socket.PlayerMessage;
using System.Reflection;


namespace MorMor.Terraria.ChatCommand;

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
                    await RunCommandCallback(new PlayerCommandArgs(args.Name, args.ServerName, cmdParam), command);
                }
            }
        }
    }

    internal void MappingCommands(Assembly assembly)
    {
        Dictionary<Type, object> types = new();
        assembly.GetTypes().ForEach(x =>
        {
            if (!x.IsAbstract && !x.IsInterface)
            {
                var flag = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public;
                //加载程序集所有类
                x.GetMethods(flag).ForEach(m =>
                {
                    //参数是否匹配
                    if (m.CommandParamPares(typeof(PlayerCommandArgs)))
                    {
                        //获取特性
                        var attribute = m.GetCustomAttribute<CommandMatch>();
                        if (attribute != null)
                        {
                            //方法非静态
                            if (!m.IsStatic && x.GetConstructors().ClassConstructParamIsZerp())
                            {
                                var instance = types.TryGetValue(x, out var obj) && obj != null ? obj : Activator.CreateInstance(x);
                                //缓存对象
                                types[x] = instance;
                                var method = instance?.GetType().GetMethod(m.Name, flag);
                                if (method != null)
                                {
                                    Add(new(attribute.Name, method.CreateDelegate<ChatCommand.CommandCallBack>(instance), attribute.Permission));
                                }
                            }
                            else
                            {
                                Add(new(attribute.Name, m.CreateDelegate<ChatCommand.CommandCallBack>(), attribute.Permission));
                            }
                        }
                    }
                });
            }
        });
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
        //MorMorAPI.Log.ConsoleInfo($"group: {args.EventArgs.Group.Id} {args.EventArgs.SenderInfo.Name}({args.EventArgs.SenderInfo.UserId}) 试图使用命令: {args.CommamdPrefix}{args.Name}", ConsoleColor.Yellow);
        //await args.EventArgs.Reply("你无权使用此命令！");
    }
}
