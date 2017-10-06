using Microsoft.AspNetCore.Mvc;
using Volo.Abp.ApiVersioning;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.V4
{
    //TODO: This is just a test controller and will be removed later
    [ApiVersion("4.0")]
    [Route("api/v{api-version:apiVersion}/identity/fixture")]
    public class FixtureController : AbpController, IRemoteService
    {
        private readonly IRequestedApiVersion _requestedApiVersion;

        public FixtureController(IRequestedApiVersion requestedApiVersion)
        {
            _requestedApiVersion = requestedApiVersion;
        }

        [HttpGet]
        public string Get()
        {
            return 41 + " - " + _requestedApiVersion.Current;
        }
    }
}