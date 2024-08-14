global using MomoAPI.IO;
using System.Diagnostics;
using System.IO;

namespace MomoAPI.IO;

public static class Log
{
    static StreamWriter StreamWriter { get; set; }

    static Log()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "log", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.log");
        var dir = Path.GetDirectoryName(path);
        if (dir == null)
            throw new NullReferenceException("目录文件不存在!");
        Directory.CreateDirectory(dir);
        StreamWriter = new(path);
    }

    public static void OutPutConsole(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    public static void ConsoleError(string info)
    {
        OutPutConsole(info, ConsoleColor.Red);
        Writer(info, TraceLevel.Error);
    }

    public static void ConsoleInfo(string info, ConsoleColor color = ConsoleColor.Gray)
    {
        OutPutConsole(info, color);
        Writer(info, TraceLevel.Info);
    }

    public static void ConsoleWarn(string info)
    {
        OutPutConsole(info, ConsoleColor.Yellow);
        Writer(info, TraceLevel.Warning);
    }

    public static void Dispose()
    {
        StreamWriter.Dispose();
    }

    public static void Error(string info)
    {
        Writer(info, TraceLevel.Error);
    }

    public static void Info(string info)
    {
        Writer(info, TraceLevel.Info);
    }

    public static void Warn(string info)
    {
        Writer(info, TraceLevel.Warning);
    }

    static void Writer(string info, TraceLevel level)
    {
        if (string.IsNullOrEmpty(info))
            return;
        var trace = new StackTrace();
        var frame = trace.GetFrame(2);
        if (frame != null)
        {
            var prefix = frame.GetMethod()?.DeclaringType?.Name;
            StreamWriter.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{prefix}] [{level.ToString().ToUpper()}]: {info}");
            StreamWriter.Flush();
        }
    }
}
