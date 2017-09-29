using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.HttpApi.Host.VersioningTests.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/calls")]
    public class CallsController : AbpController
    {
        private static List<CallDto> _calls = new List<CallDto>
        {
            new CallDto {Id = 1, Number = "123456"},
            new CallDto { Id = 2, Number = "123457" }
        };

        [HttpGet]
        public List<CallDto> GetList()
        {
            return _calls;
        }
    }

    public class CallDto : EntityDto<int>
    {
        public string Number { get; set; }
    }

    [ApiVersion("2.0")]
    [Route("api/v{api-version:apiVersion}/calls")]
    [ControllerName("Calls")]
    public class Calls2Controller : AbpController
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
