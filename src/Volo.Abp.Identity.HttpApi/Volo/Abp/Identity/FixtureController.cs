using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity
{
    //TODO: This is just a test controller and will be removed later
    [ApiVersion("3.0")]
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/v{api-version:apiVersion}/identity/fixture")]
    public class FixtureController : AbpController, IRemoteService
    {
        [HttpGet]
        public int Get()
        {
            return 42;
        }

        //[HttpGet, MapToApiVersion("3.0")]
        //public int Get3()
        //{
        //    return 42;
        //}

        [HttpPost]
        public int Post()
        {
            return 42;
        }
    }
}