using MomoAPI.Entities;
using MomoAPI.Entities.Segment;
using MorMor.Attributes;
using MorMor.Configuration;
using MorMor.Event;
using MorMor.EventArgs;
using MorMor.Exceptions;
using MorMor.Permission;
using MorMor.Picture;
using MorMor.Utils;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MorMor.Commands;

public class OneBotCommand
{
    #region 帮助
    [CommandMatch("help", OneBotPermissions.Help)]
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
    [CommandMatch("签到", OneBotPermissions.Sign)]
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
                MomoSegment.Image(args.EventArgs.SenderInfo.TitleImage),
                MomoSegment.Text($"签到成功！\n"),
                MomoSegment.Text($"[签到时长]：{result.Date}\n"),
                MomoSegment.Text($"[获得星币]：{num}\n"),
                MomoSegment.Text($"[星币总数]：{currency.num}")
            };
            await args.EventArgs.Reply(body);
        }
        catch (Exception e)
        {
            await args.EventArgs.Reply(e.Message);
        }
    }
    #endregion

    #region 重读
    [CommandMatch("reload", OneBotPermissions.Reload)]
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
    [CommandMatch("account", OneBotPermissions.Account)]
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
                MorMorAPI.AccountManager.AddAccount(atList.First().UserId, args.Parameters[1]);
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
                    MorMorAPI.AccountManager.AddAccount(id, args.Parameters[2]);
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
                MorMorAPI.AccountManager.RemoveAccount(atList.First().UserId);
                await args.EventArgs.Reply($"删除成功!");
            }
            catch (Exception ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }
        }
        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "del")
        {
            if (long.TryParse(args.Parameters[1], out long id))
            {
                try
                {
                    MorMorAPI.AccountManager.RemoveAccount(id);
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
                MorMorAPI.AccountManager.ReAccountGroup(atList.First().UserId, args.Parameters[1]);
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
                    MorMorAPI.AccountManager.ReAccountGroup(id, args.Parameters[1]);
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
                var accounts = MorMorAPI.AccountManager.Accounts;
                var lines = accounts.Select(x => $"\n账户:{x.UserId}\n权限:{x.Group.Name}");
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
    [CommandMatch("group", OneBotPermissions.Group)]
    private async Task Group(CommandArgs args)
    {
        if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "add")
        {
            try
            {
                MorMorAPI.GroupManager.AddGroup(args.Parameters[1]);
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
                MorMorAPI.GroupManager.RemoveGroup(args.Parameters[1]);
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
                MorMorAPI.GroupManager.ReParentGroup(args.Parameters[1], args.Parameters[2]);
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
                MorMorAPI.GroupManager.AddPerm(args.Parameters[1], args.Parameters[2]);
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
                MorMorAPI.GroupManager.RemovePerm(args.Parameters[1], args.Parameters[2]);
                await args.EventArgs.Reply($"权限删除成功!");
            }
            catch (GroupException ex)
            {
                await args.EventArgs.Reply(ex.Message);
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
    [CommandMatch("星币", OneBotPermissions.CurrencyUse, OneBotPermissions.CurrencyAdmin)]
    private async Task Currency(CommandArgs args)
    {
        var at = args.EventArgs.MessageContext.GetAts();
        if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "add")
        {
            if (!args.Account.HasPermission(OneBotPermissions.CurrencyAdmin))
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
            if (!args.Account.HasPermission(OneBotPermissions.CurrencyAdmin))
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
            if (!args.Account.HasPermission(OneBotPermissions.CurrencyAdmin))
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
            if (!args.Account.HasPermission(OneBotPermissions.CurrencyAdmin))
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
    [CommandMatch("scmdperm", OneBotPermissions.SearchCommandPerm)]
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
    [CommandMatch("缩写", OneBotPermissions.Nbnhhsh)]
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
    [CommandMatch("禁", OneBotPermissions.Mute)]
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
    [CommandMatch("解", OneBotPermissions.Mute)]
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
    [CommandMatch("全禁", OneBotPermissions.Mute)]
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
    [CommandMatch("设置群名", OneBotPermissions.ChangeGroupOption)]
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
    [CommandMatch("设置昵称", OneBotPermissions.ChangeGroupOption)]
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
    [CommandMatch("设置管理", OneBotPermissions.ChangeGroupOption)]
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
    [CommandMatch("取消管理", OneBotPermissions.ChangeGroupOption)]
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

    #region 服务器列表
    [CommandMatch("服务器列表", OneBotPermissions.ServerList)]
    private async Task ServerList(CommandArgs args)
    {
        if (MorMorAPI.Setting.Servers.Count == 0)
        {
            await args.EventArgs.Reply("服务器列表空空如也!");
            return;
        }
        var sb = new StringBuilder("服务器列表\n");
        foreach (var x in MorMorAPI.Setting.Servers)
        {
            var status = await x.Status();
            sb.Append("[名称]: ");
            sb.AppendLine(x.Name);
            sb.Append("[地址]: ");
            sb.AppendLine(x.IP);
            sb.Append("[端口]: ");
            sb.AppendLine(x.Port.ToString());
            sb.Append("[版本]: ");
            sb.AppendLine(x.Version);
            sb.Append("[状态]: ");
            sb.AppendLine(status.Status != Enumeration.TerrariaApiStatus.DisposeConnect ? $"已运行 {status.UpTime}" : "无法连接");
            sb.Append("[介绍]: ");
            sb.AppendLine(x.Describe);
        }
        await args.EventArgs.Reply(sb.ToString().Trim());
    }
    #endregion

    #region 切换服务器
    [CommandMatch("切换", OneBotPermissions.ChangeServer)]
    private async Task ChangeServer(CommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            var server = MorMorAPI.Setting.GetServer(args.Parameters[0]);
            if (server == null)
            {
                await args.EventArgs.Reply("你切换的服务器不存在!", true);
                return;
            }
            MorMorAPI.UserLocation.Change(args.EventArgs.Sender.Id, server);
            await args.EventArgs.Reply($"已切换至`{server.Name}`服务器", true);
        }
        else
        {
            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [服务器名称]");
        }
    }
    #endregion

    #region 查询在线玩家
    [CommandMatch("在线", OneBotPermissions.QueryOnlienPlayer)]
    private async Task OnlinePlayers(CommandArgs args)
    {
        if (MorMorAPI.Setting.Servers.Count == 0)
        {
            await args.EventArgs.Reply("还没有配置任何一个服务器!", true);
            return;
        }
        var sb = new StringBuilder();
        foreach (var server in MorMorAPI.Setting.Servers)
        {
            var api = await server.PlayerOnline();
            sb.AppendLine($"[{server.Name}]在线玩家数量({(api.IsSuccess ? api.Players.Count : 0)}/{255})");
            sb.AppendLine(api.IsSuccess ? string.Join(",", api.Players) : "无法连接服务器");
        }
        await args.EventArgs.Reply(sb.ToString().Trim());
    }
    #endregion

    #region 生成地图
    [CommandMatch("生成地图", OneBotPermissions.GenerateMap)]
    private async Task GenerateMap(CommandArgs args)
    {
        if (MorMorAPI.UserLocation.TryGetServer(args.EventArgs.Sender.Id, out var server) && server != null)
        {
            var api = await server.GeneareMap();
            var body = new MessageBody();
            if (api.IsSuccess)
            {
                body.Add(MomoSegment.Image($"base64://{api.Uri}"));
            }
            else
            {
                body.Add("无法连接服务器！");
            }
            await args.EventArgs.Reply(body);
        }
        else
        {
            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
        }
    }
    #endregion

    #region 注册
    [CommandMatch("注册", OneBotPermissions.RegisterUser)]
    private async Task Register(CommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            if (args.Parameters[0].Length > 19)
            {
                await args.EventArgs.Reply("注册的人物名称不能大于19个字符!", true);
                return;
            }
            if (!new Regex("^[a-zA-Z\u4E00-\u9FA5]+$").IsMatch(args.Parameters[0]))
            {
                await args.EventArgs.Reply("注册的人物名称不能包含中文以及字母以外的字符", true);
                return;
            }
            if (MorMorAPI.UserLocation.TryGetServer(args.EventArgs.Sender.Id, out var server) && server != null)
            {
                var pass = Guid.NewGuid().ToString()[..8];
                try
                {
                    MorMorAPI.TerrariaUserManager.Add(args.EventArgs.Sender.Id, server.Name, args.Parameters[0], pass);
                    var api = await server.Register(args.Parameters[0], pass);
                    var body = new MessageBody();
                    if (api.IsSuccess)
                    {
                        MailHelper.SendMail($"{args.EventArgs.Sender.Id}@qq.com",
                            $"{server.Name}服务器注册密码",
                            $"您的注册密码是:{pass}<br>请注意保存不要暴露给他人");
                        body.Add($"注册成功!" +
                            $"\n注册服务器: {server.Name}" +
                            $"\n注册名称: {args.Parameters[0]}" +
                            $"\n注册账号: {args.EventArgs.Sender.Id}" +
                            $"\n注册人昵称: {args.EventArgs.SenderInfo.Name}" +
                            $"\n注册密码已发送至QQ邮箱请点击下方链接查看" +
                            $"\nhttps://wap.mail.qq.com/home/index");
                    }
                    else
                    {
                        MorMorAPI.TerrariaUserManager.Remove(server.Name, args.Parameters[0]);
                        body.Add(string.IsNullOrEmpty(api.ErrorMessage) ? "无法连接服务器！" : api.ErrorMessage);
                    }
                    await args.EventArgs.Reply(body);
                }
                catch (AccountException ex)
                {
                    await args.EventArgs.Reply(ex.Message);
                }

            }
            else
            {
                await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
            }
        }
        else
        {
            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [名称]");
        }
    }
    #endregion

    #region 注册列表
    [CommandMatch("注册列表", OneBotPermissions.QueryUserList)]
    private async Task RegisterList(CommandArgs args)
    {
        if (MorMorAPI.UserLocation.TryGetServer(args.EventArgs.Sender.Id, out var server) && server != null)
        {
            var users = MorMorAPI.TerrariaUserManager.GetUsers(server.Name);
            if (users == null || users.Count == 0)
            {
                await args.EventArgs.Reply("注册列表空空如也!");
                return;
            }
            var sb = new StringBuilder($"[{server.Name}]注册列表\n");
            foreach (var user in users)
            {
                sb.AppendLine($"{user.Name} => {user.Id}");
            }
            await args.EventArgs.Reply(sb.ToString().Trim());
        }
        else
        {
            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
        }
    }
    #endregion

    #region user管理
    [CommandMatch("user", OneBotPermissions.UserAdmin)]
    private async Task User(CommandArgs args)
    {
        if (MorMorAPI.UserLocation.TryGetServer(args.EventArgs.Sender.Id, out var server) && server != null)
        {
            if (args.Parameters.Count == 2)
            {
                switch (args.Parameters[0].ToLower())
                {
                    case "del":
                        try
                        {
                            MorMorAPI.TerrariaUserManager.Remove(server.Name, args.Parameters[1]);
                            await args.EventArgs.Reply("移除成功!", true);
                        }
                        catch (TerrariaUserException ex)
                        {
                            await args.EventArgs.Reply(ex.Message);
                        }
                        break;
                    default:
                        await args.EventArgs.Reply("未知子命令!");
                        break;
                }
            }
        }
        else
        {
            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
        }

    }
    #endregion

    #region 进度查询
    [CommandMatch("进度查询", OneBotPermissions.QueryProgress)]
    private async Task GameProgress(CommandArgs args)
    {
        if (MorMorAPI.UserLocation.TryGetServer(args.EventArgs.Sender.Id, out var server) && server != null)
        {
            var api = await server.Progress();
            var body = new MessageBody();
            if (api.IsSuccess)
            {
                body.Add(MomoSegment.Image(ProgressImage.DrawImg(api.Progress, server.Name)));
            }
            else
            {
                body.Add("无法获取服务器信息！");
            }
            await args.EventArgs.Reply(body);
        }
        else
        {
            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
        }
    }
    #endregion

    #region 查询背包
    [CommandMatch("查背包", OneBotPermissions.QueryInventory)]
    private async Task QueryInventory(CommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            if (MorMorAPI.UserLocation.TryGetServer(args.EventArgs.Sender.Id, out var server) && server != null)
            {
                var api = await server.QueryInventory(args.Parameters[0]);
                var body = new MessageBody();
                if (api.IsSuccess)
                {
                    body.Add(MomoSegment.Image(new InventoryImage().DrawImg(api.PlayerinventoryInfo, args.Parameters[0], server.Name)));
                }
                else
                {
                    body.Add("无法获取用户信息！");
                }
                await args.EventArgs.Reply(body);
            }
            else
            {
                await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
            }
        }
        else
        {
            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [用户名]");
        }

    }
    #endregion

    #region 执行命令
    [CommandMatch("执行", OneBotPermissions.ExecuteCommand)]
    private async Task ExecuteCommand(CommandArgs args)
    {
        if (args.Parameters.Count < 1)
        {
            await args.EventArgs.Reply("请输入要执行的命令!", true);
            return;
        }
        if (MorMorAPI.UserLocation.TryGetServer(args.EventArgs.Sender.Id, out var server) && server != null)
        {
            var cmd = "/" + string.Join(" ", args.Parameters);
            var api = await server.ExecCommamd(cmd);
            var body = new MessageBody();
            if (api.IsSuccess)
            {
                var cmdResult = $"[{server.Name}]命令执行结果:\n{string.Join("\n", api.Response)}";
                body.Add(cmdResult);
            }
            else
            {
                body.Add("无法连接到服务器！");
            }
            await args.EventArgs.Reply(body);
        }
        else
        {
            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
        }
    }
    #endregion

    #region 在线排行
    [CommandMatch("在线排行", OneBotPermissions.OnlineRank)]
    private async Task OnlineRank(CommandArgs args)
    {
        if (MorMorAPI.UserLocation.TryGetServer(args.EventArgs.Sender.Id, out var server) && server != null)
        {
            var api = await server.QueryOnlines();
            var body = new MessageBody();
            if (api.IsSuccess)
            {
                if (api.Rank.Count == 0)
                {
                    await args.EventArgs.Reply("当前还没有数据记录", true);
                    return;
                }
                var sb = new StringBuilder($"[{server.Name}]在线排行:\n");
                var rank = api.Rank.OrderByDescending(x => x.Duration);
                foreach (var duration in rank)
                {
                    var day = duration.Duration / (60 * 60 * 24);
                    var hour = (duration.Duration - day * 60 * 60 * 24) / (60 * 60);
                    var minute = (duration.Duration - day * 60 * 60 * 24 - hour * 60 * 60) / 60;
                    var second = duration.Duration - day * 60 * 60 * 24 - hour * 60 * 60 - minute * 60;
                    sb.Append($"[{duration.Name}]在线时长: ");
                    if (day > 0)
                        sb.Append($"{day}天");
                    if (hour > 0)
                        sb.Append($"{hour}时");
                    if (minute > 0)
                        sb.Append($"{minute}分");
                    sb.Append($"{second}秒\n");
                }
                body.Add(sb.ToString().Trim());
            }
            else
            {
                body.Add("无法连接到服务器！");
            }
            await args.EventArgs.Reply(body);
        }
        else
        {
            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
        }
    }
    #endregion

    #region 死亡排行
    [CommandMatch("死亡排行", OneBotPermissions.DeathRank)]
    private async Task DeathRank(CommandArgs args)
    {
        if (MorMorAPI.UserLocation.TryGetServer(args.EventArgs.Sender.Id, out var server) && server != null)
        {
            var api = await server.DeatRank();
            var body = new MessageBody();
            if (api.IsSuccess)
            {
                if (api.Rank.Count == 0)
                {
                    await args.EventArgs.Reply("当前还没有数据记录", true);
                    return;
                }
                var sb = new StringBuilder($"[{server.Name}]死亡排行:\n");
                var rank = api.Rank.OrderByDescending(x => x.Count);
                foreach (var deathInfo in rank)
                {
                    sb.AppendLine($"[{deathInfo.Name}]死亡次数: {deathInfo.Count}");
                }
                body.Add(sb.ToString().Trim());
            }
            else
            {
                body.Add("无法连接到服务器！");
            }
            await args.EventArgs.Reply(body);
        }
        else
        {
            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
        }
    }
    #endregion

    #region 消息转发
    [CommandMatch("消息转发", OneBotPermissions.ForwardMsg)]
    private async Task MessageForward(CommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            if (MorMorAPI.UserLocation.TryGetServer(args.EventArgs.Sender.Id, out var server) && server != null)
            {
                switch (args.Parameters[0])
                {
                    case "开启":
                    case "true":
                        server.ForwardGroups.Add(args.EventArgs.Group.Id);
                        await args.EventArgs.Reply("开启成功", true);
                        break;
                    case "关闭":
                    case "false":
                        server.ForwardGroups.Remove(args.EventArgs.Group.Id);
                        await args.EventArgs.Reply("关闭成功", true);
                        break;
                    default:
                        await args.EventArgs.Reply("未知子命令！", true);
                        break;
                }
                Config.Write(MorMorAPI.ConfigPath, MorMorAPI.Setting);
            }
            else
            {
                await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
            }
        }
        else
        {
            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [开启|关闭]!", true);
        }
    }
    #endregion

    #region 我的信息
    [CommandMatch("我的信息", OneBotPermissions.SelfInfo)]
    private async Task SelfInfo(CommandArgs args)
    {
        var userid = args.EventArgs.SenderInfo.UserId;
        var serverName = MorMorAPI.UserLocation.TryGetServer(userid,out var server) ? server?.Name ?? "NULL" : "NULL";
        var group = args.Account.Group.Name;
        var bindUser = MorMorAPI.TerrariaUserManager.GetUserById(userid, serverName);
        var bindName = bindUser == null ? "NULL" : bindUser.Name;
        var api = await server?.QueryEconomicBank(bindName);
        var signInfo = MorMorAPI.SignManager.Query(args.EventArgs.Group.Id, userid);
        var sign = signInfo != null ? signInfo.Date : 0;
        var currencyInfo = MorMorAPI.CurrencyManager.Query(args.EventArgs.Group.Id, userid);
        var currency = currencyInfo != null ? currencyInfo.num : 0;
        var exp = api == null || api.IsSuccess ? 0 : api.CurrentNum;
        MessageBody body = new()
        {
            MomoSegment.Image(args.EventArgs.SenderInfo.TitleImage),
            MomoSegment.Text($"[QQ账号]:{userid}\n"),
            MomoSegment.Text($"[签到时长]:{sign}\n"),
            MomoSegment.Text($"[星币数量]:{currency}\n"),
            MomoSegment.Text($"[拥有权限]:{group}\n"),
            MomoSegment.Text($"[绑定角色]:{bindName}\n"),
            MomoSegment.Text($"[经验数量]:{exp}\n"),
            MomoSegment.Text($"[所在服务器]:{serverName}")
        };
        await args.EventArgs.Reply(body);
    }
    #endregion

    #region Wiki
    [CommandMatch("wiki", OneBotPermissions.TerrariaWiki)]
    private async Task Wiki(CommandArgs args)
    {
        string url = "https://terraria.wiki.gg/zh/index.php?search=";
        var msg = args.Parameters.Count > 0 ? url += HttpUtility.UrlEncode(args.Parameters[0]) : url.Split("?")[0];
        await args.EventArgs.Reply(msg);
    }
    #endregion
}
