using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Model.Database;

public class SuperAdminGroup : Group
{
    public override List<string> TotalPermissions => new() { "*" };

    public SuperAdminGroup()
        : base("superadmin")
    {

    }

    public override bool HasPermission(string permission) => true;
}
