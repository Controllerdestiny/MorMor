using MorMor.Event;
using MorMor.Exceptions;
using MorMor.Extensions;
using MorMor.Model.Database;
using MySql.Data.MySqlClient;
using System.Data;

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

    public List<Account> Accounts { get; } = [];

    public AccountManager()
    {
        database = MorMorAPI.DB;
        var table = new SqlTable("Account",
            new SqlColumn("ID", MySqlDbType.Int64) { Unique = true },
            new SqlColumn("Group", MySqlDbType.Text) { DefaultValue = MorMorAPI.Setting.DefaultPermGroup }
            );
        var create = new SqlTableCreator(database,
                database.GetSqlType() == SqlType.Sqlite
                    ? new SqliteQueryCreator()
                    : new MysqlQueryCreator());

        create.EnsureTableStructure(table);
        Accounts = GetAccounts();

    }

    /// <summary>
    /// 缓存账户数据
    /// </summary>
    /// <returns></returns>
    private List<Account> GetAccounts()
    {
        List<Account> accounts = new();
        using var read = database.QueryReader("select * from `Account`");
        while (read.Read())
        {
            var ID = read.Get<long>("ID");
            var GroupName = read.Get<string>("Group");
            accounts.Add(new Account(ID, MorMorAPI.GroupManager.GetGroupNullDefault(GroupName)));
        }
        return accounts;
    }


    /// <summary>
    /// 查找是否有指定账户
    /// </summary>
    /// <param name="userid"></param>
    /// <param name="groupid"></param>
    /// <returns></returns>
    public bool HasAccount(long userid)
    {
        return Accounts.Any(x => x.UserId == userid);
    }

    /// <summary>
    /// 添加账户
    /// </summary>
    /// <param name="userid"></param>
    /// <param name="groupid"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    public void AddAccount(long userid, string group)
    {
        if (HasAccount(userid))
            throw new AccountException($"账户 {userid} 已经存在了，无法重复添加!");
        if (!MorMorAPI.GroupManager.HasGroup(group))
            throw new AccountException($"组 {group} 不存在，无法添加!");
        var exec = database.Query("INSERT INTO `Account` (`ID`, `Group`) VALUES (@0, @1)", userid, group);
        if (exec == 1)
        {
            Accounts.Add(new Account(userid, MorMorAPI.GroupManager.GetGroupNullDefault(group)));
        }
        else
        {
            throw new AccountException($"添加至数据库失败!");
        }
    }

    public bool HasPermssion(long userid, string perm)
    {
        return GetAccountNullDefault(userid).HasPermission(perm);
    }


    public Account GetAccountNullDefault(long userid)
    {
        return Accounts.Find(x => x.UserId == userid)
            ?? new Account(userid, MorMorAPI.GroupManager.GetGroupNullDefault(""));
    }

    public Account? GetAccount(long userid)
    {
        return Accounts.Find(x => x.UserId == userid);
    }

    public bool TryGetAccount(long userid, out Account? account)
    {
        account = GetAccount(userid);
        return account is not null;
    }

    /// <summary>
    /// 更改账户组
    /// </summary>
    /// <param name="userid"></param>
    /// <param name="Group"></param>
    /// <returns></returns>
    public void ReAccountGroup(long userid, string Group)
    {
        if (!HasAccount(userid))
            throw new AccountException($"账户 {userid} 不存在无法更改组!");
        if (database.Query("UPDATE `Account` SET `Group` = @0 WHERE `Account`.`Account`.`ID` = @1", Group, userid) == 1)
        {
            GetAccountNullDefault(userid).Group = MorMorAPI.GroupManager.GetGroupNullDefault(Group);
        }
        else
        {
            throw new AccountException("更新至数据库失败!");
        }
    }

    /// <summary>
    /// 移除账户
    /// </summary>
    /// <param name="userid"></param>
    /// <returns></returns>
    public void RemoveAccount(long userid)
    {
        if (!HasAccount(userid))
            throw new AccountException($"账户 {userid} 不存在，无法移除!");
        if (database.Query("DELETE FROM `Account` WHERE `Account`.`ID` = @0", userid) == 1)
        {
            Accounts.RemoveAll(f => f.UserId == userid);
        }
        else
        {
            throw new AccountException("更新至数据库失败!");
        }
    }
}
