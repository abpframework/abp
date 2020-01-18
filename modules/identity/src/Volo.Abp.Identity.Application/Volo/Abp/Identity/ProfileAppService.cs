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

        public virtual async Task<ProfileDto> GetAsync()
        {
            return ObjectMapper.Map<IdentityUser, ProfileDto>(
                await _userManager.GetByIdAsync(CurrentUser.GetId())
.ConfigureAwait(false));
        }

        public virtual async Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
        {
            var user = await _userManager.GetByIdAsync(CurrentUser.GetId()).ConfigureAwait(false);

            if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled).ConfigureAwait(false))
            {
                (await _userManager.SetUserNameAsync(user, input.UserName).ConfigureAwait(false)).CheckErrors();
            }

            if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsEmailUpdateEnabled).ConfigureAwait(false))
            {
                (await _userManager.SetEmailAsync(user, input.Email).ConfigureAwait(false)).CheckErrors();
            }

            (await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber).ConfigureAwait(false)).CheckErrors();

            user.Name = input.Name;
            user.Surname = input.Surname;

            (await _userManager.UpdateAsync(user).ConfigureAwait(false)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return ObjectMapper.Map<IdentityUser, ProfileDto>(user);
        }

        public virtual async Task ChangePasswordAsync(ChangePasswordInput input)
        {
            var currentUser = await _userManager.GetByIdAsync(CurrentUser.GetId()).ConfigureAwait(false);
            (await _userManager.ChangePasswordAsync(currentUser, input.CurrentPassword, input.NewPassword).ConfigureAwait(false)).CheckErrors();
        }
    }
}
