using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Enumeration;

public enum TerrariaApiStatus
{
    [Description("200")]
    Success,

    Error,

    DisposeConnect
}
