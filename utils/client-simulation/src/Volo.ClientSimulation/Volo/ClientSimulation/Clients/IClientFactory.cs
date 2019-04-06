using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.ClientSimulation.Scenarios;

namespace Volo.ClientSimulation.Clients
{
    public interface IClientFactory
    {
        IDisposableClientHandler Create(IScenario scenario);
    }

    public class DefaultClientFactory : IClientFactory, ISingletonDependency
    {
        public IDisposableClientHandler Create(IScenario scenario)
        {
            return new DisposableClientHandler(new Client(scenario));
        }
    }
}
