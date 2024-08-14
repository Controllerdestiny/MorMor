global using MomoAPI.Log;
using System.Diagnostics;

namespace MomoAPI.Log;

static class Log
{
    static StreamWriter StreamWriter { get; set; }

    static Log()
    {
        var dir = Path.GetDirectoryName("Log");
        if (dir == null)
            throw new NullReferenceException("目录文件不存在!");
        Directory.CreateDirectory(dir);
        StreamWriter = new("Log");
    }

    static void OutPutConsole(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    static void ConsoleError(string info)
    {
        OutPutConsole(info, ConsoleColor.Red);
        Writer(info, TraceLevel.Error);
    }

    static void ConsoleInfo(string info, ConsoleColor color = ConsoleColor.Gray)
    {
        OutPutConsole(info, color);
        Writer(info, TraceLevel.Info);
    }

    static void ConsoleWarn(string info)
    {
        OutPutConsole(info, ConsoleColor.Yellow);
        Writer(info, TraceLevel.Warning);
    }

    static void Dispose()
    {
        StreamWriter.Dispose();
    }

    static void Error(string info)
    {
        Writer(info, TraceLevel.Error);
    }

    static void Info(string info)
    {
        Writer(info, TraceLevel.Info);
    }

    static void Warn(string info)
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
