using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity
{
    [RemoteService]
    [Area("identity")]
    [ControllerName("User")]
    [Route("api/identity/users")]
    public class IdentityUserController : AbpController, IIdentityUserAppService
    {
        private readonly IIdentityUserAppService _userAppService;

        public IdentityUserController(IIdentityUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<IdentityUserDto> GetAsync(Guid id)
        {
            return _userAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {
            return _userAppService.GetListAsync(input);
        }

        [HttpPost]
        public virtual Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            return _userAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            return _userAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _userAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}/roles")]
        public virtual Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
        {
            return _userAppService.GetRolesAsync(id);
        }

        [HttpPut]
        [Route("{id}/roles")]
        public virtual Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
        {
            return _userAppService.UpdateRolesAsync(id, input);
        }

        [HttpGet]
        [Route("by-username/{userName}")]
        public virtual Task<IdentityUserDto> FindByUsernameAsync(string username)
        {
            return _userAppService.FindByUsernameAsync(username);
        }

        [HttpGet]
        [Route("by-email/{email}")]
        public virtual Task<IdentityUserDto> FindByEmailAsync(string email)
        {
            return _userAppService.FindByEmailAsync(email);
        }
    }
}
