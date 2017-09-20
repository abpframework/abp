using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Ui;

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

        [HttpGet]
        [Route("increment")]
        public Task<int> IncrementValueAsync(int value)
        {
            return Task.FromResult(value + 1);
        }

        [HttpGet]
        [Route("get-exception1")]
        public Task GetException1Async()
        {
            throw new UserFriendlyException("This is an error message!");
        }
    }
}