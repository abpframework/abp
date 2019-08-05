using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;
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

            if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled))
            {
                (await _userManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
            }

            if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsEmailUpdateEnabled))
            {
                (await _userManager.SetEmailAsync(user, input.Email)).CheckErrors();
            }

            (await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();

            user.Name = input.Name;
            user.Surname = input.Surname;

            (await _userManager.UpdateAsync(user)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, ProfileDto>(user);
        }

        public async Task ChangePasswordAsync(ChangePasswordInput input)
        {
            var currentUser = await _userManager.GetByIdAsync(CurrentUser.GetId());
            (await _userManager.ChangePasswordAsync(currentUser, input.CurrentPassword, input.NewPassword)).CheckErrors();
        }
    }
}
