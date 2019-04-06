using Volo.ClientSimulation.Scenarios;

namespace Volo.ClientSimulation.Clients
{
    public interface IClientFactory
    {
        IDisposableClientHandler Create(IScenario scenario);
    }
}
