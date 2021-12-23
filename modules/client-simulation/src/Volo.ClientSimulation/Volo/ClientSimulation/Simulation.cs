using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.ClientSimulation.Clients;
using Volo.ClientSimulation.Scenarios;
using Volo.ClientSimulation.Snapshot;

namespace Volo.ClientSimulation;

public class Simulation : ISingletonDependency, IDisposable
{
    public ILogger<Simulation> Logger { get; set; }

    public SimulationState State {
        get => _state;
        private set => _state = value;
    }
    private volatile SimulationState _state;

    public List<IClient> Clients { get; }

    protected ClientSimulationOptions Options { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IServiceScope ServiceScope { get; private set; }
    protected object SyncObj { get; } = new object();

    public Simulation(
        IServiceScopeFactory serviceScopeFactory,
        IOptions<ClientSimulationOptions> options)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Options = options.Value;

        Logger = NullLogger<Simulation>.Instance;

        Clients = new List<IClient>();
    }

    public virtual void Start()
    {
        lock (SyncObj)
        {
            if (State != SimulationState.Stopped)
            {
                throw new UserFriendlyException($"Simulation should be stopped to be able to start. Current state is '{State}'.");
            }

            State = SimulationState.Starting;

            try
            {
                DisposeResources();
                ServiceScope = ServiceScopeFactory.CreateScope();

                foreach (var scenarioConfiguration in Options.Scenarios)
                {
                    for (int i = 0; i < scenarioConfiguration.ClientCount; i++)
                    {
                        var scenario = (Scenario)ServiceScope.ServiceProvider.GetRequiredService(
                            scenarioConfiguration.ScenarioType
                        );

                        var client = ServiceScope.ServiceProvider.GetRequiredService<IClient>();
                        client.Stopped += Client_OnStopped;
                        client.Initialize(scenario);
                        Clients.Add(client);
                    }
                }

                foreach (var client in Clients)
                {
                    client.Start();
                }

                State = SimulationState.Started;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                State = SimulationState.Stopped;
            }
        }
    }

    public virtual void Stop()
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
        }
    }

    public virtual SimulationSnapshot CreateSnapshot()
    {
        SimulationSnapshot snapshot;

        lock (SyncObj)
        {
            snapshot = new SimulationSnapshot
            {
                State = State,
                Clients = Clients
                    .Select(c => c.CreateSnapshot())
                    .ToList()
            };
        }

        snapshot.CreateSummaries();

        return snapshot;
    }

    public virtual void Dispose()
    {
        DisposeResources();
    }

    protected virtual void Client_OnStopped(object sender, EventArgs e)
    {
        lock (SyncObj)
        {
            if (Clients.All(c => c.State == ClientState.Stopped))
            {
                OnStopped();
            }
        }
    }

    private void OnStopped()
    {
        State = SimulationState.Stopped;
    }

    private void DisposeResources()
    {
        Clients.Clear();
        ServiceScope?.Dispose();
    }
}
