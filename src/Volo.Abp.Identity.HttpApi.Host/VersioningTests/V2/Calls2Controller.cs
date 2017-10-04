using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity.HttpApi.Host.VersioningTests.V1;

namespace Volo.Abp.Identity.HttpApi.Host.VersioningTests.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{api-version:apiVersion}/calls")]
    public class CallsController : AbpController, IRemoteService
    {
        private static List<CallDto> _calls = new List<CallDto>
        {
            new CallDto {Id = 1, Number = "123456000"},
            new CallDto { Id = 2, Number = "123457000" }
        };

        [HttpGet]
        public List<CallDto> GetList()
        {
            return _calls;
        }

        [HttpGet]
        [Route("by-filter")]
        public List<CallDto> GetList(string num)
        {
            return _calls.Where(c => c.Number.Contains(num)).ToList();
        }
    }
}