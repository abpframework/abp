using System;
using System.Collections.Generic;
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

        public async Task<PagedResultDto<IdentityUserDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var userCount = (int) await _userRepository.GetCountAsync();
            var userDtos = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(
                await _userRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount)
            );

            return new PagedResultDto<IdentityUserDto>(userCount, userDtos);
        }

        public async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            var user = new IdentityUser(GuidGenerator.Create(), input.UserName);

            await UpdateUserProperties(input, user);
            await _userManager.AddPasswordAsync(user, input.Password);
            await _userManager.CreateAsync(user);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            var user = await _userManager.GetByIdAsync(id);

            await _userManager.SetUserNameAsync(user, input.UserName);
            await UpdateUserProperties(input, user);
            await _userManager.UpdateAsync(user);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userManager.GetByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        private async Task UpdateUserProperties(IdentityUserCreateOrUpdateDtoBase input, IdentityUser user)
        {
            await _userManager.SetEmailAsync(user, input.Email);
            await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber);
            await _userManager.SetTwoFactorEnabledAsync(user, input.TwoFactorEnabled);
            await _userManager.SetLockoutEnabledAsync(user, input.LockoutEnabled);
        }
    }
}
