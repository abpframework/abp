using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace DistDemoApp;

[ApiController]
[Route("api/dist-demo/dapr")]
public class ProviderScenarioController : AbpController
{
    private readonly IDistEventScenarioRunner _scenarioRunner;

    public ProviderScenarioController(IDistEventScenarioRunner scenarioRunner)
    {
        _scenarioRunner = scenarioRunner;
    }

    [HttpGet]
    public async Task<IActionResult> RunAsync()
    {
        await _scenarioRunner.RunAsync(DistEventScenarioProfile.DaprWeb());
        return Ok(new { Status = "ScenarioCompleted", Profile = "dapr-web" });
    }
}
