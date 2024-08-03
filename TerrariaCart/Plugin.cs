using System.Drawing;
using System.Text;
using MomoAPI.Entities;
using MorMor;
using MorMor.Commands;
using MorMor.Configuration;
using MorMor.Extensions;
using MorMor.Permission;
using MorMor.Plugin;
using MorMor.TShock.ChatCommand;

namespace TerrariaCart;

public class Plugin : MorMorPlugin
{
    public override string Name => "TerrariaCart";

    public override string Description => "提供泰拉商店的购物车功能，更加方便的使用商店!";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    public Config Config { get; set; } = new();

    public static readonly string SavePath = Path.Combine(MorMorAPI.SAVE_PATH, "Cart.json");

    public Plugin()
    {
        Config = ConfigHelpr.LoadConfig(SavePath, Config);
    }

    public override void Initialize()
    {
        CommandManager.Hook.Add(new("cart", CartManager, OneBotPermissions.TerrariaShop));
        ChatCommandMananger.Hook.Add(new("结算", CartBuy, OneBotPermissions.TerrariaShop));
    }

    public async Task CartBuy(PlayerCommandArgs args)
    {
        if (args.Server == null) return;
        if (args.Parameters.Count != 1)
        {
            await args.Server.PrivateMsg(args.Name, $"语法错误:\n正确语法:/结算 [购物车]", Color.GreenYellow);
            return;
        }
        if (!args.Server.EnabledShop)
        {
            await args.Server.PrivateMsg(args.Name, "服务器未开启商店系统！", Color.DarkRed);
            return;
        }
        if (args.User != null)
        {
            try
            {
                var carts = Config.GetCartShop(args.Account.UserId, args.Parameters[0]);
                if (carts.Count == 0)
                {
                    await args.Server.PrivateMsg(args.Name, "购物车中不存在物品!", Color.DarkRed);
                    return;
                }
                var all = carts.Sum(x => x.Price);
                var curr = MorMorAPI.CurrencyManager.Query(args.User.GroupID, args.User.Id);
                if (curr != null && curr.num >= all)
                {
                    foreach (var shop in carts)
                    {
                        var res = await args.Server.Command($"/g {shop.ID} {args.Name} {shop.num}");
                    }
                    await args.Server.PrivateMsg(args.Name, "结算成功!", Color.GreenYellow);
                }
                else
                {
                    await args.Server.PrivateMsg(args.Name, "星币不足!", Color.GreenYellow);
                }
            }
            catch (Exception e)
            {
                await args.Server.PrivateMsg(args.Name, e.Message, Color.DarkRed);
                return;
            }

        }
    }

    private async Task CartManager(CommandArgs args)
    {
        try
        {
            if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "add")
            {
                if (int.TryParse(args.Parameters[2], out int id))
                {
                    Config.Add(args.EventArgs.SenderInfo.UserId, args.Parameters[1], id);
                    await args.EventArgs.Reply("添加成功!", true);
                }
                else
                {
                    await args.EventArgs.Reply("请填写一个正确的商品ID!", true);
                }
            }
            else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "del")
            {
                if (int.TryParse(args.Parameters[2], out int id))
                {
                    Config.Remove(args.EventArgs.SenderInfo.UserId, args.Parameters[1], id);
                    await args.EventArgs.Reply("删除成功!", true);
                }
                else
                {
                    await args.EventArgs.Reply("请填写一个正确的商品ID!", true);
                }
            }
            else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "clear")
            {
                Config.ClearCart(args.EventArgs.Sender.Id, args.Parameters[1]);
                await args.EventArgs.Reply("已清除购物车" + args.Parameters[1]);
            }
            else if (args.Parameters.Count == 1 && args.Parameters[0].ToLower() == "list")
            {
                var carts = Config.GetCarts(args.EventArgs.Sender.Id);
                if (carts.Count == 0)
                {
                    await args.EventArgs.Reply("购物车空空如也!", true);
                    return;
                }
                var sb = new StringBuilder();
                sb.AppendLine($$"""<div align="center">""");
                sb.AppendLine();
                sb.AppendLine();
                foreach (var (name, shops) in carts)
                {
                    sb.AppendLine();
                    sb.AppendLine($"# 购物车`{name}`");
                    sb.AppendLine();
                    sb.AppendLine("|商品ID|商品名称|数量|价格|");
                    sb.AppendLine("|:--:|:--:|:--:|:--:|");
                    foreach (var index in shops)
                    {
                        var shop = MorMorAPI.TerrariaShop.GetShop(index);
                        if (shop != null)
                            sb.AppendLine($"|{index}|{shop.Name}|{shop.num}|{shop.Price}|");
                    }
                }
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("</div>");

                await args.EventArgs.Reply(new MessageBody().MarkdownImage(sb.ToString()));
            }
            else
            {
                await args.EventArgs.Reply("语法错误,正确语法\n" +
                    $"{args.CommamdPrefix}{args.Name} add [购物车] [商品ID]\n" +
                    $"{args.CommamdPrefix}{args.Name} del [购物车] [商品ID]\n" +
                    $"{args.CommamdPrefix}{args.Name} clear [购物车]\n" +
                    $"{args.CommamdPrefix}{args.Name} list");
            }

        }
        catch (Exception e)
        {
            await args.EventArgs.Reply(e.Message);
        }
        ConfigHelpr.Write(SavePath, Config);
    }

    protected override void Dispose(bool dispose)
    {
        CommandManager.Hook.commands.RemoveAll(x => x.CallBack == CartManager);
        ChatCommandMananger.Hook.commands.RemoveAll(x => x.CallBack == CartBuy);
    }
}
