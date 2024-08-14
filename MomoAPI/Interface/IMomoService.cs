using MomoAPI.Adapter;

namespace MomoAPI.Interface;

public interface IMomoService : IDisposable
{
    EventAdapter Event { get; }

    public ValueTask<IMomoService> Start();
}
