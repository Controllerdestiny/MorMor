using MorMor.Attributes;
using MorMor.Permission;
using System.Drawing;

namespace MorMor.TShock.ChatCommand;

[CommandSeries]
public class ChatServerCommand
{
    [CommandMatch("抽", OneBotPermissions.TerrariaPrize)]
    public static async Task Prize(PlayerCommandArgs args)
    {
        if (args.Server == null) return;
        if (args.User == null)
        {
            await args.Server.PrivateMsg(args.Name, "没有你的注册信息！", Color.DarkRed);
            return;
        }
        if (!args.Server.EnabledPrize)
        {
            await args.Server.PrivateMsg(args.Name, "服务器未开启抽奖系统！", Color.DarkRed);
            return;
        }
        var count = 1;
        if (args.Parameters.Count > 0)
            _ = int.TryParse(args.Parameters[0], out count);
        if (count > 50)
            count = 50;
        var prizes = MorMorAPI.TerrariaPrize.Nexts(count);
        var curr = MorMorAPI.CurrencyManager.Query(args.User.GroupID, args.User.Id);
        if (curr == null || curr.num < count * MorMorAPI.TerrariaPrize.Fess)
        {
            await args.Server.PrivateMsg(args.Name, $"你的星币不足抽取{count}次", Color.Red);
            return;
        }
        MorMorAPI.CurrencyManager.Del(args.User.GroupID, args.User.Id, count * MorMorAPI.TerrariaPrize.Fess);
        Random random = new();
        var tasks = new List<Task>();
        foreach (var prize in prizes)
        {
            tasks.Add(args.Server.Command($"/g {prize.ID} {args.Name} {random.Next(prize.Min, prize.Max)}"));
        }
        await Task.WhenAll(tasks);
    }


    [CommandMatch("购买", OneBotPermissions.TerrariaShop)]
    public static async Task ShopBuy(PlayerCommandArgs args)
    {
        if (args.Server == null) return;
        if (args.Parameters.Count != 1)
        {
            await args.Server.PrivateMsg(args.Name, $"语法错误:\n正确语法:/购买 [名称|ID]", Color.GreenYellow);
            return;
        }
        if (!args.Server.EnabledShop)
        {
            await args.Server.PrivateMsg(args.Name, "服务器未开启商店系统！", Color.DarkRed);
            return;
        }
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
                            await args.Server.PrivateMsg(args.Name, "购买成功!", Color.GreenYellow);
                        }
                        else
                        {
                            await args.Server.PrivateMsg(args.Name, "失败! 错误信息:\n" + res.Message, Color.GreenYellow);
                        }
                    }
                    else
                    {
                        await args.Server.PrivateMsg(args.Name, "星币不足!", Color.GreenYellow);
                    }
                }
                else
                {
                    await args.Server.PrivateMsg(args.Name, "该商品不存在!", Color.GreenYellow);
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
                            await args.Server.PrivateMsg(args.Name, "购买成功!", Color.GreenYellow);
                        }
                        else
                        {
                            await args.Server.PrivateMsg(args.Name, "失败! 错误信息:\n" + res.Message, Color.GreenYellow);
                        }
                    }
                    else
                    {
                        await args.Server.PrivateMsg(args.Name, "星币不足!", Color.GreenYellow);
                    }
                }
                else
                {
                    await args.Server.PrivateMsg(args.Name, "该商品不存在!", Color.GreenYellow);
                }
            }
        }
        else
        {
            await args.Server.PrivateMsg(args.Name, "未找到你的注册信息!", Color.GreenYellow);
        }
        //}
        //else
        //{
        //    await args.EventArgs.Reply("服务器不存在或未切换到服务器!", true);
        //}
    }
}
