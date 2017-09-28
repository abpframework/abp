using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.HttpApi.Host.VersioningTests
{
    [Route("api/calls")]
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
}
