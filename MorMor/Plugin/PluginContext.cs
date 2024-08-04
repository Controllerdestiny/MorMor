using System.Reflection;
using System.Runtime.Loader;
using System.Security.Permissions;
using MomoAPI.Utils;

namespace MorMor.Plugin;

public class PluginContext : AssemblyLoadContext
{
    public readonly List<Assembly> lLoadAssemblys = [];

    public readonly List<MorMorPlugin> Plugins = [];

    public PluginContext(string name) : base(name, true)
    {
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        foreach (Assembly assembly in lLoadAssemblys)
        { 
            if(assembly.GetName() == assemblyName)
                return assembly;
        }
        return null;
    }

    public void LoadPlugins(DirectoryInfo dir)
    {
        foreach (FileInfo file in dir.GetFiles("*.dll"))
        {
            using var stream = file.OpenRead();
            using var pdbStream = File.Exists(Path.ChangeExtension(file.FullName, ".pdb")) ? File.OpenRead(Path.ChangeExtension(file.FullName, ".pdb")) : null;
            var assembly = LoadFromStream(stream, pdbStream);
            lLoadAssemblys.Add(assembly);
        }
        foreach (Assembly assembly in lLoadAssemblys)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.IsSubclassOf(typeof(MorMorPlugin)) && !type.IsAbstract)
                {
                    if (Activator.CreateInstance(type) is MorMorPlugin instance)
                        Plugins.Add(instance);
                }
            }
        }
        foreach(var dt in dir.GetDirectories())
            LoadPlugins(dt);
        Plugins.OrderBy(p=>p.Order).ForEach(p=>p.Initialize());
    }

    public void UnloadPlugin()
    {
        Plugins.ForEach(x =>x.Dispose());
        Plugins.Clear();
        lLoadAssemblys.Clear();
        Unload();
    }
}
