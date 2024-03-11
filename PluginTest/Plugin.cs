using MorMor;
using MorMor.Event;
using MorMor.Plugin;
using System.Reflection;

namespace PluginTest;

public class Plugin : MorMorPlugin
{
    public override string Description => "这是一个测试插件";

    public override string Author => "少司命";

    public override string Name => Assembly.GetExecutingAssembly().GetName().Name!;

    public override Version Version => Assembly.GetExecutingAssembly().GetName().Version!;

    public override void Initialize()
    {
        //注册群事消息事件
        MorMorAPI.Service.Event.OnGroupMessage += Event_OnGroupMessage;
        //注册指令执行事件
        OperatHandler.OnCommand += OperatHandler_OnCommand;
    }

    private async Task OperatHandler_OnCommand(MorMor.Commands.CommandArgs args)
    {
        //拦截签到指令
        if (args.Name == "签到")
            args.Handler = true;
    }

    private async Task Event_OnGroupMessage(MomoAPI.EventArgs.GroupMessageEventArgs args)
    {
        //复读机
        await args.Reply(args.MessageContext.Messages);
    }

    protected override void Dispose(bool dispose)
    {
        //注销
        MorMorAPI.Service.Event.OnGroupMessage -= Event_OnGroupMessage;
        //注销
        OperatHandler.OnCommand -= OperatHandler_OnCommand;
    }
}
