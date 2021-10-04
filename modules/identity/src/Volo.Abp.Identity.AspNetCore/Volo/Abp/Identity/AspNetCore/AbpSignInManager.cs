using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Identity.AspNetCore
{
    public class AbpSignInManager : SignInManager<IdentityUser>
    {
        protected AbpIdentityOptions AbpOptions { get; }

        public AbpSignInManager(
            IdentityUserManager userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<IdentityUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<IdentityUser> confirmation,
            IOptions<AbpIdentityOptions> options
        ) : base(
            userManager,
            contextAccessor,
            claimsFactory,
            optionsAccessor,
            logger,
            schemes,
            confirmation)
        {
            AbpOptions = options.Value;
        }

        public override async Task<SignInResult> PasswordSignInAsync(
            string userName,
            string password,
            bool isPersistent,
            bool lockoutOnFailure)
        {
            foreach (var externalLoginProviderInfo in AbpOptions.ExternalLoginProviders.Values)
            {
                var externalLoginProvider = (IExternalLoginProvider) Context.RequestServices
                    .GetRequiredService(externalLoginProviderInfo.Type);

                if (await externalLoginProvider.TryAuthenticateAsync(userName, password))
                {
                    var user = await UserManager.FindByNameAsync(userName);
                    if (user == null)
                    {
                        user = await externalLoginProvider.CreateUserAsync(userName, externalLoginProviderInfo.Name);
                    }
                    else
                    {
                        await externalLoginProvider.UpdateUserAsync(user, externalLoginProviderInfo.Name);
                    }

                    return await SignInOrTwoFactorAsync(user, isPersistent);
                }
            }

            return await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }

        protected override async Task<SignInResult> PreSignInCheck(IdentityUser user)
        {
            if (!user.IsActive)
            {
                Logger.LogWarning("User is currently inactive.");
                return SignInResult.NotAllowed;
            }

            return await base.PreSignInCheck(user);
        }
    }
}
