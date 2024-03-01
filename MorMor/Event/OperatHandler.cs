using MorMor.Commands;
using MorMor.DB.Manager;
using MorMor.Enumeration;
using MorMor.EventArgs;

namespace MorMor.Event;

public static class OperatHandler
{
    public delegate IResult EventCallBack<in IEventArgs, out IResult>(IEventArgs args) where IEventArgs : System.EventArgs;

    public static event EventCallBack<PermissionEventArgs, UserPermissionType> OnUserPermission;

    public static event EventCallBack<CommandArgs, Task> OnCommand;

    public static event EventCallBack<ReloadEventArgs, Task> OnReload;

    public static UserPermissionType UserPermission(AccountManager.Account account, string prem)
    {
        if (OnUserPermission == null)
            return UserPermissionType.Denied;
        var args = new PermissionEventArgs(account, prem, UserPermissionType.Denied);
        return OnUserPermission(args);
    }

    public static async Task<bool> UserCommand(CommandArgs args)
    {
        if (OnCommand == null)
            return false;
        await OnCommand(args);
        return args.Handler;
    }

    public static async Task Reload(ReloadEventArgs args)
    {
        if (OnReload != null)
            await OnReload(args);
    }
}
