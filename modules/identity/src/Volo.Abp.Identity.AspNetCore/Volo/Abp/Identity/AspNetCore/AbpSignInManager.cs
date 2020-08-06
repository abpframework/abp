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
        protected AbpIdentityAspNetCoreOptions AbpOptions { get; }

        public AbpSignInManager(
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<IdentityUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<IdentityUser> confirmation,
            IOptions<AbpIdentityAspNetCoreOptions> options
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
                        user = await externalLoginProvider.CreateUserAsync(userName);
                        //TODO: TenantId, LoginProvider, Password, NormalizeNames
                        //TODO: Set default roles
                        await UserManager.CreateAsync(user);
                    }
                    else
                    {
                        await externalLoginProvider.UpdateUserAsync(user);
                        //TODO: LoginProvider
                        await UserManager.UpdateAsync(user);
                    }

                    return await SignInOrTwoFactorAsync(user, isPersistent);
                }
            }

            return await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }
    }
}
