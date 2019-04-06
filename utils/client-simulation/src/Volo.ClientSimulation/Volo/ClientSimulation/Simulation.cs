using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.ClientSimulation.Clients;

namespace Volo.ClientSimulation
{
    public class Simulation : ISingletonDependency
    {
        public GlobalOptions GlobalOptions { get; }

        public GlobalOptions CurrentOptions { get; private set; }

        public SimulationState State { get; private set; }

        public List<IDisposableClientHandler> ActiveClients { get; }

        protected IClientFactory ClientFactory { get; }

        protected ClientSimulationOptions Options { get; }

        public Simulation(
            GlobalOptions globalOptions,
            IClientFactory clientFactory, 
            IOptions<ClientSimulationOptions> options)
        {
            GlobalOptions = globalOptions;
            ClientFactory = clientFactory;
            Options = options.Value;
            ActiveClients = new List<IDisposableClientHandler>();
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
                    var selectedScenario = RandomHelper.GetRandomOfList(Options.Scenarios);
                    ActiveClients.Add(ClientFactory.Create(selectedScenario));
                }

                foreach (var activeClient in ActiveClients)
                {
                    activeClient.Client.Stopped += ActiveClientOnStopped;
                    activeClient.Client.Start();
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
    }
}
