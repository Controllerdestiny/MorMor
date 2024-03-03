using MorMor.Permission;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Model.Database;

public class DefaultGroup : Group
{
    public override List<string> permissions => new()
    { 
        Permissions.Sign,
        Permissions.Help,
        Permissions.Jrrp,
        Permissions.CurrencyUse,
        Permissions.Nbnhhsh
    };
    public DefaultGroup() : base(MorMorAPI.Setting.DefaultPermGroup)
    {
    }
}
