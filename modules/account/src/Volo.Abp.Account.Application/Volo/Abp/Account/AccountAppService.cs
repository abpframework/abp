using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Account.Settings;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Settings;

namespace Volo.Abp.Account
{
    public class AccountAppService : ApplicationService, IAccountAppService
    {
        protected IdentityUserManager UserManager { get; }

        public AccountAppService(
            IdentityUserManager userManager)
        {
            UserManager = userManager;
        }

        public virtual async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            await CheckSelfRegistrationAsync().ConfigureAwait(false);

            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);

            (await UserManager.CreateAsync(user, input.Password).ConfigureAwait(false)).CheckErrors();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled).ConfigureAwait(false))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }
    }
}
