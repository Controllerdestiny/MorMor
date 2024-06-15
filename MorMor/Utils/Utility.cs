using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Terraria;

namespace MorMor.Utils;

public class Utility
{
    
    public static Item GetItemById(int id)
    {
        var item = new Item();
        item.netDefaults(id);
        return item;
    }

    
    public static List<Item> GetItemByIdOrName(string text)
	{
		int type = -1;
		if (Int32.TryParse(text, out type))
		{
			if (type >= Terraria.ID.ItemID.Count)
				return new List<Item>();
			return new List<Item> { GetItemById(type) };
		}
		Item item = GetItemFromTag(text);
		if (item != null)
			return new List<Item>() { item };
		return GetItemByName(text);
	}
		
	public static Item GetItemFromTag(string tag)
	{
		Regex regex = new Regex(@"\[i(tem)?(?:\/s(?<Stack>\d{1,4}))?(?:\/p(?<Prefix>\d{1,3}))?:(?<NetID>-?\d{1,4})\]");
		Match match = regex.Match(tag);
		if (!match.Success)
			return null;
		Item item = new Item();
		item.netDefaults(Int32.Parse(match.Groups["NetID"].Value));
		if (!String.IsNullOrWhiteSpace(match.Groups["Stack"].Value))
			item.stack = Int32.Parse(match.Groups["Stack"].Value);
		if (!String.IsNullOrWhiteSpace(match.Groups["Prefix"].Value))
			item.prefix = Byte.Parse(match.Groups["Prefix"].Value);
		return item;
	}
    
    public static List<Item> GetItemByName(string name)
	{
		var startswith = new List<int>();
		var contains = new List<int>();
		for (int i = 1; i < Terraria.ID.ItemID.Count; i++)
		{
			var currentName = Lang.GetItemNameValue(i);
			if (!string.IsNullOrEmpty(currentName))
			{
				if (currentName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
					return new List<Item> { GetItemById(i) };
				if (currentName.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
				{
					startswith.Add(i);
					continue;
				}
				if (currentName.Contains(name, StringComparison.InvariantCultureIgnoreCase))
				{
					contains.Add(i);
					continue;
				}
			}
		}

		if (startswith.Count != 1)
			startswith.AddRange(contains);
		return startswith.Select(GetItemById).ToList();
	}

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
}
