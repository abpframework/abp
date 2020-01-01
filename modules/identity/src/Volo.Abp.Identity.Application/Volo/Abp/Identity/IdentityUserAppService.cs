using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity
{
    public class IdentityUserAppService : IdentityAppServiceBase, IIdentityUserAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly IIdentityUserRepository _userRepository;

        public IdentityUserAppService(
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        //TODO: [Authorize(IdentityPermissions.Users.Default)] should go the IdentityUserAppService class.
        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<IdentityUserDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
                await _userManager.GetByIdAsync(id)
.ConfigureAwait(false));
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {
            var count = await _userRepository.GetCountAsync(input.Filter).ConfigureAwait(false);
            var list = await _userRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter).ConfigureAwait(false);

            return new PagedResultDto<IdentityUserDto>(
                count,
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list)
            );
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
        {
            var roles = await _userRepository.GetRolesAsync(id).ConfigureAwait(false);
            return new ListResultDto<IdentityRoleDto>(
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(roles)
            );
        }

        [Authorize(IdentityPermissions.Users.Create)]
        public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.Email, CurrentTenant.Id);

            (await _userManager.CreateAsync(user, input.Password).ConfigureAwait(false)).CheckErrors();
            await UpdateUserByInput(user, input).ConfigureAwait(false);

            await CurrentUnitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            var user = await _userManager.GetByIdAsync(id).ConfigureAwait(false);
            user.ConcurrencyStamp = input.ConcurrencyStamp;

            (await _userManager.SetUserNameAsync(user, input.UserName).ConfigureAwait(false)).CheckErrors();
            await UpdateUserByInput(user, input).ConfigureAwait(false);
            (await _userManager.UpdateAsync(user).ConfigureAwait(false)).CheckErrors();

            if (!input.Password.IsNullOrEmpty())
            {
                (await _userManager.RemovePasswordAsync(user).ConfigureAwait(false)).CheckErrors();
                (await _userManager.AddPasswordAsync(user, input.Password).ConfigureAwait(false)).CheckErrors();
            }

            await CurrentUnitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [Authorize(IdentityPermissions.Users.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            if (CurrentUser.Id == id)
            {
                throw new BusinessException(code: IdentityErrorCodes.UserSelfDeletion);
            }

            var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
            if (user == null)
            {
                return;
            }

            (await _userManager.DeleteAsync(user).ConfigureAwait(false)).CheckErrors();
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
        {
            var user = await _userManager.GetByIdAsync(id).ConfigureAwait(false);
            (await _userManager.SetRolesAsync(user, input.RoleNames).ConfigureAwait(false)).CheckErrors();
            await _userRepository.UpdateAsync(user).ConfigureAwait(false);
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<IdentityUserDto> FindByUsernameAsync(string username)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
                await _userManager.FindByNameAsync(username)
.ConfigureAwait(false));
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<IdentityUserDto> FindByEmailAsync(string email)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
                await _userManager.FindByEmailAsync(email)
.ConfigureAwait(false));
        }

        private async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
        {
            if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                (await _userManager.SetEmailAsync(user, input.Email).ConfigureAwait(false)).CheckErrors();
            }

            if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                (await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber).ConfigureAwait(false)).CheckErrors();
            }

            (await _userManager.SetTwoFactorEnabledAsync(user, input.TwoFactorEnabled).ConfigureAwait(false)).CheckErrors();
            (await _userManager.SetLockoutEnabledAsync(user, input.LockoutEnabled).ConfigureAwait(false)).CheckErrors();

            user.Name = input.Name;
            user.Surname = input.Surname;

            if (input.RoleNames != null)
            {
                (await _userManager.SetRolesAsync(user, input.RoleNames).ConfigureAwait(false)).CheckErrors();
            }
        }
    }
}
