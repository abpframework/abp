using Volo.ClientSimulation.Scenarios;

namespace Volo.ClientSimulation.Clients
{
    public interface IClient
    {
        Scenario Scenario { get; }

        ClientState State { get; }

        void Initialize(Scenario scenario);

        void Start();

        void Stop();
    }
}