using System.Diagnostics;
using System.Text;
using MomoAPI.Entities;
using MorMor.Commands;
using MorMor.Extensions;
using MorMor.Plugin;
using MorMor.TShock.ChatCommand;

namespace PluginManager;

public class Plugin : MorMorPlugin
{
    public override string Name => "PluginManager";

    public override string Description => "提供插件管理功能";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    public override void Initialize()
    {
        CommandManager.Hook.Add(new("pm", PManager, "onebot.plugin.admin"));
        CommandManager.Hook.Add(new("重载插件", HotReloadPlugin, "onebot.plugin.admin"));
    }

    protected override void Dispose(bool dispose)
    {
        CommandManager.Hook.commands.RemoveAll(x => x.CallBack == PManager);
        CommandManager.Hook.commands.RemoveAll(x => x.CallBack == HotReloadPlugin);
    }

    private async Task HotReloadPlugin(CommandArgs args)
    {
        Stopwatch sw = new();
        sw.Start();
        CommandManager.Hook.commands.Clear();
        ChatCommandMananger.Hook.commands.Clear();
        MappingPlugin.UnLoadPlugin();
        MappingPlugin.Initializer();
        sw.Stop();
        await args.EventArgs.Reply($"插件热重载成功!\n用时:{sw.Elapsed.TotalSeconds:F5} 秒!", true);
    }

    private async Task PManager(CommandArgs args)
    {
        if (args.Parameters.Count == 1 && args.Parameters[0].ToLower() == "list")
        {
            var sb = new StringBuilder();
            sb.AppendLine($$"""<div align="center">""");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine($"# 插件列表");
            sb.AppendLine();
            sb.AppendLine("|序号|插件名称|插件作者|插件说明|插件版本|");
            sb.AppendLine("|:--:|:--:|:--:|:--:|:--:|");
            int index = 1;
            foreach (var plugin in MappingPlugin.AssemblyLoadContext.Plugins)
            {
                sb.AppendLine($"|{index}|{plugin.Name}|{plugin.Author}|{plugin.Description}|{plugin.Version}");
                index++;
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("</div>");
            await args.EventArgs.Reply(new MessageBody().MarkdownImage(sb.ToString()));
        }
        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "off")
        {
            if (!int.TryParse(args.Parameters[1], out var index) || index < 1 || index > MappingPlugin.AssemblyLoadContext.Plugins.Count)
            {
                await args.EventArgs.Reply("请输入一个正确的序号!", true);
                return;
            }
            var instance = MappingPlugin.AssemblyLoadContext.Plugins[index - 1];
            instance.Dispose();
            await args.EventArgs.Reply($"{instance.Name} 插件卸载成功!", true);
        }
        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "on")
        {
            if (!int.TryParse(args.Parameters[1], out var index) || index < 1 || index > MappingPlugin.AssemblyLoadContext.Plugins.Count)
            {
                await args.EventArgs.Reply("请输入一个正确的序号!", true);
                return;
            }
            var instance = MappingPlugin.AssemblyLoadContext.Plugins[index - 1];
            instance.Initialize();
            await args.EventArgs.Reply($"{instance.Name} 插件加载成功!", true);
        }
        else
        {
            await args.EventArgs.Reply("语法错误,正确语法:\n" +
                $"${args.CommamdPrefix}{args.Name} list" +
                $"${args.CommamdPrefix}{args.Name} off [序号]" +
                $"${args.CommamdPrefix}{args.Name} on [序号]");
        }
    }
}
