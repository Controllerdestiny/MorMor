using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Enumeration;

public enum UserPermissionType
{
    /// <summary>
    /// 绝对无权
    /// </summary>
    Unhandled,

    /// <summary>
    /// 正常判断
    /// </summary>
    Denied,

    /// <summary>
    /// 绝对有权
    /// </summary>
    Granted,
}
