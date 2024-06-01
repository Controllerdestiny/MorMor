using MomoAPI.Utils;
using MorMor.TShock.ChatCommand;
using System.Reflection;

namespace MorMor.Plugin;

internal class MappingPlugin
{
    private static List<MorMorPlugin> Instances = new();

    private static List<Assembly> Assemblies = new();

    /// <summary>
    /// 加载插件
    /// </summary>
    public static void Initializer()
    {
        var DirPath = Path.Combine(MorMorAPI.PATH, "Plugin");
        DirectoryInfo directoryInfo = new(DirPath);
        if (!directoryInfo.Exists)
            directoryInfo.Create();
        var files = directoryInfo.GetFiles("*.dll");
        GetAssmblyInstance(Assembly.GetExecutingAssembly());
        foreach (var file in files)
        {
            var assembly = Assembly.Load(File.ReadAllBytes(file.FullName));
            GetAssmblyInstance(assembly);
        }
        Instances.OrderByDescending(x => x.Order).ForEach(x =>
        {
            x.Initialize();

        });
        Assemblies.ForEach(Commands.CommandManager.Hook.MappingCommands);
        Assemblies.ForEach(ChatCommandMananger.Hook.MappingCommands);
    }

    private static void GetAssmblyInstance(Assembly assembly)
    {
        Assemblies.Add(assembly);
        assembly.GetExportedTypes().ForEach(x =>
        {
            if (x.IsSubclassOf(typeof(MorMorPlugin)) && x.IsPublic && !x.IsAbstract)
            {
                var Instance = Activator.CreateInstance(x) as MorMorPlugin;
                Instances.Add(Instance);
            }
        });
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
