using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.HttpApi.Host.VersioningTests.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{apiVersion:apiVersion}/calls")]
    public class CallsController : AbpController, IRemoteService
    {
        private static readonly List<CallDto> Calls = new List<CallDto>
        {
            new CallDto {Id = 1, Number = "123456"},
            new CallDto { Id = 2, Number = "123457" }
        };

        [HttpGet]
        public List<CallDto> GetList()
        {
            return Calls;
        }
    }
}
