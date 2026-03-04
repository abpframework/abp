using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace DistDemoApp
{
    public class DemoService : ITransientDependency
    {
        private readonly IDistEventScenarioRunner _scenarioRunner;

        public DemoService(IDistEventScenarioRunner scenarioRunner)
        {
            _scenarioRunner = scenarioRunner;
        }

        public virtual async Task CreateTodoItemAsync()
        {
            await _scenarioRunner.RunAsync(DistEventScenarioProfile.Default());
        }
    }
}