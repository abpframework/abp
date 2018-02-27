using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Permissions;

namespace Volo.Abp.Identity
{
    [Authorize(IdentityPermissions.Users.Default)]
    public class IdentityUserAppService : IdentityAppServiceBase, IIdentityUserAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IPermissionAppServiceHelper _permissionAppServiceHelper;

        public IdentityUserAppService(
            IdentityUserManager userManager, 
            IIdentityUserRepository userRepository,
            IPermissionAppServiceHelper permissionAppServiceHelper)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _permissionAppServiceHelper = permissionAppServiceHelper;
        }

        public async Task<IdentityUserDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
                await _userManager.GetByIdAsync(id)
            );
        }

        public async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {
            var count = await _userRepository.GetCountAsync();
            var list = await _userRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

            return new PagedResultDto<IdentityUserDto>(
                count,
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list)
            );
        }

        public async Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
        {
            var roles = await _userRepository.GetRolesAsync(id);
            return new ListResultDto<IdentityRoleDto>(
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(roles)
            );
        }

        [Authorize(IdentityPermissions.Users.Create)]
        public async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, CurrentTenant.Id);

            CheckIdentityErrors(await _userManager.CreateAsync(user, input.Password));
            await UpdateUserByInput(user, input);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            var user = await _userManager.GetByIdAsync(id);

            CheckIdentityErrors(await _userManager.SetUserNameAsync(user, input.UserName));
            await UpdateUserByInput(user, input);
            CheckIdentityErrors(await _userManager.UpdateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [Authorize(IdentityPermissions.Users.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return;
            }

            CheckIdentityErrors(await _userManager.DeleteAsync(user));
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
        {
            var user = await _userManager.GetByIdAsync(id);
            CheckIdentityErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
        }

        public async Task<GetPermissionListResultDto> GetPermissionsAsync(Guid id)
        {
            return await _permissionAppServiceHelper.GetAsync(UserPermissionValueProvider.ProviderName, id.ToString());
        }

        public async Task UpdatePermissionsAsync(Guid id, UpdatePermissionsDto input)
        {
            await _permissionAppServiceHelper.UpdateAsync(UserPermissionValueProvider.ProviderName, id.ToString(), input);
        }
        
        private async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
        {
            CheckIdentityErrors(await _userManager.SetEmailAsync(user, input.Email));
            CheckIdentityErrors(await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber));
            CheckIdentityErrors(await _userManager.SetTwoFactorEnabledAsync(user, input.TwoFactorEnabled));
            CheckIdentityErrors(await _userManager.SetLockoutEnabledAsync(user, input.LockoutEnabled));

            if (input.RoleNames != null)
            {
                CheckIdentityErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }
        }
    }
}
