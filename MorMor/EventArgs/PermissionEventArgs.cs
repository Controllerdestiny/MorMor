using MorMor.DB.Manager;
using MorMor.Enumeration;

namespace MorMor.EventArgs;

public class PermissionEventArgs : System.EventArgs
{
    public AccountManager.Account Account { get; }

    public string permission { get; }

    public UserPermissionType UserPermissionType { get; set; }

    public PermissionEventArgs(AccountManager.Account account, string perm, UserPermissionType userPermissionType)
    {
        Account = account;
        permission = perm;
        UserPermissionType = userPermissionType;
    }
}
