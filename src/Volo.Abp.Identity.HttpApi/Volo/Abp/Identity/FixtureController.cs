using Microsoft.AspNetCore.Mvc;
using Volo.Abp.ApiVersioning;
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
        private readonly IRequestedApiVersion _requestedApiVersion;

        public FixtureController(IRequestedApiVersion requestedApiVersion)
        {
            _requestedApiVersion = requestedApiVersion;
        }

        [HttpGet, MapToApiVersion("2.0")]
        public string Get()
        {
            return 41 + " - " + _requestedApiVersion.Current;
        }

        [HttpGet, MapToApiVersion("3.0")]
        public string Get3()
        {
            return 42 + " - " + _requestedApiVersion.Current;
        }

        [HttpPost]
        public int Post()
        {
            return 43;
        }
    }
}