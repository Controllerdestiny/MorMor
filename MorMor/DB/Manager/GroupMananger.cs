using MorMor.Exceptions;
using MorMor.Extensions;
using MorMor.Model.Database;
using MySql.Data.MySqlClient;
using System.Data;

namespace MorMor.DB.Manager;

public class GroupMananger
{
    private readonly IDbConnection database;

    private readonly DefaultGroup DefaultGroup = new();

    public readonly List<Group> Groups = [];
    public GroupMananger()
    {
        database = MorMorAPI.DB;
        var table = new SqlTable("GroupList",
            new SqlColumn("Name", MySqlDbType.VarChar) { Unique = true, Length = 255 },
            new SqlColumn("Permission", MySqlDbType.Text),
            new SqlColumn("Parent", MySqlDbType.Text) { DefaultValue = "" }
            );
        var create = new SqlTableCreator(database,
                database.GetSqlType() == SqlType.Sqlite
                    ? new SqliteQueryCreator()
                    : new MysqlQueryCreator());

        if (create.EnsureTableStructure(table))
        {
            AddGroup(DefaultGroup.Name, DefaultGroup.Permssion);
        }
        Groups = GetGroups();
    }

    public void AddGroup(string groupName, string perms = "")
    {
        if (HasGroup(groupName))
        {
            throw new GroupException("此组已经存在了，无法重复添加!");
        }
        var exec = database.Query("INSERT INTO `grouplist` (`Name`, `Permission`, `Parent`) VALUES (@0, @1, @2)", groupName, perms, groupName == MorMorAPI.Setting.DefaultPermGroup ? "" : MorMorAPI.Setting.DefaultPermGroup);
        if (exec != 1)
            throw new GroupException("添加至数据库失败!");
        else
        {
            if (groupName == "superadmin")
                Groups.Add(new SuperAdminGroup());
            else
                Groups.Add(new Group(groupName, null, null));
        }
    }

    public void AddPerm(string groupName, string perm)
    {
        var group = GetGroup(groupName) ?? throw new GroupException($"组 {groupName} 不存在!");
        if (!group.permissions.Contains(perm))
        {
            group.AddPermission(perm);
            if (database.Query("UPDATE `grouplist` SET `Permission` = @0 WHERE `grouplist`.`Name` = @1", group.Permssion, groupName) != 1)
            {
                group.RemovePermission(perm);
                throw new GroupException("添加至数据库失败!");
            }
        }
        else
        {
            throw new GroupException("权限已存在请不要重复添加!!");
        }
    }

    public void ReParentGroup(string groupName, string Parent)
    {
        var group = GetGroup(groupName) ?? throw new GroupException($"组 {groupName} 不存在!");
        if (database.Query("UPDATE `grouplist` SET `Parent` = @0 WHERE `grouplist`.`Name` = @1", Parent, groupName) == 1)
        {
            group.Parent = GetGroupNullDefault(Parent);
        }
        else
        {
            throw new GroupException("更新至数据库失败!");
        }
    }


    public bool HasGroup(string? Name)
    {
        return Groups.Any(x => x.Name == Name);
    }

    public void RemovePerm(string groupName, string perm)
    {
        var group = GetGroup(groupName) ?? throw new GroupException("删除权限指向的目标组不存在!");
        if (group.permissions.Contains(perm))
        {
            group.RemovePermission(perm);
            if (database.Query("UPDATE `grouplist` SET `Permission` = @0 WHERE `grouplist`.`Name` = @1", group.Permssion, groupName) != 1)
            {
                group.permissions.Add(perm);
                throw new GroupException("添加至数据库失败!");
            }
        }
        else
        {
            throw new GroupException("此组没有该权限无需删除!");
        }
    }

    public void RemoveGroup(string groupName)
    {
        if (!HasGroup(groupName))
            throw new GroupException($"组 {groupName} 不存在!");
        if (database.Query("DELETE FROM `grouplist` WHERE `grouplist`.`Name` = @0", groupName) == 1)
        {
            Groups.RemoveAll(f => f.Name == groupName);
        }
        else
        {
            throw new GroupException("更新至数据库失败!");
        }
    }

    public List<Group> GetGroups()
    {
        List<Group> list = [];
        using var read = database.QueryReader("select * from `grouplist`");
        while (read.Read())
        {
            var GroupName = read.Get<string>("Name");
            var permssions = read.Get<string>("Permission");
            var Parent = read.Get<string>("Parent");
            if (GroupName == "superadmin")
                list.Add(new SuperAdminGroup());
            else if (GroupName == MorMorAPI.Setting.DefaultPermGroup)
            {
                DefaultGroup.permissions = DefaultGroup.permissions.Concat(permssions.Split(",")).Distinct().ToList();
                list.Add(DefaultGroup);
            }
            else if (!string.IsNullOrEmpty("parent"))
                list.Add(new Group(GroupName, new Group(Parent), permssions));
            else
                list.Add(new Group(GroupName, null, permssions));
        }
        return list;
    }

    public List<string> GetGroupPerms(string groupName)
    {
        return Groups.Find(f => f.Name == groupName)?.permissions ?? new List<string>();
    }

    public bool HasPermssions(string groupName, string perm)
    {
        return Groups.Where(x => x.Name == groupName).Any(x => x.permissions.Contains(perm));
    }

    public Group? GetGroup(string groupName)
    {
        return Groups.Find(f => f.Name == groupName);
    }

    public Group GetGroupNullDefault(string groupName)
    {
        return Groups.Find(f => f.Name == groupName)
            ?? DefaultGroup;
    }
}
