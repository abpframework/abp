using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity
{
    public class IdentityUserAppService : IdentityAppServiceBase, IIdentityUserAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly IIdentityUserRepository _userRepository;

        public IdentityUserAppService(IdentityUserManager userManager, IIdentityUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
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

        public async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, CurrentTenant.Id);

            CheckIdentityErrors(await _userManager.CreateAsync(user, input.Password));
            await UpdateUserByInput(user, input);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            var user = await _userManager.GetByIdAsync(id);

            CheckIdentityErrors(await _userManager.SetUserNameAsync(user, input.UserName));
            await UpdateUserByInput(user, input);
            CheckIdentityErrors(await _userManager.UpdateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return;
            }

            CheckIdentityErrors(await _userManager.DeleteAsync(user));
        }

        public async Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
        {
            var roles = await _userRepository.GetRolesAsync(id);
            return new ListResultDto<IdentityRoleDto>(
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(roles)
            );
        }

        public async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
        {
            var user = await _userManager.GetByIdAsync(id);
            CheckIdentityErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
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
