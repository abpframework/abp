using System;

namespace Volo.ClientSimulation.Clients
{
    public interface IDisposableClientHandler : IDisposable
    {
        IClient Client { get; }
    }

    public class DisposableClientHandler : IDisposableClientHandler
    {
        public IClient Client { get; }

        public DisposableClientHandler(IClient client)
        {
            Client = client;
        }
        
        public virtual void Dispose()
        {

        }
    }
}