using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.ClientSimulation.Clients;

namespace Volo.ClientSimulation
{
    public class Simulation : ISingletonDependency, IDisposable
    {
        public GlobalOptions GlobalOptions { get; }

        public GlobalOptions CurrentOptions { get; private set; }

        public SimulationState State { get; private set; }

        public List<IClient> ActiveClients { get; }

        private readonly IServiceScope _serviceScope;

        public Simulation(
            GlobalOptions globalOptions, 
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScope = serviceScopeFactory.CreateScope();
            GlobalOptions = globalOptions;
            ActiveClients = new List<IClient>();
        }

        public void Start()
        {
            State = SimulationState.Starting;

            CurrentOptions = GlobalOptions.Clone();

            lock (ActiveClients)
            {
                ActiveClients.Clear();

                for (int i = 0; i < CurrentOptions.MaxClientCount; i++)
                {
                    ActiveClients.Add(_serviceScope.ServiceProvider.GetRequiredService<IClient>());
                }

                foreach (var activeClient in ActiveClients)
                {
                    activeClient.Stopped += ActiveClientOnStopped;
                    activeClient.Start();
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
                    activeClient.Stop();
                }
            }

            State = SimulationState.Stopped;
        }

        private void ActiveClientOnStopped(object sender, EventArgs e)
        {
            var client = (IClient) sender;

            lock (ActiveClients)
            {
                ActiveClients.Remove(client);
            }
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}
