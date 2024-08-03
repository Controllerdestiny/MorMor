using System.Reflection;
using System.Runtime.Loader;
using Newtonsoft.Json;

namespace MorMor.Plugin;

public class PluginAssemblyLoadContext : AssemblyLoadContext
{

    public static FileInfo[] LoadFiles = [];

    public readonly List<MorMorPlugin> Plugins = new();

    public readonly List<Assembly> LoadAssemblies = new();
    public PluginAssemblyLoadContext(string name, string path) : base(name, true)
    {
        DirectoryInfo directoryInfo = new(path);
        if (!directoryInfo.Exists)
            directoryInfo.Create();
        LoadFiles = directoryInfo.GetFiles("*.dll");
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        for (int i = 0; i < LoadAssemblies.Count; i++)
            if (LoadAssemblies[i].GetName() == assemblyName)
                return LoadAssemblies[i];
     
        return Default.LoadFromAssemblyName(assemblyName);
    }
    public void LoadPlugin()
    {
        List<MorMorPlugin> _plugins = [];
        foreach (var file in LoadFiles)
        {
            using var stream = File.OpenRead(file.FullName);
            using var pdbStream = File.Exists(Path.ChangeExtension(file.FullName, ".pdb")) ? File.OpenRead(Path.ChangeExtension(file.FullName, ".pdb")) : null;
            var assembly =  LoadFromStream(stream, pdbStream);
            LoadAssemblies.Add(assembly);
            foreach (var exportedType in assembly.GetExportedTypes())
                if (exportedType.IsSubclassOf(typeof(MorMorPlugin)) && !exportedType.IsAbstract)
                    if (Activator.CreateInstance(exportedType) is MorMorPlugin instance)
                        _plugins.Add(instance);
        }
        Plugins.AddRange(_plugins.OrderBy(p => p.Order));
        Plugins.ForEach(p => p.Initialize());
    }
    public void UnloadPlugin()
    {
        Plugins.ForEach(x =>
        {
            x.Dispose();
        });
        Plugins.Clear();
        LoadAssemblies.Clear();
        Unload();
    }
}
