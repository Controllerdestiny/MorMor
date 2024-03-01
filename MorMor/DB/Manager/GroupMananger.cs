using MorMor.Extensions;
using MySql.Data.MySqlClient;
using System.Data;

namespace MorMor.DB.Manager;
public class Group
{
    public string GroupName { get; set; }

    public List<string> negatedpermissions = new();

    public List<string> permissions = new();

    public Group? Parent { get; set; }
    public long GroupId { get; set; }

    public string Permssions
    {
        get
        {
            List<string> all = new(permissions);
            negatedpermissions.ForEach(p => all.Add("!" + p));
            return string.Join(",", all);
        }
        set
        {
            permissions.Clear();
            negatedpermissions.Clear();
            if (null != value)
                value.Split(',').ForEach(p => AddPermission(p.Trim()));
        }
    }

    public Group(string groupname, long groupid, Group parentgroup = null, string permissions = null)
    {
        GroupName = groupname;
        Parent = parentgroup;
        GroupId = groupid;
        Permssions = permissions;
    }

    public void AddPermission(string permission)
    {
        if (permission.StartsWith("!"))
        {
            NegatePermission(permission.Substring(1));
            return;
        }
        // Avoid duplicates
        if (!permissions.Contains(permission))
        {
            permissions.Add(permission);
            negatedpermissions.Remove(permission); // Ensure we don't have conflicting definitions for a permissions
        }
    }

    public void NegatePermission(string permission)
    {
        // Avoid duplicates
        if (!negatedpermissions.Contains(permission))
        {
            negatedpermissions.Add(permission);
            permissions.Remove(permission); // Ensure we don't have conflicting definitions for a permissions
        }
    }

    public void SetPermission(List<string> permission)
    {
        permissions.Clear();
        negatedpermissions.Clear();
        permission.ForEach(p => AddPermission(p));
    }

    /// <summary>
    /// Will remove a permission from the respective list,
    /// where "!permission" will remove a negated permission.
    /// </summary>
    /// <param name="permission"></param>
    public void RemovePermission(string permission)
    {
        if (permission.StartsWith("!"))
        {
            negatedpermissions.Remove(permission.Substring(1));
            return;
        }
        permissions.Remove(permission);
    }

    public virtual List<string> TotalPermissions
    {
        get
        {
            var cur = this;
            var traversed = new List<Group>();
            HashSet<string> all = new();
            while (cur != null)
            {
                foreach (var perm in cur.permissions)
                {
                    all.Add(perm);
                }

                foreach (var perm in cur.negatedpermissions)
                {
                    all.Remove(perm);
                }

                if (traversed.Contains(cur))
                {
                    throw new Exception("Infinite group parenting ({0})".SFormat(cur.GroupName));
                }
                traversed.Add(cur);
                cur = cur.Parent;
            }
            return all.ToList();
        }
    }

    public virtual bool HasPermission(string permission)
    {
        bool negated = false;
        if (String.IsNullOrEmpty(permission) || (RealHasPermission(permission, ref negated) && !negated))
        {
            return true;
        }

        if (negated)
            return false;

        string[] nodes = permission.Split('.');
        for (int i = nodes.Length - 1; i >= 0; i--)
        {
            nodes[i] = "*";
            if (RealHasPermission(String.Join(".", nodes, 0, i + 1), ref negated))
            {
                return !negated;
            }
        }
        return false;
    }

    private bool RealHasPermission(string permission, ref bool negated)
    {
        negated = false;
        if (string.IsNullOrEmpty(permission))
            return true;

        var cur = this;
        var traversed = new List<Group>();
        while (cur != null)
        {
            if (cur.negatedpermissions.Contains(permission))
            {
                negated = true;
                return false;
            }
            if (cur.permissions.Contains(permission))
                return true;
            if (traversed.Contains(cur))
            {
                throw new InvalidOperationException("Infinite group parenting ({0})".SFormat(cur.GroupName));
            }
            traversed.Add(cur);
            cur = cur.Parent;
        }
        return false;
    }


    public class SuperAdminGroup : Group
    {
        /// <summary>
        /// The superadmin class has every permission, represented by '*'.
        /// </summary>
        public override List<string> TotalPermissions
        {
            get { return new List<string> { "*" }; }
        }

        /// <summary>
        /// Initializes a new instance of the SuperAdminGroup class with the configured parameters.
        /// Those can be changed in the config file.
        /// </summary>
        public SuperAdminGroup(long groupid)
            : base("superadmin", groupid)
        {

        }

        /// <summary>
        /// Override to allow access to everything.
        /// </summary>
        /// <param name="permission">The permission</param>
        /// <returns>True</returns>
        public override bool HasPermission(string permission)
        {
            return true;
        }
    }
}

public class GroupException : Exception
{
    public GroupException(string message)
        : base(message) { }
}


public class GroupMananger
{
    private readonly IDbConnection database;

    public List<Group> Groups;
    public GroupMananger()
    {
        database = MorMorAPI.DB;
        var table = new SqlTable("GroupList",
            new SqlColumn("GroupName", MySqlDbType.VarChar) { Unique = true, Length = 255 },
            new SqlColumn("ID", MySqlDbType.Int64) { Unique = true },
            new SqlColumn("Perms", MySqlDbType.Text),
            new SqlColumn("Parent", MySqlDbType.Text) { DefaultValue = "" }
            );

        var create = new SqlTableCreator(database, new MysqlQueryCreator());
        create.EnsureTableStructure(table);
        Groups = GetGroups();
        TidyUpParent();
    }



    public void AddGroup(long groupid, string groupName)
    {
        if (HasGroup(groupid, groupName))
        {
            throw new GroupException("此组已经存在了，无法重复添加!");
        }
        var exec = database.Query("INSERT INTO `grouplist` (`GroupName`, `ID`, `Perms`, `Parent`) VALUES (@0, @1, @2, @3)", groupName, groupid, "", "");
        if (exec != 1)
            throw new GroupException("添加至数据库失败!");
        else
        {
            if (groupName == "superadmin")
                Groups.Add(new Group.SuperAdminGroup(groupid));
            else
                Groups.Add(new Group(groupName, groupid, null, null));
        }

    }

    public void AddPerm(long groupid, string groupName, string perm)
    {
        var group = GetGroupData(groupid, groupName);
        if (group == null)
        {
            throw new GroupException($"组 {groupName} 不存在!");
        }
        if (!group.permissions.Contains(perm))
        {
            group.AddPermission(perm);
            if (database.Query("UPDATE `grouplist` SET `Perms` = @0 WHERE `grouplist`.`GroupName` = @1 AND `grouplist`.`ID` = @2", string.Join(",", group.permissions), groupName, groupid) == 1)
            {
                group.Permssions = string.Join(",", group.Permssions);
            }
            else
            {
                throw new GroupException("添加至数据库失败!");
            }
        }
        else
        {
            throw new GroupException("权限已存在请不要重复添加!!");
        }
    }

    public void ReParentGroup(long groupid, string groupName, string Parent)
    {
        var group = GetGroupData(groupid, groupName);
        if (group == null)
            throw new GroupException($"组 {groupName} 不存在!");
        if (database.Query("UPDATE `grouplist` SET `Parent` = @0 WHERE `grouplist`.`GroupName` = @1 AND `grouplist`.`ID` = @2", Parent, groupName, groupid) == 1)
        {
            group.Parent = GetGroupData(groupid, Parent);
        }
        else
        {
            throw new GroupException("更新至数据库失败!");
        }
    }


    public bool HasGroup(long groupid, string Name)
    {
        return Groups.Any(x => x.GroupId == groupid && x.GroupName == Name);
    }

    public void RemovePerm(long groupid, string groupName, string perm)
    {
        var group = GetGroupData(groupid, groupName);
        if (group == null)
        {
            throw new GroupException("删除权限指向的目标组不存在!");
        }
        if (group.permissions.Contains(perm))
        {
            group.RemovePermission(perm);
            if (database.Query("UPDATE `grouplist` SET `Perms` = @0 WHERE `grouplist`.`GroupName` = @1 AND `grouplist`.`ID` = @2", string.Join(",", group.permissions), groupName, groupid) == 1)
            {
                group.Permssions = string.Join(",", group.permissions);
            }
            else
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

    public void RemoveGroup(long groupid, string groupName)
    {
        if (!HasGroup(groupid, groupName))
            throw new GroupException($"组 {groupName} 不存在!");
        if (database.Query("DELETE FROM `grouplist` WHERE `grouplist`.Group`Name` = @0 AND `grouplist`.`ID` = @1", groupName, groupid) == 1)
        {
            Groups.RemoveAll(f => f.GroupId == groupid && f.GroupName == groupName);
        }
        else
        {
            throw new GroupException("更新至数据库失败!");
        }
    }

    public List<Group> GetGroups()
    {
        List<Group> list = new();
        using var read = database.QueryReader("select * from `grouplist`");
        while (read.Read())
        {
            var GroupName = read.Get<string>("GroupName");
            var permssions = read.Get<string>("Perms");
            var Parent = read.Get<string>("Parent");
            var GroupId = read.Get<long>("ID");
            if (GroupName == "superadmin")
                list.Add(new Group.SuperAdminGroup(GroupId));
            else if (!string.IsNullOrEmpty("parent"))
                list.Add(new Group(GroupName, GroupId, new Group(Parent, GroupId), permssions));
            else
                list.Add(new Group(GroupName, GroupId, null, permssions));
        }
        return list;
    }

    private void TidyUpParent()
    {
        Groups.ForEach(x =>
        {
            if (x.Parent != null)
                x.Parent = GetGroupData(x.GroupId, x.Parent.GroupName);
        });
    }

    public List<string>? GetGroupPerms(long groupid, string groupName)
    {
        return Groups.Find(f => f.GroupId == groupid && f.GroupName == groupName)?.permissions;
    }

    public bool HasPermssions(long groupid, string groupName, string perm)
    {
        var group = Groups.Find(f => f.GroupId == groupid && f.GroupName == groupName);
        if (group == null)
        {
            return false;
        }
        else
        {
            return group.permissions.Contains(perm);

        }
    }

    public Group GetGroupData(long groupid, string groupName)
    {
        return GetGroupsById(groupid).Find(f => f.GroupName == groupName)
            ?? new Group(MorMorAPI.Setting.DefaultPermGroup, groupid);
    }

    public List<Group> GetGroupsById(long groupid)
    {
        return Groups.FindAll(f => f.GroupId == groupid);
    }
}
