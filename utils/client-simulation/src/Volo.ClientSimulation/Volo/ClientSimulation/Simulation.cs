using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.ClientSimulation.Clients;
using Volo.ClientSimulation.Scenarios;

namespace Volo.ClientSimulation
{
    public class Simulation : ISingletonDependency, IDisposable
    {
        public SimulationState State { get; private set; }

        public List<IDisposableClientHandler> ActiveClients { get; }

        protected IClientFactory ClientFactory { get; }

        protected ClientSimulationOptions Options { get; }

        protected IServiceScope ServiceScope { get; }

        protected readonly object SyncObj = new object();

        public Simulation(
            IClientFactory clientFactory, 
            IServiceScopeFactory serviceScopeFactory,
            IOptions<ClientSimulationOptions> options)
        {
            ClientFactory = clientFactory;
            Options = options.Value;
            ServiceScope = serviceScopeFactory.CreateScope();
            ActiveClients = new List<IDisposableClientHandler>();

            foreach (var scenarioConfiguration in Options.Scenarios)
            {
                for (int i = 0; i < scenarioConfiguration.ClientCount; i++)
                {
                    var scenario = (IScenario) ServiceScope.ServiceProvider.GetRequiredService(
                        scenarioConfiguration.ScenarioType
                    );

                    ActiveClients.Add(ClientFactory.Create(scenario));
                }
            }
        }

        public void Start()
        {
            lock (SyncObj)
            {
                State = SimulationState.Starting;

                foreach (var clientHandler in ActiveClients)
                {
                    clientHandler.Client.Start();
                }

                State = SimulationState.Started;
            }
        }

        public void Stop()
        {
            lock (SyncObj)
            {
                State = SimulationState.Stopping;

                foreach (var activeClient in ActiveClients)
                {
                    activeClient.Client.Stop();
                }

                State = SimulationState.Stopped;
            }
        }

        public void Dispose()
        {
            ServiceScope.Dispose();
        }
    }
}
