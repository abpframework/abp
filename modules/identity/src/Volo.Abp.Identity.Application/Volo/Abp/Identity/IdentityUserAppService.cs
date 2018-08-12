using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.PermissionManagement;

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
            var count = await _userRepository.GetCountAsync(); //TODO: 
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
            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.Email, CurrentTenant.Id);

            (await _userManager.CreateAsync(user, input.Password)).CheckErrors();
            await UpdateUserByInput(user, input);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            var user = await _userManager.GetByIdAsync(id);

            (await _userManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
            await UpdateUserByInput(user, input);
            (await _userManager.UpdateAsync(user)).CheckErrors();
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

            (await _userManager.DeleteAsync(user)).CheckErrors();
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
        {
            var user = await _userManager.GetByIdAsync(id);
            (await _userManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
            await _userRepository.UpdateAsync(user);
        }

        [Authorize(IdentityPermissions.Users.ManagePermissions)]
        public async Task<GetPermissionListResultDto> GetPermissionsAsync(Guid id)
        {
            var user = await _userManager.GetByIdAsync(id);
            var result = await _permissionAppServiceHelper.GetAsync(UserPermissionValueProvider.ProviderName, id.ToString());
            result.EntityDisplayName = user.UserName;
            return result;
        }

        [Authorize(IdentityPermissions.Users.ManagePermissions)]
        public async Task UpdatePermissionsAsync(Guid id, UpdatePermissionsDto input)
        {
            await _permissionAppServiceHelper.UpdateAsync(UserPermissionValueProvider.ProviderName, id.ToString(), input);
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public async Task<IdentityUserDto> FindByUsernameAsync(string username)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
                await _userManager.FindByNameAsync(username)
            );
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public async Task<IdentityUserDto> FindByEmailAsync(string email)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
                await _userManager.FindByEmailAsync(email)
            );
        }

        private async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
        {
            (await _userManager.SetEmailAsync(user, input.Email)).CheckErrors();
            (await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            (await _userManager.SetTwoFactorEnabledAsync(user, input.TwoFactorEnabled)).CheckErrors();
            (await _userManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();

            if (input.RoleNames != null)
            {
                (await _userManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
            }
        }
    }
}
