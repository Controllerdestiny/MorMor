using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace TerrariaMap;

class IPCO
{
    public static (string, byte[]) Start(string name, byte[] buffer)
    {
        using NamedPipeClientStream pipeClient = new(".", name, PipeDirection.InOut);
        Console.WriteLine("尝试连接到服务器...");
        pipeClient.Connect();

        Console.WriteLine("已连接到服务器.");
        using BinaryWriter sw = new(pipeClient);
        sw.Write(buffer.Length);
        sw.Write(buffer);
        sw.Flush();

        byte[] responseBytes = new byte[1024 * 100];
        int bytesRead = pipeClient.Read(responseBytes, 0, responseBytes.Length);
        using var ms = new MemoryStream(responseBytes);
        using var br = new BinaryReader(ms);
        var worldN = br.ReadString();
        var count = br.ReadInt32();
        var worldB = br.ReadBytes(count);
        return (worldN, worldB);
    }
}
