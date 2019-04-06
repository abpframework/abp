using System.Threading;

namespace Volo.ClientSimulation.Scenarios
{
    public class SleepScenarioStep : ScenarioStep
    {
        public int Duration { get; }

        public SleepScenarioStep(int duration = 1000)
        {
            Duration = duration;
        }

        public override void Run()
        {
            Thread.Sleep(Duration);
        }

        public override string GetDisplayText()
        {
            return base.GetDisplayText() + $"({Duration})";
        }
    }
}