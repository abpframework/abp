using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Permissions;

namespace Volo.Abp.Identity
{
    [RemoteService]
    [Area("identity")]
    [Controller]
    [ControllerName("User")]
    public class IdentityUserController : IIdentityUserAppService, ITransientDependency //TODO: Try to implement these type wrapper controllers automatically!
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

        public virtual Task<GetPermissionListResultDto> GetPermissionsAsync(Guid id)
        {
            return _userAppService.GetPermissionsAsync(id);
        }

        public virtual Task UpdatePermissionsAsync(Guid id, UpdatePermissionsDto input)
        {
            return _userAppService.UpdatePermissionsAsync(id, input);
        }
    }
}
