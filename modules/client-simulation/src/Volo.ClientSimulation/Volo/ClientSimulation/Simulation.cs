using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.ClientSimulation.Clients;
using Volo.ClientSimulation.Scenarios;

namespace Volo.ClientSimulation
{
    public class Simulation : ISingletonDependency, IDisposable
    {
        public SimulationState State { get; private set; }

        public List<IClient> Clients { get; }

        protected ClientSimulationOptions Options { get; }

        protected IServiceScope ServiceScope { get; }

        protected readonly object SyncObj = new object();

        public Simulation(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<ClientSimulationOptions> options)
        {
            Options = options.Value;
            ServiceScope = serviceScopeFactory.CreateScope();

            Clients = new List<IClient>();

            foreach (var scenarioConfiguration in Options.Scenarios)
            {
                for (int i = 0; i < scenarioConfiguration.ClientCount; i++)
                {
                    var scenario = (Scenario) ServiceScope.ServiceProvider.GetRequiredService(
                        scenarioConfiguration.ScenarioType
                    );

                    var client = ServiceScope.ServiceProvider.GetRequiredService<IClient>();
                    client.Initialize(scenario);
                    Clients.Add(client);
                }
            }
        }

        public void Start()
        {
            lock (SyncObj)
            {
                if (State != SimulationState.Stopped)
                {
                    throw new UserFriendlyException($"Simulation should be stopped to be able to start. Current state is '{State}'.");
                }

                State = SimulationState.Starting;

                foreach (var client in Clients)
                {
                    client.Start();
                }

                State = SimulationState.Started;
            }
        }

        public void Stop()
        {
            lock (SyncObj)
            {
                if (State != SimulationState.Started)
                {
                    throw new UserFriendlyException($"Simulation should be started to be able to stop. Current state is '{State}'.");
                }

                State = SimulationState.Stopping;

                foreach (var client in Clients)
                {
                    client.Stop();
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
