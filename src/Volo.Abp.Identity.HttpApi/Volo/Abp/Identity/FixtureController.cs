using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity
{
    //TODO: This is just a test controller and will be removed lster
    [Route("api/identity/fixture")]
    [ApiVersion("2.0", Deprecated = true)]
    [ApiVersion("3.0")]
    public class FixtureController : AbpController, IRemoteService
    {
        [HttpGet]
        public int Get()
        {
            return 42;
        }

        [HttpGet, MapToApiVersion("3.0")]
        public int Get3()
        {
            return 42;
        }

        [HttpPost]
        public int Post()
        {
            return 42;
        }
    }
}