using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity
{
    [Route("api/identity/users")]
    public class IdentityUsersController : AbpController
    {
        private readonly IUserAppService _userAppService;

        public IdentityUsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpGet]
        [Route("")]
        public Task<ListResultDto<IdentityUserDto>> Get()
        {
            return _userAppService.Get();
        }

        [HttpGet]
        [Route("{id}")]
        public Task<IdentityUserDto> Get(Guid id)
        {
            return _userAppService.Get(id);
        }
    }
}
