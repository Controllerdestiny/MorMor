using MorMor.DB.Manager;
using MorMor.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
