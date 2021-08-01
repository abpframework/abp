using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.AbpGrantTypes;
using Volo.Abp.Account.Web.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.Account.Web.Areas.OpenIddict.Controllers
{
    [Area("openiddict")]
    [ControllerName("OpenIddictAuthorization")]
    public class AuthorizationController : AbpController
    {
        protected IOpenIddictScopeManager ScopeManager { get; }

        protected AbpSignInManager SignInManager { get; }

        protected IdentityUserManager UserManager { get; }

        protected AbpIdentityOptions AbpIdentityOptions { get; }

        protected IOptions<IdentityOptions> IdentityOptions { get; }

        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }

        protected IOpenIddictDestinationService OpenIddictDestinationService { get; }

        protected AbpGrantTypeOptions GrantTypeOptions { get; }

        public AuthorizationController(
            IOpenIddictScopeManager scopeManager,
            AbpSignInManager signInManager,
            IdentityUserManager userManager,
            IOptions<AbpIdentityOptions> abpIdentityOptions,
            IOptions<IdentityOptions> identityOptions,
            IdentitySecurityLogManager identitySecurityLogManager,
            IOpenIddictDestinationService openIddictDestinationService,
            IOptions<AbpGrantTypeOptions> grantTypeOptions)
        {
            ScopeManager = scopeManager;
            SignInManager = signInManager;
            UserManager = userManager;
            AbpIdentityOptions = abpIdentityOptions.Value;
            IdentityOptions = identityOptions;
            IdentitySecurityLogManager = identitySecurityLogManager;
            OpenIddictDestinationService = openIddictDestinationService;
            GrantTypeOptions = grantTypeOptions.Value;

            LocalizationResource = typeof(AbpOpenIddictWebResource);
        }

        /// <summary>
        /// Token endpoint
        /// </summary>
        [HttpPost("~/connect/token")]
        [Produces("application/json")]
        public virtual async Task<IActionResult> ExchangeAsync()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new AbpException("The OpenID Connect request cannot be retrieved.");

            await IdentityOptions.SetAsync();
            //password
            if (request.IsPasswordGrantType())
            {
                return await GrantTypeProviderHandleAsync(GrantTypes.Password, request);
            }
            //client_credentials
            else if (request.IsClientCredentialsGrantType())
            {
                return await GrantTypeProviderHandleAsync(GrantTypes.ClientCredentials, request);
            }
            //authorization_code, device_code and refresh_token
            else if (request.IsAuthorizationCodeGrantType() || request.IsDeviceCodeGrantType() || request.IsRefreshTokenGrantType())
            {
                return await OpenIddictServerAuthenticateAsync(request);
            }

            return await GrantTypeProviderHandleAsync(request.GrantType, request);
        }

        protected virtual async Task<IActionResult> GrantTypeProviderHandleAsync(string grantType, OpenIddictRequest request)
        {
            var grantTypeResult = await GetGrantTypeProvider(grantType)
                .HandleAsync(request);

            if (!grantTypeResult.Success)
            {
                var properties = new Dictionary<string, string>(grantTypeResult.Properties)
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = grantTypeResult.Error,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = grantTypeResult.ErrorDescription
                };

                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(properties));
            }

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(grantTypeResult.Principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        protected virtual IGrantTypeProvider GetGrantTypeProvider(string grantType)
        {
            if (!GrantTypeOptions.GrantTypeProviders.TryGetValue(grantType, out var grantTypeProviderType))
            {
                throw new InvalidOperationException("The specified grant type is not supported.");
            }

            var grantTypeProviderObject = LazyServiceProvider.LazyGetService(grantTypeProviderType);

            if (grantTypeProviderObject == null ||
                grantTypeProviderObject is not IGrantTypeProvider grantTypeProvider)
            {
                throw new InvalidOperationException("The specified grant type is not supported.");
            }

            return grantTypeProvider;
        }

        protected virtual async Task<IActionResult> OpenIddictServerAuthenticateAsync(OpenIddictRequest request)
        {
            // Retrieve the claims principal stored in the authorization code/device code/refresh token.
            var principal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;

            var tenandIdValue = principal?.FindFirst(AbpClaimTypes.TenantId)?.Value;
            Guid? tenantId = null;
            if (Guid.TryParse(tenandIdValue, out var t))
            {
                tenantId = t;
            }

            using (CurrentTenant.Change(tenantId))
            {
                var user = await SignInManager.ValidateSecurityStampAsync(principal);
                //var user = await UserManager.GetUserAsync(principal);
                if (user is null)
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
                        }));
                }

                // Ensure the user is still allowed to sign in.
                if (!await SignInManager.CanSignInAsync(user))
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                        }));
                }

                await OpenIddictDestinationService.SetDestinationsAsync(principal);

                // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
        }
    }
}