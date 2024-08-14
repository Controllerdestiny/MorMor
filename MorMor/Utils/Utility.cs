using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MorMor.Utils;

public class Utility
{

    [DllImport("psapi.dll")]
    private static extern bool EmptyWorkingSet(IntPtr lpAddress);

    public static void FreeMemory()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        foreach (var process in Process.GetProcesses())
        {
            if ((process.ProcessName == "System") && (process.ProcessName == "Idle"))
                continue;
            try
            {
                EmptyWorkingSet(process.Handle);
            }
            catch { }
        }
    }

    #region 获得内存信息API
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GlobalMemoryStatusEx(ref MEMORY_INFO mi);

    //定义内存的信息结构
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_INFO
    {
        public uint dwLength; //当前结构体大小
        public uint dwMemoryLoad; //当前内存使用率
        public ulong ullTotalPhys; //总计物理内存大小
        public ulong ullAvailPhys; //可用物理内存大小
        public ulong ullTotalPageFile; //总计交换文件大小
        public ulong ullAvailPageFile; //总计交换文件大小
        public ulong ullTotalVirtual; //总计虚拟内存大小
        public ulong ullAvailVirtual; //可用虚拟内存大小
        public ulong ullAvailExtendedVirtual; //保留 这个值始终为0
    }
    #endregion

    #region 格式化容量大小
    /// <summary>
    /// 格式化容量大小
    /// </summary>
    /// <param name="size">容量（B）</param>
    /// <returns>已格式化的容量</returns>
    public static string FormatSize(double size)
    {
        double d = (double)size;
        int i = 0;
        while ((d > 1024) && (i < 5))
        {
            d /= 1024;
            i++;
        }
        string[] unit = { "B", "KB", "MB", "GB", "TB" };
        return (string.Format("{0} {1}", Math.Round(d, 2), unit[i]));
    }
    #endregion

    #region 获得当前内存使用情况
    /// <summary>
    /// 获得当前内存使用情况
    /// </summary>
    /// <returns></returns>
    public static MEMORY_INFO GetMemoryStatus()
    {
        MEMORY_INFO mi = new MEMORY_INFO();
        mi.dwLength = (uint)System.Runtime.InteropServices.Marshal.SizeOf(mi);
        GlobalMemoryStatusEx(ref mi);
        return mi;
    }
    #endregion

    #region 获得当前可用物理内存大小
    /// <summary>
    /// 获得当前可用物理内存大小
    /// </summary>
    /// <returns>当前可用物理内存（B）</returns>
    public static ulong GetAvailPhys()
    {
        MEMORY_INFO mi = GetMemoryStatus();
        return mi.ullAvailPhys;
    }
    #endregion

    #region 获得当前已使用的内存大小
    /// <summary>
    /// 获得当前已使用的内存大小
    /// </summary>
    /// <returns>已使用的内存大小（B）</returns>
    public static ulong GetUsedPhys()
    {
        MEMORY_INFO mi = GetMemoryStatus();
        return (mi.ullTotalPhys - mi.ullAvailPhys);
    }
    #endregion

    #region 获得当前总计物理内存大小
    /// <summary>
    /// 获得当前总计物理内存大小
    /// </summary>
    /// <returns&gt;总计物理内存大小（B）&lt;/returns&gt;
    public static ulong GetTotalPhys()
    {
        MEMORY_INFO mi = GetMemoryStatus();
        return mi.ullTotalPhys;
    }
    #endregion

    public static void KillChrome()
    {
        foreach (var process in Process.GetProcesses())
        {
            if (process.ProcessName.Contains("chrome"))
            {
                process.Kill();
            }
        }
    }

    public static async Task<Stream> Markdown(string md)
    {
        return await MarkdownHelper.ToImage(md);
    }

    public static Model.Socket.Internet.Item? GetItemById(int id)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var file = assembly.GetName().Name + ".Resources.TerrariaID.json";
        var stream = assembly.GetManifestResourceStream(file)!;
        using var reader = new StreamReader(stream);
        var jobj = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd());
        var array = jobj?.Value<JArray>("物品");
        foreach (var item in array)
        {
            if (item.Value<int>("ID") == id)
            {
                return new Model.Socket.Internet.Item()
                {
                    Name = item.Value<string>("中文名称")!,
                    netID = id
                };
            }
        }
        return null;
    }

    public static List<Model.Socket.Internet.Item> GetItemByName(string name)
    {
        var list = new List<Model.Socket.Internet.Item>();
        var assembly = Assembly.GetExecutingAssembly();
        var file = assembly.GetName().Name + ".Resources.TerrariaID.json";
        var stream = assembly.GetManifestResourceStream(file)!;
        using var reader = new StreamReader(stream);
        var jobj = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd());
        var array = jobj?.Value<JArray>("物品")!;
        foreach (var item in array)
        {
            if (item.Value<string>("中文名称")!.Contains(name))
            {
                list.Add(new Model.Socket.Internet.Item()
                {
                    Name = item.Value<string>("中文名称")!,
                    netID = item.Value<int>("ID")
                });
            }
        }
        return list;
    }

    public static List<Model.Socket.Internet.Item> GetItemByIdOrName(string ji)
    {
        var list = new List<Model.Socket.Internet.Item>();
        if (int.TryParse(ji, out var i))
        {
            var item = GetItemById(i);
            if (item != null)
                list.Add(item);
        }
        else
        {
            list.AddRange(GetItemByName(ji));
        }
        return list;
    }
}
