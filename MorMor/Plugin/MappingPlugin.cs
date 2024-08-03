using MomoAPI.Utils;
using MorMor.TShock.ChatCommand;
using System.Reflection;
using System.Runtime.Loader;

namespace MorMor.Plugin;

public class MappingPlugin
{
    public readonly static string PluginPath = Path.Combine(MorMorAPI.PATH, "Plugin");

    public static PluginAssemblyLoadContext AssemblyLoadContext { get; private set; } = new("PluginLoader" + Guid.NewGuid().ToString(), PluginPath);

    /// <summary>
    /// 加载插件
    /// </summary>
    public static void Initializer()
    {
        Commands.CommandManager.Hook.MappingCommands(Assembly.GetExecutingAssembly());
        ChatCommandMananger.Hook.MappingCommands(Assembly.GetExecutingAssembly());
        AssemblyLoadContext.LoadPlugin();
        AssemblyLoadContext.LoadAssemblies.ForEach(Commands.CommandManager.Hook.MappingCommands);
        AssemblyLoadContext.LoadAssemblies.ForEach(ChatCommandMananger.Hook.MappingCommands);
    }

    public static void UnLoadPlugin()
    {
        AssemblyLoadContext.UnloadPlugin();
        AssemblyLoadContext = new("PluginLoader" + Guid.NewGuid().ToString(), PluginPath);
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    internal static Assembly? Resolve(object? sender, ResolveEventArgs args)
    {
        var dirpath = Path.Combine(MorMorAPI.PATH, "bin");
        if (!Directory.Exists(dirpath))
            Directory.CreateDirectory(dirpath);
        string fileName = args.Name.Split(',')[0];
        string path = Path.Combine(dirpath, fileName + ".dll");
        try
        {
            if (File.Exists(path))
            {
                Assembly assembly;

                assembly = Assembly.Load(File.ReadAllBytes(path));

                return assembly;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                string.Format("Error on resolving assembly \"{0}.dll\":\n{1}", fileName, ex));
        }
        return null; ;
    }
}
