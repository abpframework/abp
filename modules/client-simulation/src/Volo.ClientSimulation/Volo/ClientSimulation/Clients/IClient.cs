using System;
using Volo.ClientSimulation.Scenarios;
using Volo.ClientSimulation.Snapshot;

namespace Volo.ClientSimulation.Clients
{
    public interface IClient
    {
        event EventHandler Stopped;

        Scenario Scenario { get; }

        ClientState State { get; }

        void Initialize(Scenario scenario);

        void Start();

        void Stop();

        ClientSnapshot CreateSnapshot();
    }
}