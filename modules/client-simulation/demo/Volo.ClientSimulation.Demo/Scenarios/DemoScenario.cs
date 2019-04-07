using Volo.Abp;
using Volo.ClientSimulation.Scenarios;

namespace Volo.ClientSimulation.Demo.Scenarios
{
    public class DemoScenario : Scenario
    {
        public DemoScenario()
        {
            AddStep(new SleepScenarioStep(RandomHelper.GetRandom(1000, 5000)));
            AddStep(new SleepScenarioStep(RandomHelper.GetRandom(2000, 6000)));
        }
    }
}