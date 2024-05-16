using System.ComponentModel;

namespace MorMor.Enumeration;

public enum GithubActionType
{
    [Description("release")]
    Release,

    PullRequest,

    Push,

    Stared
}
