using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Http.DynamicProxying
{
    [RemoteService] //Automatically enables API explorer and apply ABP conventions.
    //[ApiExplorerSettings(IgnoreApi = false)] //alternative
    public class RegularTestController : AbpController, IRegularTestController
    {
        [HttpGet]
        [Route("api/regular-test-controller/{value}")]
        public int IncrementValue(int value)
        {
            return value + 1;
        }
    }
}