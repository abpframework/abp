using System.Threading.Tasks;

namespace Volo.ClientSimulation.Scenarios
{
    public class SleepScenarioStep : ScenarioStep
    {
        public int Duration { get; }

        public SleepScenarioStep(int duration = 1000)
        {
            Duration = duration;
        }

        protected override Task ExecuteAsync()
        {
            return Task.Delay(Duration);
        }

        public override string GetDisplayText()
        {
            return base.GetDisplayText() + $" ({Duration})";
        }
    }
}