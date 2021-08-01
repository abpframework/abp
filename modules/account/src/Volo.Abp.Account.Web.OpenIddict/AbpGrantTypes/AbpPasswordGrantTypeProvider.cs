using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Security.Claims;
using Volo.Abp.Validation;
using static OpenIddict.Abstractions.OpenIddictConstants;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Web.AbpGrantTypes
{
    public class AbpPasswordGrantTypeProvider : IGrantTypeProvider, ITransientDependency
    {
        public string GrantType => GrantTypes.Password;

        protected IOpenIddictScopeManager ScopeManager { get; }

        protected AbpSignInManager SignInManager { get; }

        protected IdentityUserManager UserManager { get; }

        protected AbpIdentityOptions AbpIdentityOptions { get; }

        protected IOptions<IdentityOptions> IdentityOptions { get; }

        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }

        protected IOpenIddictDestinationService OpenIddictDestinationService { get; }

        protected IAbpLazyServiceProvider LazyServiceProvider { get; }

        protected IStringLocalizer<AbpOpenIddictWebResource> L { get; }

        public ILogger<AbpPasswordGrantTypeProvider> Logger { get; set; }

        public AbpPasswordGrantTypeProvider(
            IOpenIddictScopeManager scopeManager,
            AbpSignInManager signInManager,
            IdentityUserManager userManager,
            IOptions<AbpIdentityOptions> abpIdentityOptions,
            IOptions<IdentityOptions> identityOptions,
            IdentitySecurityLogManager identitySecurityLogManager,
            IOpenIddictDestinationService openIddictDestinationService,
            IAbpLazyServiceProvider lazyServiceProvider,
            IStringLocalizer<AbpOpenIddictWebResource> localizer)
        {
            ScopeManager = scopeManager;
            SignInManager = signInManager;
            UserManager = userManager;
            AbpIdentityOptions = abpIdentityOptions.Value;
            IdentityOptions = identityOptions;
            IdentitySecurityLogManager = identitySecurityLogManager;
            OpenIddictDestinationService = openIddictDestinationService;
            LazyServiceProvider = lazyServiceProvider;
            L = localizer;
            Logger = NullLogger<AbpPasswordGrantTypeProvider>.Instance;
        }

        public virtual async Task<GrantTypeResult> HandleAsync(OpenIddictRequest request)
        {
            await ReplaceEmailToUsernameOfInputIfNeeds(request);

            IdentityUser user;

            if (AbpIdentityOptions.ExternalLoginProviders.Any())
            {
                foreach (var externalLoginProviderInfo in AbpIdentityOptions.ExternalLoginProviders.Values)
                {
                    var externalLoginProvider = (IExternalLoginProvider)LazyServiceProvider
                        .LazyGetRequiredService(externalLoginProviderInfo.Type);

                    if (await externalLoginProvider.TryAuthenticateAsync(request.Username, request.Password))
                    {
                        user = await UserManager.FindByNameAsync(request.Username);
                        if (user == null)
                        {
                            user = await externalLoginProvider.CreateUserAsync(request.Username, externalLoginProviderInfo.Name);
                        }
                        else
                        {
                            await externalLoginProvider.UpdateUserAsync(user, externalLoginProviderInfo.Name);
                        }

                        return await ReturnSuccessResultAsync(request, user);
                    }
                }
            }

            user = await UserManager.FindByNameAsync(request.Username);
            string errorDescription;
            if (user is null)
            {
                Logger.LogInformation("No user found matching username: {username}", request.Username);
                errorDescription = L["InvalidUsername"];

                return GrantTypeResult.FailedResult(Errors.InvalidGrant, errorDescription);
            }

            // Validate the username/password parameters and ensure the account is not locked out.
            var result = await SignInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: locked out", request.Username);
                    errorDescription = L["UserLockedOut"];
                }
                else if (result.IsNotAllowed)
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", request.Username);
                    errorDescription = L["LoginIsNotAllowed"];
                }
                else
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", request.Username);
                    errorDescription = L["InvalidUserNameOrPassword"];
                }

                return GrantTypeResult.FailedResult(Errors.InvalidGrant, errorDescription);
            }

            if (await IsTfaEnabledAsync(user))
            {
                return await HandleTwoFactorLoginAsync(request, user);
            }
            else
            {
                return await ReturnSuccessResultAsync(request, user);
            }
        }

        protected virtual async Task<GrantTypeResult> ReturnSuccessResultAsync(OpenIddictRequest request, IdentityUser user)
        {
            Logger.LogInformation("Credentials validated for username: {username}", request.Username);

            await IdentitySecurityLogManager.SaveAsync(
                new IdentitySecurityLogContext
                {
                    Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                    Action = OpenIddictSecurityLogActionConsts.LoginSucceeded,
                    UserName = request.Username,
                    ClientId = request.ClientId
                }
            );

            var principal = await SignInManager.CreateUserPrincipalAsync(user);

            if (user.TenantId.HasValue)
            {
                principal.SetClaim(AbpClaimTypes.TenantId, user.TenantId?.ToString());
            }

            // Note: in this sample, the granted scopes match the requested scope
            // but you may want to allow the user to uncheck specific scopes.
            // For that, simply restrict the list of scopes before calling SetScopes.

            principal.SetScopes(request.GetScopes());
            principal.SetResources(await ScopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

            await OpenIddictDestinationService.SetDestinationsAsync(principal);

            return GrantTypeResult.SuccessResult(principal);
        }

        protected virtual async Task<GrantTypeResult> HandleTwoFactorLoginAsync(OpenIddictRequest request, IdentityUser user)
        {
            var twoFactorProvider = request.GetParameter("TwoFactorProvider")?.Value?.ToString();
            var twoFactorCode = request.GetParameter("TwoFactorCode")?.Value?.ToString();
            if (!twoFactorProvider.IsNullOrWhiteSpace() && !twoFactorCode.IsNullOrWhiteSpace())
            {
                var providers = await UserManager.GetValidTwoFactorProvidersAsync(user);
                if (providers.Contains(twoFactorProvider) && await UserManager.VerifyTwoFactorTokenAsync(user, twoFactorProvider, twoFactorCode))
                {
                    return await ReturnSuccessResultAsync(request, user);
                }

                Logger.LogInformation(
                    "Authentication failed for username: {username}, reason: InvalidAuthenticatorCode",
                    request.Username);

                return GrantTypeResult.FailedResult(Errors.InvalidGrant, L["InvalidAuthenticatorCode"]);
            }
            else
            {
                Logger.LogInformation(
                    "Authentication failed for username: {username}, reason: RequiresTwoFactor",
                    request.Username);

                var twoFactorToken = await UserManager.GenerateUserTokenAsync(user,
                    TokenOptions.DefaultProvider,
                    nameof(SignInResult.RequiresTwoFactor)
                );

                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                {
                    Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                    Action = OpenIddictSecurityLogActionConsts.LoginRequiresTwoFactor,
                    UserName = request.Username,
                    ClientId = request.ClientId
                });

                var failedResult = GrantTypeResult.FailedResult(
                    Errors.InvalidGrant,
                    nameof(SignInResult.RequiresTwoFactor)
                );
                failedResult.Properties["userId"] = user.Id.ToString();
                failedResult.Properties["twoFactorToken"] = twoFactorToken;
                return failedResult;
            }
        }

        protected virtual async Task<bool> IsTfaEnabledAsync(IdentityUser user)
            => UserManager.SupportsUserTwoFactor &&
               await UserManager.GetTwoFactorEnabledAsync(user) &&
               (await UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0;

        protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(OpenIddictRequest request)
        {
            if (!ValidationHelper.IsValidEmailAddress(request.Username))
            {
                return;
            }

            var userByUsername = await UserManager.FindByNameAsync(request.Username);
            if (userByUsername != null)
            {
                return;
            }

            var userByEmail = await UserManager.FindByEmailAsync(request.Username);
            if (userByEmail == null)
            {
                return;
            }

            request.Username = userByEmail.UserName;
        }
    }
}
