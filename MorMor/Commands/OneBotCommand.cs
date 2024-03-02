using MomoAPI.Entities;
using MomoAPI.Entities.Segment;
using MorMor.Attributes;
using MorMor.Configuration;
using MorMor.Event;
using MorMor.EventArgs;
using MorMor.Exceptions;
using MorMor.Permission;
using MorMor.Utils;
using Newtonsoft.Json.Linq;

namespace MorMor.Commands;

internal class OneBotCommand
{
    #region 帮助
    [CommandMatch("help", Permissions.Help)]
    public async Task Help(CommandArgs args)
    {
        void Show(List<string> line)
        {
            if (PaginationTools.TryParsePageNumber(args.Parameters, 0, args.EventArgs, out int page)) ;
            {
                PaginationTools.SendPage(args.EventArgs, page, line, new PaginationTools.Settings()
                {
                    MaxLinesPerPage = 10,
                    NothingToDisplayString = "当前没有指令可用",
                    HeaderFormat = "指令列表 ({0}/{1})：",
                    FooterFormat = $"输入 {args.CommamdPrefix}help {{0}} 查看更多"
                });
            }
        }
        var commands = CommandManager.Hook.commands.Select(x => args.CommamdPrefix + x.Name.First()).ToList();
        Show(commands);
    }
    #endregion

    #region 签到
    [CommandMatch("签到", Permissions.Sign)]
    private async Task Sign(CommandArgs args)
    {
        try
        {
            var rand = new Random();
            long num = rand.NextInt64(500, 700);
            var result = MorMorAPI.SignManager.SingIn(args.EventArgs.Group.Id, args.EventArgs.Sender.Id);
            var currency = MorMorAPI.CurrencyManager.Add(args.EventArgs.Group.Id, args.EventArgs.Sender.Id, num);
            MessageBody body = new()
            {
                MomoSegment.Image($"http://q.qlogo.cn/headimg_dl?dst_uin={args.EventArgs.Sender.Id}&spec=640&img_type=png"),
                MomoSegment.Text($"签到成功！\n"),
                MomoSegment.Text($"[签到时长]：{result.Date}\n"),
                MomoSegment.Text($"[获得星币]：{num}\n"),
                MomoSegment.Text($"[星币总数]：{currency.num}")
            };
            await args.EventArgs.Reply(body);
        }
        catch(Exception e)
        {
            await args.EventArgs.Reply(e.Message);
        }
    }
    #endregion

    #region 重读
    [CommandMatch("reload", Permissions.Reload)]
    private async Task Reload(CommandArgs args)
    {
        var reloadArgs = new ReloadEventArgs();
        MorMorAPI.Setting = Config.LoadConfig<MorMorSetting>(MorMorAPI.ConfigPath);
        reloadArgs.Message.Add("沫沫配置文件重读成功!");
        await OperatHandler.Reload(reloadArgs);
        await args.EventArgs.Reply(reloadArgs.Message);
    }
    #endregion

    #region 账户管理
    [CommandMatch("account", Permissions.Account)]
    private async Task Account(CommandArgs args)
    {
        void Show(List<string> line)
        {
            if (PaginationTools.TryParsePageNumber(args.Parameters, 1, args.EventArgs, out int page)) ;
            {
                PaginationTools.SendPage(args.EventArgs, page, line, new PaginationTools.Settings()
                {
                    MaxLinesPerPage = 6,
                    NothingToDisplayString = "当前没有账户",
                    HeaderFormat = "账户列表 ({0}/{1})：",
                    FooterFormat = $"输入 {args.CommamdPrefix}account list {{0}} 查看更多"
                });
            }
        }
        var atList = args.EventArgs.MessageContext.GetAts();
        if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "add" && atList.Count == 1)
        {
            try
            {
                MorMorAPI.AccountManager.AddAccount(atList.First().UserId, args.EventArgs.Group.Id, args.Parameters[1]);
                await args.EventArgs.Reply($"{atList.First().UserId} 已添加到组 {args.Parameters[1]}");
            }
            catch (Exception ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }
        }
        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "add")
        {
            if (long.TryParse(args.Parameters[1], out long id))
            {
                try
                {
                    MorMorAPI.AccountManager.AddAccount(id, args.EventArgs.Group.Id, args.Parameters[2]);
                    await args.EventArgs.Reply($"{id} 已添加到组 {args.Parameters[2]}");
                }
                catch (Exception ex)
                {
                    await args.EventArgs.Reply(ex.Message);
                }
            }
            else
            {
                await args.EventArgs.Reply("错误的QQ账号，无法转换!");
            }
        }
        else if (args.Parameters.Count == 1 && args.Parameters[0].ToLower() == "del" && args.EventArgs.MessageContext.GetAts().Count == 1)
        {
            try
            {
                MorMorAPI.AccountManager.RemoveAccount(atList.First().UserId, args.EventArgs.Group.Id);
                await args.EventArgs.Reply($"删除成功!");
            }
            catch (Exception ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }
        }
        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "del")
        {
            if (long.TryParse(args.Parameters[1], out long id))
            {
                try
                {
                    MorMorAPI.AccountManager.RemoveAccount(id, args.EventArgs.Group.Id);
                    await args.EventArgs.Reply($"删除成功!");
                }
                catch (Exception ex)
                {
                    await args.EventArgs.Reply(ex.Message);
                }
            }
            else
            {
                await args.EventArgs.Reply("错误的QQ账号，无法转换!");
            }
        }
        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "group" && atList.Count == 1)
        {
            try
            {
                MorMorAPI.AccountManager.ReAccountGroup(atList.First().UserId, args.EventArgs.Group.Id, args.Parameters[1]);
                await args.EventArgs.Reply($"账户 {atList.First().UserId} 的组 成功更改为 {args.Parameters[1]}");
            }
            catch (Exception ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }
        }
        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "group")
        {
            if (long.TryParse(args.Parameters[1], out long id))
            {
                try
                {
                    MorMorAPI.AccountManager.ReAccountGroup(id, args.EventArgs.Group.Id, args.Parameters[1]);
                    await args.EventArgs.Reply($"账户 {id} 的组 成功更改为 {args.Parameters[1]}");
                }
                catch (Exception ex)
                {
                    await args.EventArgs.Reply(ex.Message);
                }
            }
            else
            {
                await args.EventArgs.Reply("错误的QQ账号，无法转换!");
            }
        }
        else if (args.Parameters.Count >= 1 && args.Parameters[0].ToLower() == "list")
        {
            try
            {
                var accounts = MorMorAPI.AccountManager.GetAccounts(args.EventArgs.Group.Id);
                var lines = accounts.Select(x => $"\n账户:{x.UserId}\n权限:{x.Group.GroupName}");
                Show(lines.ToList());
            }
            catch (AccountException ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }
        }
        else
        {
            MessageBody body = new()
            {
                "语法错误，正确的语法:\n",
                MomoSegment.Text($"{args.CommamdPrefix}account add <组> at\n"),
                MomoSegment.Text($"{args.CommamdPrefix}account del <组> at\n"),
                MomoSegment.Text($"{args.CommamdPrefix}account add <QQ> <组>\n"),
                MomoSegment.Text($"{args.CommamdPrefix}account del <QQ> <组>\n"),
                MomoSegment.Text($"{args.CommamdPrefix}account group <组> at\n"),
                MomoSegment.Text($"{args.CommamdPrefix}account group <QQ> <组>\n"),
                MomoSegment.Text($"{args.CommamdPrefix}account list")
            };
            await args.EventArgs.Reply(body);
        }
    }
    #endregion

    #region 权限组管理
    [CommandMatch("group", Permissions.Group)]
    private async Task Group(CommandArgs args)
    {
        if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "add")
        {
            try
            {
                MorMorAPI.GroupManager.AddGroup(args.EventArgs.Group.Id, args.Parameters[1]);
                await args.EventArgs.Reply($"组 {args.Parameters[1]} 添加成功!");
            }
            catch (GroupException ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }
        }
        //删除组
        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "del")
        {
            try
            {
                MorMorAPI.GroupManager.RemoveGroup(args.EventArgs.Group.Id, args.Parameters[1]);
                await args.EventArgs.Reply($"组 {args.Parameters[1]} 删除成功!");
            }
            catch (GroupException ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }
        }
        //更改父组
        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "parent")
        {
            try
            {
                MorMorAPI.GroupManager.ReParentGroup(args.EventArgs.Group.Id, args.Parameters[1], args.Parameters[2]);
                await args.EventArgs.Reply($"组 {args.Parameters[1]} 的组已更改为 {args.Parameters[2]}!");
            }
            catch (Exception ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }
        }
        //添加权限
        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "addperm")
        {
            try
            {

                MorMorAPI.GroupManager.AddPerm(args.EventArgs.Group.Id, args.Parameters[1], args.Parameters[2]);
                await args.EventArgs.Reply($"权限添加成功!");
            }
            catch (GroupException ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }
        }
        //删除权限
        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "delperm")
        {
            try
            {
                MorMorAPI.GroupManager.RemovePerm(args.EventArgs.Group.Id, args.Parameters[1], args.Parameters[2]);
                await args.EventArgs.Reply($"权限删除成功!");
            }
            catch (GroupException ex)
            {
                args.EventArgs.Reply(ex.Message);
            }
        }
        else
        {
            MessageBody body = new()
            {
                "语法错误，正确语法:\n",
                MomoSegment.Text($"{args.CommamdPrefix}group add <组>\n"),
                MomoSegment.Text($"{args.CommamdPrefix}group del <组>\n"),
                MomoSegment.Text($"{args.CommamdPrefix}group addperm <组> <权限>\n"),
                MomoSegment.Text($"{args.CommamdPrefix}group delperm <组> <权限>\n"),
                MomoSegment.Text($"{args.CommamdPrefix}group parent <组> <父组>")
            };
            await args.EventArgs.Reply(body);
        }
    }
    #endregion

    #region 星币管理
    [CommandMatch("星币", Permissions.CurrencyUse, Permissions.CurrencyAdmin)]
    private async Task Currency(CommandArgs args)
    {
        var at = args.EventArgs.MessageContext.GetAts();
        if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "add")
        {
            if (!args.Account.HasPermission(Permissions.CurrencyAdmin))
            {
                await args.EventArgs.Reply("你没有权限执行此命令!");
                return;
            }
            if (!long.TryParse(args.Parameters[1], out long qq))
            {
                await args.EventArgs.Reply("错误得QQ账号，无法转换为数值!");
                return;
            }

            if (!long.TryParse(args.Parameters[2], out long num))
            {
                await args.EventArgs.Reply("错误得数量，无法转换为数值!");
                return;
            }
            try
            {
                var result = MorMorAPI.CurrencyManager.Add(args.EventArgs.Group.Id, qq, num);
                await args.EventArgs.Reply($"成功为 {qq} 添加{num}个星币!");
            }
            catch (Exception ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }

        }
        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "add" && at.Count() == 1)
        {
            if (!args.Account.HasPermission(Permissions.CurrencyAdmin))
            {
                await args.EventArgs.Reply("你没有权限执行此命令!");
                return;
            }
            if (!long.TryParse(args.Parameters[1], out long num))
            {
                await args.EventArgs.Reply("错误得数量，无法转换为数值!");
                return;
            }
            try
            {
                var result = MorMorAPI.CurrencyManager.Add(args.EventArgs.Group.Id, at.First().UserId, num);
                await args.EventArgs.Reply($"成功为 {at.First()} 添加{num}个星币!");
            }
            catch (Exception ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }
        }
        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "del")
        {
            if (!args.Account.HasPermission(Permissions.CurrencyAdmin))
            {
                await args.EventArgs.Reply("你没有权限执行此命令!");
                return;
            }
            if (!long.TryParse(args.Parameters[1], out long qq))
            {
                await args.EventArgs.Reply("错误得QQ账号，无法转换为数值!");
                return;
            }

            if (!long.TryParse(args.Parameters[2], out long num))
            {
                await args.EventArgs.Reply("错误得数量，无法转换为数值!");
                return;
            }
            try
            {
                var result = MorMorAPI.CurrencyManager.Del(args.EventArgs.Group.Id, qq, num);
                await args.EventArgs.Reply($"成功删除 {qq} 的 {num}个星币!");
            }
            catch (Exception ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }

        }
        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "del" && at.Count() == 1)
        {
            if (!args.Account.HasPermission(Permissions.CurrencyAdmin))
            {
                await args.EventArgs.Reply("你没有权限执行此命令!");
                return;
            }
            if (!long.TryParse(args.Parameters[1], out long num))
            {
                await args.EventArgs.Reply("错误得数量，无法转换为数值!");
                return;
            }
            try
            {
                var result = MorMorAPI.CurrencyManager.Del(args.EventArgs.Group.Id, at.First().UserId, num);
                await args.EventArgs.Reply($"成功扣除 {at.First()} 的 {num}个星币!");
            }
            catch (Exception ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }
        }
        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "pay")
        {
            if (!long.TryParse(args.Parameters[1], out long qq))
            {
                await args.EventArgs.Reply("错误得QQ账号，无法转换为数值!");
                return;
            }

            if (!long.TryParse(args.Parameters[2], out long num))
            {
                await args.EventArgs.Reply("错误得数量，无法转换为数值!");
                return;
            }
            var usercurr = MorMorAPI.CurrencyManager.Query(args.EventArgs.Group.Id, args.EventArgs.Sender.Id);
            if (usercurr == null || usercurr.num < num)
            {
                await args.EventArgs.Reply("你没有足够的星币付给他人!");
            }
            else
            {
                try
                {
                    MorMorAPI.CurrencyManager.Del(args.EventArgs.Group.Id, args.EventArgs.Sender.Id, num);
                    MorMorAPI.CurrencyManager.Add(args.EventArgs.Group.Id, qq, num);
                    await args.EventArgs.Reply($"成功付给 {qq}  {num}个星币!");
                }
                catch (Exception ex)
                {
                    await args.EventArgs.Reply(ex.Message);
                }
            }
        }
        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "pay" && at.Count() == 1)
        {
            if (!long.TryParse(args.Parameters[1], out long num))
            {
                await args.EventArgs.Reply("错误得数量，无法转换为数值!");
                return;
            }
            var usercurr = MorMorAPI.CurrencyManager.Query(args.EventArgs.Group.Id, args.EventArgs.Sender.Id);
            if (usercurr == null || usercurr.num < num)
            {
                await args.EventArgs.Reply("你没有足够的星币付给他人!");
            }
            else
            {
                try
                {
                    MorMorAPI.CurrencyManager.Del(args.EventArgs.Group.Id, args.EventArgs.Sender.Id, num);
                    MorMorAPI.CurrencyManager.Add(args.EventArgs.Group.Id, at.First().UserId, num);
                    await args.EventArgs.Reply($"成功付给 {at.First()}  {num}个星币!");
                }
                catch (Exception ex)
                {
                    await args.EventArgs.Reply(ex.Message);
                }
            }
        }
        else
        {
            await args.EventArgs.Reply("语法错误，正确语法:\n" +
                $"{args.CommamdPrefix}星币 add <qq> <数量>\n" +
                $"{args.CommamdPrefix}星币 add <数量> at\n" +
                $"{args.CommamdPrefix}星币 del <qq> <数量>\n" +
                $"{args.CommamdPrefix}星币 del <数量> at\n" +
                $"{args.CommamdPrefix}星币 pay <qq> 数量\n" +
                $"{args.CommamdPrefix}星币 pay <数量> at");
        }
    }
    #endregion

    #region 查询指令权限
    [CommandMatch("scmdperm", Permissions.SearchCommandPerm)]
    private async Task CmdBan(CommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            var banName = args.Parameters[0];
            var comm = CommandManager.Hook.commands.Where(x => x.Name.Contains(banName)).SelectMany(x => x.Permission).ToList();
            if (comm == null || comm.Count == 0)
            {
                await args.EventArgs.Reply("没有找到该指令，无法查询！");
            }
            else
            {
                MessageBody body = new()
                {
                    banName + "指令的权限可能为:\n"
                };
                comm.ForEach(x => body.Add(x));
                await args.EventArgs.Reply(body);
            }
        }
        else
        {
            await args.EventArgs.Reply($"语法错误，正确语法:{args.CommamdPrefix}scmdperm [指令名]");
        }
    }
    #endregion

    #region 缩写查询
    [CommandMatch("缩写", Permissions.Nbnhhsh)]
    private async Task Nbnhhsh(CommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            var url = $"https://oiapi.net/API/Nbnhhsh?text={args.Parameters[0]}";
            HttpClient client = new();
            var result = await client.GetStringAsync(url);
            var data = JObject.Parse(result);
            var trans = data["data"]?[0]?["trans"];
            if (trans != null && trans.Any())
            {
                
                await args.EventArgs.Reply($"缩写:`{args.Parameters[0]}`可能为:\n{string.Join(",", trans)}");
            }
            else
            {
                await args.EventArgs.Reply("也许该缩写没有被收录!");
            }
        }
        else
        {
            await args.EventArgs.Reply($"语法错误，正确语法:{args.CommamdPrefix}缩写 [文本]");
        }
    }
    #endregion

    #region 禁言
    [CommandMatch("禁",Permissions.Mute)]
    private async Task Mute(CommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            if (!int.TryParse(args.Parameters[0].ToString(), out var muted))
            {
                await args.EventArgs.Reply("请输入正确的禁言时长!");
                return;
            }
            var atlist = args.EventArgs.MessageContext.GetAts();
            if (atlist.Count == 0)
            {
                await args.EventArgs.Reply("未指令目标成员!");
                return;
            }
            atlist.ForEach(async x => await args.EventArgs.Group.Mute(x.UserId, muted * 60));
            await args.EventArgs.Reply("禁言成功！");
        }
        else
        {
            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}禁 [AT] [时长]！");
        }
    }
    #endregion

    #region 解禁
    [CommandMatch("解", Permissions.Mute)]
    private async Task UnMute(CommandArgs args)
    {
        if (args.Parameters.Count == 0)
        {
            var atlist = args.EventArgs.MessageContext.GetAts();
            if (atlist.Count == 0)
            {
                await args.EventArgs.Reply("未指令目标成员!");
                return;
            }
            atlist.ForEach(async x => await args.EventArgs.Group.Mute(x.UserId, 0));
            await args.EventArgs.Reply("解禁成功！");
        }
        else
        {
            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}解 [AT] [时长]！");
        }
    }
    #endregion

    #region 全体禁言
    [CommandMatch("全禁", Permissions.Mute)]
    private async Task MuteAll(CommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            switch (args.Parameters[0])
            {
                case "开启":
                case "开":
                case "true":
                    await args.EventArgs.Group.MuteAll(true);
                    await args.EventArgs.Reply("开启成功！");
                    break;
                case "关闭":
                case "关":
                case "false":
                    await args.EventArgs.Group.MuteAll(false);
                    await args.EventArgs.Reply("关闭成功");
                    break;
                default:
                    await args.EventArgs.Reply("语法错误,正确语法:\n全禁 [开启|关闭]");
                    break;
            }
        }
        else
        {
            await args.EventArgs.Reply("语法错误,正确语法:\n全禁 [开启|关闭]");
        }
    }
    #endregion

    #region 设置群名
    [CommandMatch("设置群名", Permissions.ChangeGroupOption)]
    private async Task SetGroupName(CommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            if (string.IsNullOrEmpty(args.Parameters[0]))
            {
                await args.EventArgs.Reply("群名不能未空！");
                return;
            }
            await args.EventArgs.Group.SetName(args.Parameters[0]);
            await args.EventArgs.Reply($"群名称已修改为`{args.Parameters[0]}`");
        }
        else
        {
            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}设置群名 [名称]");
        }
    }
    #endregion

    #region 设置群成员名片
    [CommandMatch("设置昵称", Permissions.ChangeGroupOption)]
    private async Task SetGroupMemeberNick(CommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            var atlist = args.EventArgs.MessageContext.GetAts().First();
            if (atlist != null)
            {
                await args.EventArgs.Group.SetMemberCard(atlist.UserId, args.Parameters[0]);
                await args.EventArgs.Reply("修改昵称成功!");
            }
            else
            {
                await args.EventArgs.Reply("请选择一位成员！");
            }
        }
        else
        {
            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [名称]");
        }
    }
    #endregion

    #region 设置管理
    [CommandMatch("设置管理", Permissions.ChangeGroupOption)]
    private async Task SetGroupAdmin(CommandArgs args)
    {
        if (args.Parameters.Count == 0)
        {
            var atlist = args.EventArgs.MessageContext.GetAts().First();
            if (atlist != null)
            {
                await args.EventArgs.Group.SetAdmin(atlist.UserId, true);
                await args.EventArgs.Reply($"已将`{atlist.UserId}`设置为管理员!");
            }
            else
            {
                await args.EventArgs.Reply("请选择一位成员！");
            }
        }
        else
        {
            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [AT]");
        }
    }
    #endregion

    #region 取消管理
    [CommandMatch("取消管理", Permissions.ChangeGroupOption)]
    private async Task UnsetGroupAdmin(CommandArgs args)
    {
        if (args.Parameters.Count == 0)
        {
            var atlist = args.EventArgs.MessageContext.GetAts().First();
            if (atlist != null)
            {
                await args.EventArgs.Group.SetAdmin(atlist.UserId, false);
                await args.EventArgs.Reply($"已取消`{atlist.UserId}`的管理员!");
            }
            else
            {
                await args.EventArgs.Reply("请选择一位成员！");
            }
        }
        else
        {
            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [AT]");
        }
    }
    #endregion
}
