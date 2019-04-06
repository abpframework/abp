using Volo.ClientSimulation.Scenarios;

namespace Volo.ClientSimulation.Clients
{
    public interface IClient
    {
        IScenario Scenario { get; }

        ClientState State { get; }

        void Start();

        void Stop();
    }
}