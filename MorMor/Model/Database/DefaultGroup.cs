using MorMor.Permission;

namespace MorMor.Model.Database;

public class DefaultGroup : Group
{
    public override List<string> permissions => new()
    {
        OneBotPermissions.Sign,
        OneBotPermissions.Help,
        OneBotPermissions.Jrrp,
        OneBotPermissions.CurrencyUse,
        OneBotPermissions.Nbnhhsh,
        OneBotPermissions.QueryOnlienPlayer,
        OneBotPermissions.QueryProgress,
        OneBotPermissions.QueryInventory,
        OneBotPermissions.ChangeServer,
        OneBotPermissions.QueryUserList,
        OneBotPermissions.GenerateMap,
        OneBotPermissions.ServerList,
        OneBotPermissions.RegisterUser,
        OneBotPermissions.OnlineRank,
        OneBotPermissions.DeathRank,
        OneBotPermissions.SelfInfo,
        OneBotPermissions.TerrariaWiki,
        OneBotPermissions.Version,
        OneBotPermissions.Music,
        OneBotPermissions.EmojiLike,
        OneBotPermissions.TerrariaShop
    };
    public DefaultGroup() : base(MorMorAPI.Setting.DefaultPermGroup)
    {
    }
}
