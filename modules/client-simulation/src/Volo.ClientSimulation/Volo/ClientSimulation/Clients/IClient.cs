using System;
using Volo.ClientSimulation.Scenarios;
using Volo.ClientSimulation.Snapshot;

namespace Volo.ClientSimulation.Clients
{
    public interface IClient
    {
        event EventHandler Stopped;

        ClientState State { get; }

        void Initialize(Scenario scenario);

        void Start();

        void Stop();

        ClientSnapshot CreateSnapshot();
    }
}