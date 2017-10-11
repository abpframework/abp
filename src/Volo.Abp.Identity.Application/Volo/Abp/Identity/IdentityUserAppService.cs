using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity
{
    //TODO: Consider a way of passing cancellation token to all async application service methods!

    public class IdentityUserAppService : ApplicationService, IIdentityUserAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IIdentityRoleRepository _roleRepository;

        public IdentityUserAppService(IdentityUserManager userManager, IIdentityUserRepository userRepository, IIdentityRoleRepository roleRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<IdentityUserDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
                await _userManager.GetByIdAsync(id)
            );
        }

        public async Task<IdentityUserCreateOrUpdateOutput> GetUserForCreateOrUpdateAsync(Guid? id)
        {
            var userRoleDtos = ObjectMapper.Map<List<IdentityRole>, IdentityUserRoleDto[]>(await _roleRepository.GetListAsync());
            var output = new IdentityUserCreateOrUpdateOutput
            {
                Roles = userRoleDtos
            };

            if (!id.HasValue) return output;

            var user = await _userManager.GetByIdAsync(id.Value);
            output.User = ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);

            foreach (var userRoleDto in userRoleDtos)
            {
                userRoleDto.IsAssigned = await _userManager.IsInRoleAsync(user, userRoleDto.Name);
            }

            return output;
        }

        public async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {
            var count = (int)await _userRepository.GetCountAsync();
            var list = await _userRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

            return new PagedResultDto<IdentityUserDto>(
                count,
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list)
            );
        }

        public async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            var user = new IdentityUser(GuidGenerator.Create(), input.UserName);

            await UpdateUserByInput(user, input);
            await _userManager.AddPasswordAsync(user, input.Password);
            await _userManager.CreateAsync(user);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            var user = await _userManager.GetByIdAsync(id);

            await _userManager.SetUserNameAsync(user, input.UserName);
            await UpdateUserByInput(user, input);
            await _userManager.UpdateAsync(user);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userManager.GetByIdAsync(id);
            await _userManager.DeleteAsync(user);
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
            await _userManager.SetRolesAsync(user, input.RoleNames);
        }

        private async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
        {
            await _userManager.SetEmailAsync(user, input.Email);
            await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber);
            await _userManager.SetTwoFactorEnabledAsync(user, input.TwoFactorEnabled);
            await _userManager.SetLockoutEnabledAsync(user, input.LockoutEnabled);

            if (input.RoleNames != null)
            {
                await _userManager.SetRolesAsync(user, input.RoleNames);
            }
        }
    }
}
