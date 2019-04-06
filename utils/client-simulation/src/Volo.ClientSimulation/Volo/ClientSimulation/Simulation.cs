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

        public Simulation(
            IClientFactory clientFactory, 
            IServiceScopeFactory serviceScopeFactory,
            IOptions<ClientSimulationOptions> options)
        {
            ClientFactory = clientFactory;
            Options = options.Value;
            ServiceScope = serviceScopeFactory.CreateScope();
            ActiveClients = new List<IDisposableClientHandler>();
        }

        public void Start()
        {
            State = SimulationState.Starting;

            lock (ActiveClients)
            {
                ActiveClients.Clear();

                foreach (var scenarioConfiguration in Options.Scenarios)
                {
                    for (int i = 0; i < scenarioConfiguration.ClientCount; i++)
                    {
                        var scenario = (IScenario) ServiceScope.ServiceProvider.GetRequiredService(scenarioConfiguration.ScenarioType);
                        var clientHandler = ClientFactory.Create(scenario);

                        ActiveClients.Add(clientHandler);
                        clientHandler.Client.Stopped += ActiveClientOnStopped;
                        clientHandler.Client.Start();
                    }
                }
            }

            State = SimulationState.Started;
        }

        public void Stop()
        {
            State = SimulationState.Stopping;

            lock (ActiveClients)
            {
                foreach (var activeClient in ActiveClients)
                {
                    activeClient.Client.Stop();
                }
            }

            State = SimulationState.Stopped;
        }

        private void ActiveClientOnStopped(object sender, EventArgs e)
        {
            var client = (IClient) sender;

            lock (ActiveClients)
            {
                ActiveClients.RemoveAll(c => c.Client == client);
            }
        }

        public void Dispose()
        {
            ServiceScope.Dispose();
        }
    }
}
