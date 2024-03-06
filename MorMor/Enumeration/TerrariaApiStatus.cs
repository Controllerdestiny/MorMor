using System.ComponentModel;

namespace MorMor.Enumeration;

public enum TerrariaApiStatus
{
    [Description("200")]
    Success,

    Error,

    DisposeConnect
}
