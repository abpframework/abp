using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Volo.Abp.OpenIddict.Controllers;

public partial class TokenController
{
    protected IHybridServiceScopeFactory ServiceScopeFactory => LazyServiceProvider.LazyGetRequiredService<IHybridServiceScopeFactory>();
    protected IOptions<AbpIdentityOptions> AbpIdentityOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpIdentityOptions>>();
    protected IOptions<IdentityOptions> IdentityOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<IdentityOptions>>();
    protected IdentitySecurityLogManager IdentitySecurityLogManager => LazyServiceProvider.LazyGetRequiredService<IdentitySecurityLogManager>();
    
    [UnitOfWork]
    protected virtual async Task<IActionResult> HandlePasswordAsync(OpenIddictRequest request)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            await ReplaceEmailToUsernameOfInputIfNeeds(request);

            IdentityUser user = null;

            if (AbpIdentityOptions.Value.ExternalLoginProviders.Any())
            {
                foreach (var externalLoginProviderInfo in AbpIdentityOptions.Value.ExternalLoginProviders.Values)
                {
                    var externalLoginProvider = (IExternalLoginProvider)scope.ServiceProvider
                        .GetRequiredService(externalLoginProviderInfo.Type);

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

                        return await SetSuccessResultAsync(request, user);
                    }
                }
            }
            
            await IdentityOptions.SetAsync();
            
            user = await UserManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                Logger.LogInformation("No user found matching username: {username}", request.Username);
                
                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = L["InvalidUsername"]
                });
                
                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                {
                    Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                    Action = OpenIddictSecurityLogActionConsts.LoginInvalidUserName,
                    UserName = request.Username,
                    ClientId = request.ClientId
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            
            var result = await SignInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (!result.Succeeded)
            {
                string errorDescription;
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
                
                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            
            if (await IsTfaEnabledAsync(user))
            {
                return await HandleTwoFactorLoginAsync(request, user);
            }

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = request.Username,
                ClientId = request.ClientId
            });
                
            return await SetSuccessResultAsync(request, user);
        }
    }
    
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
    
    protected virtual async Task<IActionResult> HandleTwoFactorLoginAsync(OpenIddictRequest request, IdentityUser user)
    {
        var twoFactorProvider = request.GetParameter("TwoFactorProvider")?.ToString();
        var twoFactorCode = request.GetParameter("TwoFactorCode")?.ToString();;
        if (!twoFactorProvider.IsNullOrWhiteSpace() && !twoFactorCode.IsNullOrWhiteSpace())
        {
            var providers = await UserManager.GetValidTwoFactorProvidersAsync(user);
            if (providers.Contains(twoFactorProvider) && await UserManager.VerifyTwoFactorTokenAsync(user, twoFactorProvider, twoFactorCode))
            {
                return await SetSuccessResultAsync(request, user);
            }

            Logger.LogInformation("Authentication failed for username: {username}, reason: InvalidAuthenticatorCode", request.Username);

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = L["InvalidAuthenticatorCode"]
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        else
        {
            Logger.LogInformation("Authentication failed for username: {username}, reason: RequiresTwoFactor", request.Username);
            var twoFactorToken = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, nameof(SignInResult.RequiresTwoFactor));

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                Action = OpenIddictSecurityLogActionConsts.LoginRequiresTwoFactor,
                UserName = request.Username,
                ClientId = request.ClientId
            });
            
            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = nameof(SignInResult.RequiresTwoFactor),
                
                ["userId"] = user.Id.ToString("N"),
                ["twoFactorToken"] = twoFactorToken
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }

    protected virtual async Task<IActionResult> SetSuccessResultAsync(OpenIddictRequest request, IdentityUser user)
    {
        // Create a new ClaimsPrincipal containing the claims that
        // will be used to create an id_token, a token or a code.
        var principal = await SignInManager.CreateUserPrincipalAsync(user);

        // Set the list of scopes granted to the client application.
        principal.SetScopes(new[]
        {
            OpenIddictConstants.Scopes.OpenId,
            OpenIddictConstants.Scopes.Email,
            OpenIddictConstants.Scopes.Profile,
            OpenIddictConstants.Scopes.Roles
        }.Intersect(request.GetScopes()));

        foreach (var claim in principal.Claims)
        {
            claim.SetDestinations(GetDestinations(claim, principal));
        }
        
        await IdentitySecurityLogManager.SaveAsync(
            new IdentitySecurityLogContext
            {
                Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                Action = OpenIddictSecurityLogActionConsts.LoginSucceeded,
                UserName = request.Username,
                ClientId = request.ClientId
            }
        );
        
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
    
    protected virtual async Task<bool> IsTfaEnabledAsync(IdentityUser user)
    {
        return UserManager.SupportsUserTwoFactor &&
               await UserManager.GetTwoFactorEnabledAsync(user) &&
               (await UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0;
    }

    //TODO: Check the token claims
    protected virtual Task AddCustomClaimsAsync(List<Claim> customClaims, IdentityUser user)
    {
        if (user.TenantId.HasValue)
        {
            customClaims.Add(new Claim(AbpClaimTypes.TenantId, user.TenantId.Value.ToString()));
        }

        return Task.CompletedTask;
    }
}