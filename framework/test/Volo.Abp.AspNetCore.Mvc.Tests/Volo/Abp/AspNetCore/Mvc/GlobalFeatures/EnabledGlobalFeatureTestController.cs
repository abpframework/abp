using Microsoft.AspNetCore.Mvc;
using Volo.Abp.GlobalFeatures;

namespace Volo.Abp.AspNetCore.Mvc.GlobalFeatures
{
    [RequiresGlobalFeature(AbpAspNetCoreMvcTestFeature1.Name)]
    [Route("api/EnabledGlobalFeatureTestController-Test")]
    public class EnabledGlobalFeatureTestController : AbpController
    {
        [HttpGet]
        [Route("TestMethod")]
        public string TestMethod()
        {
            return "TestMethod";
        }
    }
}
