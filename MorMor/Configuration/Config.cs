using Newtonsoft.Json;

namespace MorMor.Configuration;

public class Config
{
    /// <summary>
    /// 读取配置文件内容
    /// </summary>
    /// <param name="Path">配置文件路径</param>
    /// <returns></returns>
    public static T Read<T>(string Path) where T : new()
    {
        if (!File.Exists(Path)) return new T();
        using var fs = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Read<T>(fs);
    }

    /// <summary>
    /// 通过文件流读取文件内容
    /// </summary>
    /// <param name="stream">流</param>
    /// <returns></returns>
    public static T Read<T>(Stream stream) where T : new()
    {
        using var sr = new StreamReader(stream);
        var cf = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
        return cf ?? new T();
    }

    /// <summary>
    /// 写入配置
    /// </summary>
    /// <param name="Path">配置文件路径</param>
    public static void Write<T>(string Path, T obj)//给定路径进行写
    {
        using var fs = new FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.Write);
        Write(fs, obj);
    }

    /// <summary>
    /// 通过文件流写入配置
    /// </summary>
    /// <param name="stream">文件流</param>
    public static void Write<T>(Stream stream, T obj)//给定流文件写
    {
        var data = JsonConvert.SerializeObject(obj, Formatting.Indented);
        using var sw = new StreamWriter(stream);
        sw.Write(data);
        sw.Close();
    }

    /// <summary>
    /// 加载配置文件
    /// 如果配置出错那么将会直接覆盖原配置
    /// 建议使用重载
    /// </summary>
    /// <param name="PATH">路径</param>
    /// <returns>T</returns>
    public static T LoadConfig<T>(string PATH) where T : new()
    {
        T obj = new();
        if (File.Exists(PATH))
        {
            try
            {
                obj = Read<T>(PATH);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }
        }
        Write<T>(PATH, obj);
        return obj;
    }

    /// <summary>
    /// 加载配置文件
    /// </summary>
    /// <param name="PATH"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T LoadConfig<T>(string PATH, T obj) where T : new()
    {
        if (File.Exists(PATH))
        {
            try
            {
                obj = Read<T>(PATH);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }
        }
        Write(PATH, obj);
        return obj;
    }
}