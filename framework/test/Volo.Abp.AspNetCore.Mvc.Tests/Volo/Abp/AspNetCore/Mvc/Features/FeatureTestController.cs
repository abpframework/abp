using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Features;

namespace Volo.Abp.AspNetCore.Mvc.Features;

[Route("api/feature-test")]
public class FeatureTestController : AbpController
{
    [HttpGet]
    [Route("allowed-feature")]
    [RequiresFeature("AllowedFeature")]
    public Task AllowedFeatureAsync()
    {
        return Task.CompletedTask;
    }

    [HttpGet]
    [Route("not-allowed-feature")]
    [RequiresFeature("NotAllowedFeature")]
    public void NotAllowedFeature()
    {

    }

    [HttpGet]
    [Route("no-feature")]
    public int NoFeature()
    {
        return 42;
    }
}
