using System;
using System.Threading;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.ClientSimulation.Scenarios;
using Volo.ClientSimulation.Snapshot;

namespace Volo.ClientSimulation.Clients
{
    public class Client : IClient, ITransientDependency
    {
        public event EventHandler Stopped;

        public Scenario Scenario { get; private set; }

        public ClientState State
        {
            get => _state;
            private set => _state = value;
        }
        private volatile ClientState _state;

        private Thread _thread;

        protected readonly object SyncLock = new object();

        public void Initialize(Scenario scenario)
        {
            lock (SyncLock)
            {
                if (State != ClientState.Stopped)
                {
                    throw new UserFriendlyException($"Client should be stopped to be able to initialize it. Current state is '{State}'.");
                }

                Scenario = scenario;
            }
        }

        public void Start()
        {
            lock (SyncLock)
            {
                if (State != ClientState.Stopped)
                {
                    throw new UserFriendlyException($"Client should be stopped to be able to start it. Current state is '{State}'.");
                }

                State = ClientState.Running;

                Scenario.Reset();
                _thread = new Thread(Run);
                _thread.Start();
            }
        }

        public void Stop()
        {
            lock (SyncLock)
            {
                if (State != ClientState.Running)
                {
                    return;
                }

                State = ClientState.Stopping;
            }
        }

        public ClientSnapshot CreateSnapshot()
        {
            lock (SyncLock)
            {
                return new ClientSnapshot
                {
                    State = State,
                    Scenario = Scenario.CreateSnapshot()
                };
            }
        }

        private void Run()
        {
            while (true)
            {
                lock (SyncLock)
                {
                    if (State != ClientState.Running)
                    {
                        State = ClientState.Stopped;
                        _thread = null;
                        Stopped.InvokeSafely(this);
                        break;
                    }
                }

                AsyncHelper.RunSync(() => Scenario.ProceedAsync());
            }
        }
    }
}