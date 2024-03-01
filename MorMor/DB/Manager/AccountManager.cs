using MorMor.Exceptions;
using MorMor.Extensions;
using MySql.Data.MySqlClient;
using System.Data;
using MorMor.Event;

namespace MorMor.DB.Manager;

public class AccountManager
{
    public class Account
    {
        public long UserId { get; }

        public Group Group { get; set; }

        public Account(long userid, Group group)
        {
            UserId = userid;
            Group = group;
        }

        public bool HasPermission(string perm)
        {
            var result = OperatHandler.UserPermission(this, perm);
            return result switch
            {
                Enumeration.UserPermissionType.Unhandled => false,
                Enumeration.UserPermissionType.Granted => true,
                _ => Group.HasPermission(perm)
            };
        }
    }

    private readonly IDbConnection database;

    public List<Account> Accounts;

    public AccountManager()
    {
        database = MorMorAPI.DB;
        var table = new SqlTable("Account",
            new SqlColumn("ID", MySqlDbType.Int64) { Unique = true },
            new SqlColumn("GroupID", MySqlDbType.Int64) { Unique = true },
            new SqlColumn("Group", MySqlDbType.Text) { DefaultValue = MorMorAPI.Setting.DefaultPermGroup }
            );

        var create = new SqlTableCreator(database, new MysqlQueryCreator());
        create.EnsureTableStructure(table);
        Accounts = GetAccpunts();
    }

    /// <summary>
    /// 缓存账户数据
    /// </summary>
    /// <returns></returns>
    private List<Account> GetAccpunts()
    {
        List<Account> accounts = new();
        using var read = database.QueryReader("select * from `Account`");
        while (read.Read())
        {
            var ID = read.Get<long>("ID");
            var GroupName = read.Get<string>("Group");
            var GroupID = read.Get<long>("GroupID");
            accounts.Add(new Account(ID, MorMorAPI.GroupManager.GetGroupData(GroupID, GroupName)));
        }
        return accounts;
    }


    /// <summary>
    /// 查找是否有指定账户
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="groupid"></param>
    /// <returns></returns>
    public bool HasAccount(long ID, long groupid)
    {
        return Accounts.Any(x => x.UserId == ID && x.Group.GroupId == groupid);
    }

    /// <summary>
    /// 添加账户
    /// </summary>
    /// <param name="id"></param>
    /// <param name="groupid"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    public void AddAccount(long id, long groupid, string group)
    {
        if (HasAccount(id, groupid))
            throw new AccountException($"账户 {id} 已经存在了，无法重复添加!");
        if (!MorMorAPI.GroupManager.HasGroup(groupid, group))
            throw new AccountException($"组 {group} 不存在，无法添加!");
        var exec = database.Query("INSERT INTO `Account` (`ID`, `GroupID`, `Group`) VALUES (@0, @1, @2)", id, groupid, group);
        if (exec == 1)
        {
            Accounts.Add(new Account(id, MorMorAPI.GroupManager.GetGroupData(groupid, group)));
        }
        else
        {
            throw new AccountException($"添加至数据库失败!");
        }
    }

    public bool HasPermssion(long id, long groupid, string perm)
    {
        var account = GetAccount(id, groupid);
        if (account is not null && account.Group is not null)
        {
            return account.Group.HasPermission(perm);
        }
        return false;
    }


    public Account GetAccount(long id, long groupid)
    {
        return Accounts.Find(x => x.UserId == id && x.Group.GroupId == groupid)
            ?? new Account(id, MorMorAPI.GroupManager.GetGroupData(groupid, MorMorAPI.Setting.DefaultPermGroup));
    }

    public List<Account> GetAccounts(long groupid)
    {
        return Accounts.FindAll(x => x.Group.GroupId == groupid);
    }

    public bool TryGetAccoumt(long id, long groupid, out Account account)
    {
        account = GetAccount(id, groupid);
        return account is not null;
    }

    /// <summary>
    /// 更改账户组
    /// </summary>
    /// <param name="id"></param>
    /// <param name="groupid"></param>
    /// <param name="Group"></param>
    /// <returns></returns>
    public void ReAccountGroup(long id, long groupid, string Group)
    {
        var account = GetAccount(id, groupid);
        if (account == null)
            throw new AccountException($"账户 {id} 不存在无法更改组!");
        if (database.Query("UPDATE `Account` SET `Group` = @0 WHERE `Account`.`GroupID` = @1 AND `Account`.`ID` = @2", Group, groupid, id) == 1)
        {
            account.Group = MorMorAPI.GroupManager.GetGroupData(groupid, Group);
        }
        else
        {
            throw new AccountException("更新至数据库失败!");
        }
    }

    /// <summary>
    /// 移除账户
    /// </summary>
    /// <param name="id"></param>
    /// <param name="groupid"></param>
    /// <returns></returns>
    public void RemoveAccount(long id, long groupid)
    {
        if (!HasAccount(id, groupid))
            throw new AccountException($"账户 {id} 不存在，无法移除!");
        if (database.Query("DELETE FROM `Account` WHERE `Account`.`ID` = @0 AND `Account`.`GroupID` = @1", id, groupid) == 1)
        {
            Accounts.RemoveAll(f => f.Group.GroupId == groupid && f.UserId == id);
        }
        else
        {
            throw new AccountException("更新至数据库失败!");
        }
    }
}
