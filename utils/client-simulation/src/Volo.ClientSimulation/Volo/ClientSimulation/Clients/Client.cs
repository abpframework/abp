using System;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.ClientSimulation.Clients
{
    public class Client : IClient, ITransientDependency
    {
        public event EventHandler Stopped;

        public ClientState State { get; private set; }

        private Thread _thread;

        public Client()
        {
            
        }

        public void Start()
        {
            if (State != ClientState.Stopped)
            {
                throw new ApplicationException("State is not stopped. It is " + State);
            }

            State = ClientState.Running;

            _thread = new Thread(Run);
            _thread.Start();
        }

        public void Stop()
        {
            State = ClientState.Stopping;
        }

        private void Run()
        {
            while (State == ClientState.Running)
            {
                Thread.Sleep(1);
            }

            State = ClientState.Stopped;
            Stopped.InvokeSafely(this);
        }
    }
}