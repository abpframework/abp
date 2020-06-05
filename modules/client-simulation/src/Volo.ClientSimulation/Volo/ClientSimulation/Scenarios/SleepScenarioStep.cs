using System.Threading.Tasks;

namespace Volo.ClientSimulation.Scenarios
{
    public class SleepScenarioStep : ScenarioStep
    {
        public string Name { get; }

        public int Duration { get; }

        public SleepScenarioStep(
            string name, 
            int duration = 1000)
        {
            Name = name;
            Duration = duration;
        }

        protected override Task ExecuteAsync(ScenarioExecutionContext context)
        {
            return Task.Delay(Duration);
        }

        public override string GetDisplayText()
        {
            return base.GetDisplayText() + $" ({Name})";
        }
    }
}