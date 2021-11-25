using Microsoft.AspNetCore.Mvc;
using Volo.Abp.GlobalFeatures;

namespace Volo.Abp.AspNetCore.Mvc.GlobalFeatures;

[RequiresGlobalFeature("Not-Enabled-Feature")]
[Route("api/DisabledGlobalFeatureTestController-Test")]
public class DisabledGlobalFeatureTestController : AbpController
{
    [HttpGet]
    [Route("TestMethod")]
    public string TestMethod()
    {
        return "TestMethod";
    }
}
