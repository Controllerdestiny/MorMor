namespace MorMor.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CommandMatch : Attribute
{
    public List<string> Name { get; }

    public string[] Permission { get; }

    public CommandMatch(string name, params string[] permission) : this(permission)
    {
        Name = new() { name };
    }

    public CommandMatch(List<string> name, params string[] permission)
    {
        Name = name;
        Permission = permission;
    }

    private CommandMatch(params string[] permission)
    {
        Permission = permission;
    }
}
