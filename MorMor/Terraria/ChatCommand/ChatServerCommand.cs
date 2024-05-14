using MorMor.Attributes;
using MorMor.Permission;
using System.Drawing;

namespace MorMor.Terraria.ChatCommand;

public class ChatServerCommand
{
    [CommandMatch("购买", OneBotPermissions.TerrariaShop)]
    public static async Task ShopBuy(PlayerCommandArgs args)
    {
        if (args.Server == null) return;
        if (args.Parameters.Count != 1)
        {
            await args.Server.PrivateMsg($"语法错误:\n正确语法:/购买 [名称|ID]", Color.GreenYellow);
            return;
        }
        //if (MorMorAPI.UserLocation.TryGetServer(args.EventArgs.Sender.Id, args.EventArgs.Group.Id, out var server) && server != null)
        //{
        //    var user = MorMorAPI.TerrariaUserManager.GetUserById(args.EventArgs.Sender.Id, server.Name);
        if (args.User != null)
        {
            if (int.TryParse(args.Parameters[0], out var id))
            {
                if (MorMorAPI.TerrariaShop.TryGetShop(id, out var shop) && shop != null)
                {
                    var curr = MorMorAPI.CurrencyManager.Query(args.User.GroupID, args.User.Id);
                    if (curr != null && curr.num >= shop.Price)
                    {
                        var res = await args.Server.Command($"/g {shop.ID} {args.Name} {shop.num}");
                        if (res.Status)
                        {
                            MorMorAPI.CurrencyManager.Del(args.User.GroupID, args.User.Id, shop.Price);
                            await args.Server.PrivateMsg("购买成功!", Color.GreenYellow);
                        }
                        else
                        {
                            await args.Server.PrivateMsg("失败! 错误信息:\n" + res.Message, Color.GreenYellow);
                        }
                    }
                    else
                    {
                        await args.Server.PrivateMsg("星币不足!", Color.GreenYellow);
                    }
                }
                else
                {
                    await args.Server.PrivateMsg("该商品不存在!", Color.GreenYellow);
                }
            }
            else
            {
                if (MorMorAPI.TerrariaShop.TryGetShop(args.Parameters[0], out var shop) && shop != null)
                {
                    var curr = MorMorAPI.CurrencyManager.Query(args.User.GroupID, args.User.Id);
                    if (curr != null && curr.num >= shop.Price)
                    {
                        var res = await args.Server.Command($"/g {shop.ID} {args.Name} {shop.num}");
                        if (res.Status)
                        {
                            MorMorAPI.CurrencyManager.Del(args.User.GroupID, args.User.Id, shop.Price);
                            await args.Server.PrivateMsg("购买成功!", Color.GreenYellow);
                        }
                        else
                        {
                            await args.Server.PrivateMsg("失败! 错误信息:\n" + res.Message, Color.GreenYellow);
                        }
                    }
                    else
                    {
                        await args.Server.PrivateMsg("星币不足!", Color.GreenYellow);
                    }
                }
                else
                {
                    await args.Server.PrivateMsg("该商品不存在!", Color.GreenYellow);
                }
            }
        }
        else
        {
            await args.Server.PrivateMsg("未找到你的注册信息!", Color.GreenYellow);
        }
        //}
        //else
        //{
        //    await args.EventArgs.Reply("服务器不存在或未切换到服务器!", true);
        //}
    }
}
