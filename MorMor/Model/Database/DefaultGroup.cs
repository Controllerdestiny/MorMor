using MorMor.Permission;

namespace MorMor.Model.Database;

public class DefaultGroup : Group
{
    public List<string> Selfpermissions =
    [
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
        OneBotPermissions.TerrariaShop,
        OneBotPermissions.TerrariaPrize,
        OneBotPermissions.ImageEmoji,
        OneBotPermissions.SelfPassword,
        OneBotPermissions.SearchItem
    ];

    public override void NegatePermission(string permission)
    {
        base.NegatePermission(permission);
    }
    public override void RemovePermission(string permission)
    {
        base.RemovePermission(permission);
    }

    public override bool HasPermission(string permission)
    {
        return base.HasPermission(permission);
    }
    public override void AddPermission(string permission)
    {
        base.AddPermission(permission);
    }
    public DefaultGroup() : base(MorMorAPI.Setting.DefaultPermGroup)
    {
        if (permissions.Count == 0)
            SetPermission(Selfpermissions);
    }
}
