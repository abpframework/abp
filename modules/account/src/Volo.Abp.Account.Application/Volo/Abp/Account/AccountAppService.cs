using System.Linq;
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
        private readonly IIdentityRoleRepository _roleRepository;
        protected IdentityUserManager UserManager { get; }

        public AccountAppService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
            UserManager = userManager;
        }

        public virtual async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            await CheckSelfRegistrationAsync();

            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);

            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

            await UserManager.SetEmailAsync(user,input.EmailAddress).ConfigureAwait(false);

            await SetDefaultRolesAsync(user);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        protected virtual async Task SetDefaultRolesAsync(IdentityUser user)
        {
            var defaultRoles = await _roleRepository.GetDefaultOnesAsync().ConfigureAwait(false);

            await UserManager.SetRolesAsync(user, defaultRoles.Select(r => r.Name)).ConfigureAwait(false);
        }

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }
    }
}
