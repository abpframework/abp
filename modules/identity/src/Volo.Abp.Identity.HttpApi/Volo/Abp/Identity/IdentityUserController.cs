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
    public class IdentityUserController : AbpController, IIdentityUserAppService
    {
        private readonly IIdentityUserAppService _userAppService;

        public IdentityUserController(IIdentityUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public virtual Task<IdentityUserDto> GetAsync(Guid id)
        {
            return _userAppService.GetAsync(id);
        }

        public virtual Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {
            return _userAppService.GetListAsync(input);
        }

        public virtual Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            return _userAppService.CreateAsync(input);
        }

        public virtual Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            return _userAppService.UpdateAsync(id, input);
        }

        public virtual Task DeleteAsync(Guid id)
        {
            return _userAppService.DeleteAsync(id);
        }

        public virtual Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
        {
            return _userAppService.GetRolesAsync(id);
        }

        public virtual Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
        {
            return _userAppService.UpdateRolesAsync(id, input);
        }

        [HttpGet]
        public virtual Task<IdentityUserDto> FindByUsernameAsync(string username)
        {
            return _userAppService.FindByUsernameAsync(username);
        }

        [HttpGet]
        public virtual Task<IdentityUserDto> FindByEmailAsync(string email)
        {
            return _userAppService.FindByEmailAsync(email);
        }
    }
}
