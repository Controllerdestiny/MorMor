using System.Diagnostics;

namespace MomoAPI.Interface;

public interface ILog : IDisposable
{
    void Writer(string info, TraceLevel level);

    void Error(string info);

    void Warn(string info);

    void Info(string info);

    void ConsoleInfo(string info, ConsoleColor color);

    void ConsoleWarn(string info);

    void ConsoleError(string info);
    new void Dispose();
}
