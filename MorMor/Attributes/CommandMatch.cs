namespace MorMor.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CommandMatch : Attribute
{
    public List<string> Name { get; init; }

    public string[] Permission { get; }

    public CommandMatch(string name, params string[] permission)
    {
        Name = new() { name };
        Permission = permission;
    }

    public CommandMatch(List<string> name, params string[] permission)
    {
        Name = name;
        Permission = permission;
    }
}
