using System;

namespace Volo.ClientSimulation.Clients
{
    public interface IClient
    {
        event EventHandler Stopped;

        ClientState State { get; }

        void Start();

        void Stop();
    }
}