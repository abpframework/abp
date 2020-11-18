using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity
{
    public class LinkUserTokenProvider : TotpSecurityStampBasedTokenProvider<IdentityUser>
    {
        public const string LinkUserTokenProviderName = "AbpLinkUser";

        public const string LinkUserTokenPurpose = "AbpLinkUserLogin";

        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            return Task.FromResult(false);
        }
    }
}
