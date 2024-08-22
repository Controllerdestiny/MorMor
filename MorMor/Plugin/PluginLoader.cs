using System.Reflection;

namespace MorMor.Plugin;

public class PluginLoader
{
    public static PluginContext PluginContext { get; private set; } = new(Guid.NewGuid().ToString());

    public readonly static string PATH = Path.Combine(MorMorAPI.PATH, "Plugins");

    /// <summary>
    /// 加载插件
    /// </summary>
    public static void Load()
    {
        DirectoryInfo directoryInfo = new(PATH);
        if (!directoryInfo.Exists)
            directoryInfo.Create();
        PluginContext.LoadPlugins(directoryInfo);
        Commands.CommandManager.Hook.MappingCommands(Assembly.GetExecutingAssembly());
        PluginContext.LoadAssemblys.ForEach(Commands.CommandManager.Hook.MappingCommands);    }

    public static void UnLoad()
    {
        PluginContext.UnloadPlugin();
        PluginContext = new(Guid.NewGuid().ToString());
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
