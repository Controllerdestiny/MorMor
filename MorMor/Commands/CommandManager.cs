using MomoAPI.EventArgs;
using MomoAPI.Utils;
using MorMor.Attributes;
using MorMor.Event;
using System.Reflection;
using System.Text;

namespace MorMor.Commands;

public class CommandManager
{
    public static readonly CommandManager Hook = new();

    internal readonly List<Command> commands = new();
    private CommandManager()
    {

    }

    public void Add(Command command)
    {
        commands.Add(command);
    }

    private List<string> ParseParameters(string str)
    {
        var ret = new List<string>();
        var sb = new StringBuilder();
        bool instr = false;
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];

            if (c == '\\' && ++i < str.Length)
            {
                if (str[i] != '"' && str[i] != ' ' && str[i] != '\\')
                    sb.Append('\\');
                sb.Append(str[i]);
            }
            else if (c == '"')
            {
                instr = !instr;
                if (!instr)
                {
                    ret.Add(sb.ToString());
                    sb.Clear();
                }
                else if (sb.Length > 0)
                {
                    ret.Add(sb.ToString());
                    sb.Clear();
                }
            }
            else if (IsWhiteSpace(c) && !instr)
            {
                if (sb.Length > 0)
                {
                    ret.Add(sb.ToString());
                    sb.Clear();
                }
            }
            else
                sb.Append(c);
        }
        if (sb.Length > 0)
            ret.Add(sb.ToString());

        return ret;
    }

    private bool IsWhiteSpace(char c)
    {
        return c == ' ' || c == '\t' || c == '\n';
    }

    private Dictionary<string, string> ParseCommandLine(List<string> command)
    {
        var args = new Dictionary<string, string>();
        for (int i = 0; i < command.Count; i++)
        {
            var cmd = command[i];
            if (cmd.StartsWith("-"))
            {
                var str = "";
                for (int j = i + 1; j < command.Count; j++)
                {
                    if (!command[j].StartsWith("-"))
                        str += " " + command[j];
                    else
                        break;
                }
                if (!string.IsNullOrEmpty(str.Trim()))
                    args[cmd] = str.Trim();
            }
        }
        return args;
    }

    public async Task CommandAdapter(GroupMessageEventArgs args)
    {
        var text = args.MessageContext.GetText();
        string prefix = string.Empty;
        MorMorAPI.Setting.CommamdPrefix.ForEach(x =>
        {
            if (text.StartsWith(x))
            {
                prefix = x;
            }
        });
        if (!string.IsNullOrEmpty(prefix))
        {
            var cmdParam = ParseParameters(text[prefix.Length..]);
            if (cmdParam.Count > 0)
            {
                var cmdName = cmdParam[0];
                cmdParam.RemoveAt(0);
                var account = MorMorAPI.AccountManager.TryGetAccoumt(args.Sender.Id, args.Group.Id, out var user) ? user
                            : new DB.Manager.AccountManager.Account(args.Sender.Id, new(MorMorAPI.Setting.DefaultPermGroup, args.Group.Id));
                foreach (var command in commands)
                {
                    if (command.Name.Contains(cmdName))
                    {
                        await RunCommandCallback(new CommandArgs(cmdName, args, prefix, cmdParam, ParseCommandLine(cmdParam), account), command);
                    }
                }
            }
        }
    }

    private async Task RunCommandCallback(CommandArgs args, Command command)
    {
        foreach(var perm in command.Permission)
        {
            if (args.Account.HasPermission(perm))
            {
                if (!await OperatHandler.UserCommand(args))
                    await command.CallBack(args);
                return;
            }
        }
        await args.EventArgs.Reply("你无权使用此命令！");
    }

    public void MappingCommands(Assembly assembly)
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
                    if (m.CommandParamPares())
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
                                    Add(new(attribute.Name, method.CreateDelegate<Command.CommandCallBack>(instance), attribute.Permission));
                                }
                            }
                            else
                            {
                                Add(new(attribute.Name, m.CreateDelegate<Command.CommandCallBack>(), attribute.Permission));
                            }
                        }
                    }
                });
            }
        });
    }
}
