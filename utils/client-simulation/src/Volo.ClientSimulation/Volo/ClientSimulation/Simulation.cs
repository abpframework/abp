using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.ClientSimulation
{
    public class Simulation : ISingletonDependency
    {
        public SimulationState State { get; private set; }

        public Task StartAsync()
        {
            State = SimulationState.Starting;
            State = SimulationState.Started;

            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            State = SimulationState.Stopping;
            State = SimulationState.Stopped;

            return Task.CompletedTask;
        }
    }
}
