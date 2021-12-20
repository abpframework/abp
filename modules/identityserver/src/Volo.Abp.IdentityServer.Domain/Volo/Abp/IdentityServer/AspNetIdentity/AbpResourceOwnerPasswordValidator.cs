using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.IdentityServer.AspNetIdentity;

public class AbpResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    protected SignInManager<IdentityUser> SignInManager { get; }
    protected UserManager<IdentityUser> UserManager { get; }
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    protected ILogger<ResourceOwnerPasswordValidator<IdentityUser>> Logger { get; }
    protected IStringLocalizer<AbpIdentityServerResource> Localizer { get; }
    protected IHybridServiceScopeFactory ServiceScopeFactory { get; }
    protected AbpIdentityOptions AbpIdentityOptions { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }

    public AbpResourceOwnerPasswordValidator(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IdentitySecurityLogManager identitySecurityLogManager,
        ILogger<ResourceOwnerPasswordValidator<IdentityUser>> logger,
        IStringLocalizer<AbpIdentityServerResource> localizer,
        IOptions<AbpIdentityOptions> abpIdentityOptions,
        IHybridServiceScopeFactory serviceScopeFactory,
        IOptions<IdentityOptions> identityOptions)
    {
        UserManager = userManager;
        SignInManager = signInManager;
        IdentitySecurityLogManager = identitySecurityLogManager;
        Logger = logger;
        Localizer = localizer;
        ServiceScopeFactory = serviceScopeFactory;
        AbpIdentityOptions = abpIdentityOptions.Value;
        IdentityOptions = identityOptions;
    }

    /// <summary>
    /// https://github.com/IdentityServer/IdentityServer4/blob/master/src/AspNetIdentity/src/ResourceOwnerPasswordValidator.cs#L53
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            await ReplaceEmailToUsernameOfInputIfNeeds(context);

            IdentityUser user = null;

            if (AbpIdentityOptions.ExternalLoginProviders.Any())
            {
                foreach (var externalLoginProviderInfo in AbpIdentityOptions.ExternalLoginProviders.Values)
                {
                    var externalLoginProvider = (IExternalLoginProvider)scope.ServiceProvider
                        .GetRequiredService(externalLoginProviderInfo.Type);

                    if (await externalLoginProvider.TryAuthenticateAsync(context.UserName, context.Password))
                    {
                        user = await UserManager.FindByNameAsync(context.UserName);
                        if (user == null)
                        {
                            user = await externalLoginProvider.CreateUserAsync(context.UserName, externalLoginProviderInfo.Name);
                        }
                        else
                        {
                            await externalLoginProvider.UpdateUserAsync(user, externalLoginProviderInfo.Name);
                        }

                        await SetSuccessResultAsync(context, user);
                        return;
                    }
                }
            }

            user = await UserManager.FindByNameAsync(context.UserName);
            string errorDescription;
            if (user != null)
            {
                await IdentityOptions.SetAsync();
                var result = await SignInManager.CheckPasswordSignInAsync(user, context.Password, true);
                if (result.Succeeded)
                {
                    if (await IsTfaEnabledAsync(user))
                    {
                        await HandleTwoFactorLoginAsync(context, user);
                    }
                    else
                    {
                        await SetSuccessResultAsync(context, user);
                    }
                    return;
                }

                if (result.IsLockedOut)
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: locked out", context.UserName);
                    errorDescription = Localizer["UserLockedOut"];
                }
                else if (result.IsNotAllowed)
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.UserName);
                    errorDescription = Localizer["LoginIsNotAllowed"];
                }
                else
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
                    errorDescription = Localizer["InvalidUserNameOrPassword"];
                }

                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                {
                    Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
                    Action = result.ToIdentitySecurityLogAction(),
                    UserName = context.UserName,
                    ClientId = await FindClientIdAsync(context)
                });
            }
            else
            {
                Logger.LogInformation("No user found matching username: {username}", context.UserName);
                errorDescription = Localizer["InvalidUsername"];

                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                {
                    Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
                    Action = IdentityServerSecurityLogActionConsts.LoginInvalidUserName,
                    UserName = context.UserName,
                    ClientId = await FindClientIdAsync(context)
                });
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, errorDescription);
        }
    }

    protected virtual async Task HandleTwoFactorLoginAsync(ResourceOwnerPasswordValidationContext context, IdentityUser user)
    {
        var twoFactorProvider = context.Request?.Raw?["TwoFactorProvider"];
        var twoFactorCode = context.Request?.Raw?["TwoFactorCode"];
        if (!twoFactorProvider.IsNullOrWhiteSpace() && !twoFactorCode.IsNullOrWhiteSpace())
        {
            var providers = await UserManager.GetValidTwoFactorProvidersAsync(user);
            if (providers.Contains(twoFactorProvider) && await UserManager.VerifyTwoFactorTokenAsync(user, twoFactorProvider, twoFactorCode))
            {
                await SetSuccessResultAsync(context, user);
                return;
            }

            Logger.LogInformation("Authentication failed for username: {username}, reason: InvalidAuthenticatorCode", context.UserName);
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, Localizer["InvalidAuthenticatorCode"]);
        }
        else
        {
            Logger.LogInformation("Authentication failed for username: {username}, reason: RequiresTwoFactor", context.UserName);
            var twoFactorToken = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, nameof(SignInResult.RequiresTwoFactor));
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, nameof(SignInResult.RequiresTwoFactor),
                new Dictionary<string, object>()
                {
                        {"userId", user.Id},
                        {"twoFactorToken", twoFactorToken}
                });

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
                Action = IdentityServerSecurityLogActionConsts.LoginRequiresTwoFactor,
                UserName = context.UserName,
                ClientId = await FindClientIdAsync(context)
            });
        }
    }

    protected virtual async Task SetSuccessResultAsync(ResourceOwnerPasswordValidationContext context, IdentityUser user)
    {
        var sub = await UserManager.GetUserIdAsync(user);

        Logger.LogInformation("Credentials validated for username: {username}", context.UserName);

        var additionalClaims = new List<Claim>();

        await AddCustomClaimsAsync(additionalClaims, user, context);

        context.Result = new GrantValidationResult(
            sub,
            OidcConstants.AuthenticationMethods.Password,
            additionalClaims.ToArray()
        );

        await IdentitySecurityLogManager.SaveAsync(
            new IdentitySecurityLogContext
            {
                Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
                Action = IdentityServerSecurityLogActionConsts.LoginSucceeded,
                UserName = context.UserName,
                ClientId = await FindClientIdAsync(context)
            }
        );
    }

    protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(ResourceOwnerPasswordValidationContext context)
    {
        if (!ValidationHelper.IsValidEmailAddress(context.UserName))
        {
            return;
        }

        var userByUsername = await UserManager.FindByNameAsync(context.UserName);
        if (userByUsername != null)
        {
            return;
        }

        var userByEmail = await UserManager.FindByEmailAsync(context.UserName);
        if (userByEmail == null)
        {
            return;
        }

        context.UserName = userByEmail.UserName;
    }

    protected virtual Task<string> FindClientIdAsync(ResourceOwnerPasswordValidationContext context)
    {
        return Task.FromResult(context.Request?.Client?.ClientId);
    }

    protected virtual async Task<bool> IsTfaEnabledAsync(IdentityUser user)
        => UserManager.SupportsUserTwoFactor &&
           await UserManager.GetTwoFactorEnabledAsync(user) &&
           (await UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0;

    protected virtual Task AddCustomClaimsAsync(List<Claim> customClaims, IdentityUser user, ResourceOwnerPasswordValidationContext context)
    {
        if (user.TenantId.HasValue)
        {
            customClaims.Add(
                new Claim(
                    AbpClaimTypes.TenantId,
                    user.TenantId?.ToString()
                )
            );
        }

        return Task.CompletedTask;
    }
}
