using System.Threading.Tasks;

namespace DistDemoApp;

public interface IDistEventScenarioRunner
{
    Task RunAsync(DistEventScenarioProfile profile);
}
