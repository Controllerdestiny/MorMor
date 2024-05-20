using MorMor.Extensions;

namespace MorMor.Model.Database;

public class Group
{
    public string Name { get; set; }

    private List<string> negatedpermissions { get; set; } = new();

    public List<string> permissions { get; set; } = new();

    public Group Parent { get; set; }

    public string Permssion
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
            value?.Split(',').ForEach(p => AddPermission(p.Trim()));
        }
    }

    public Group(string groupname, Group parentgroup = null, string permissions = null)
    {
        Name = groupname;
        Parent = parentgroup;
        Permssion = permissions;
    }

    public virtual void AddPermission(string permission)
    {
        if (permission.StartsWith("!"))
        {
            NegatePermission(permission[1..]);
            return;
        }

        if (!permissions.Contains(permission))
        {
            permissions.Add(permission);
            negatedpermissions.Remove(permission);
        }
    }

    public virtual void NegatePermission(string permission)
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

    public virtual void RemovePermission(string permission)
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
                    throw new Exception("Infinite group parenting ({0})".SFormat(cur.Name));
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
                throw new InvalidOperationException("Infinite group parenting ({0})".SFormat(cur.Name));
            }
            traversed.Add(cur);
            cur = cur.Parent;
        }
        return false;
    }
}
