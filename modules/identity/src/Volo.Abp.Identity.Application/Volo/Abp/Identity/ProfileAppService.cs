using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    [Authorize]
    public class ProfileAppService : IdentityAppServiceBase, IProfileAppService
    {
        private readonly IdentityUserManager _userManager;

        public ProfileAppService(IdentityUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<ProfileDto> GetAsync()
        {
            return ObjectMapper.Map<IdentityUser, ProfileDto>(
                await _userManager.GetByIdAsync(CurrentUser.GetId())
            );
        }

        public async Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
        {
            var user = await _userManager.GetByIdAsync(CurrentUser.GetId());

            (await _userManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
            (await _userManager.SetEmailAsync(user, input.Email)).CheckErrors();
            (await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            user.Name = input.Name;
            user.Surname = input.Surname;

            (await _userManager.UpdateAsync(user)).CheckErrors();
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, ProfileDto>(user);
        }
    }
}
