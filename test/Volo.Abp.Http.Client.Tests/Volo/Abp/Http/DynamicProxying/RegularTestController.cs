using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Http.DynamicProxying
{
    [Route("api/regular-test-controller")]
    [RemoteService] //Automatically enables API explorer and apply ABP conventions.
    //[ApiExplorerSettings(IgnoreApi = false)] //alternative
    public class RegularTestController : AbpController, IRegularTestController
    {
        [HttpGet]
        [Route("increment/{value}")]
        public int IncrementValue(int value)
        {
            return value + 1;
        }
    }
}