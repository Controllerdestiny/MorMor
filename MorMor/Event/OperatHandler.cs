using MorMor.Commands;
using MorMor.DB.Manager;
using MorMor.Enumeration;
using MorMor.EventArgs;
using MorMor.TShock.ChatCommand;

namespace MorMor.Event;

public static class OperatHandler
{
    public delegate IResult EventCallBack<in IEventArgs, out IResult>(IEventArgs args) where IEventArgs : System.EventArgs;

    public static event EventCallBack<PermissionEventArgs, UserPermissionType>? OnUserPermission;

    public static event EventCallBack<CommandArgs, ValueTask>? OnCommand;

    public static event EventCallBack<ReloadEventArgs, ValueTask>? OnReload;

    public static event EventCallBack<GroupMessageForwardArgs, ValueTask>? OnGroupMessageForward;

    public static event EventCallBack<PlayerCommandArgs, ValueTask>? OnServerCommand;

    public static UserPermissionType UserPermission(AccountManager.Account account, string prem)
    {
        if (account.UserId == MorMorAPI.Setting.OwnerId)
            return UserPermissionType.Granted;
        if (OnUserPermission == null)
            return UserPermissionType.Denied;
        var args = new PermissionEventArgs(account, prem, UserPermissionType.Denied);
        return OnUserPermission(args);
    }

    public static async ValueTask<bool> MessageForward(GroupMessageForwardArgs args)
    {
        if (OnGroupMessageForward == null)
            return false;
        await OnGroupMessageForward(args);
        return args.Handler;
    }

    public static async ValueTask<bool> UserCommand(CommandArgs args)
    {
        if (OnCommand == null)
            return false;
        await OnCommand(args);
        return args.Handler;
    }

    public static async ValueTask Reload(ReloadEventArgs args)
    {
        if (OnReload != null)
            await OnReload(args);
    }

    internal static async ValueTask<bool> ServerUserCommand(PlayerCommandArgs args)
    {
        if (OnServerCommand == null)
            return false;
        await OnServerCommand(args);
        return args.Handler;
    }
}
