using System;
using System.Threading;
using Volo.ClientSimulation.Scenarios;

namespace Volo.ClientSimulation.Clients
{
    public class Client : IClient
    {
        public IScenario Scenario { get; }

        public event EventHandler Stopped;

        public ClientState State
        {
            get => _state;
            private set => _state = value;
        }
        private volatile ClientState _state;

        private Thread _thread;

        protected readonly object SyncLock = new object();

        public Client(IScenario scenario)
        {
            Scenario = scenario;
        }

        public void Start()
        {
            lock (SyncLock)
            {
                if (State != ClientState.Stopped)
                {
                    throw new ApplicationException("State is not stopped. It is " + State);
                }

                State = ClientState.Running;

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

        private void Run()
        {
            while (State == ClientState.Running)
            {
                Scenario.Proceed();
            }

            State = ClientState.Stopped;
            Stopped.InvokeSafely(this);
        }
    }
}