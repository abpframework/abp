using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Globalization;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.WildcardDomains;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.OpenIddict;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpOpenIddictDomainModule)
)]
public class AbpOpenIddictAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        AddOpenIddictServer(context.Services);

        Configure<AbpOpenIddictClaimsPrincipalOptions>(options =>
        {
            options.ClaimsPrincipalHandlers.Add<AbpDefaultScopesHandler>();
            options.ClaimsPrincipalHandlers.Add<AbpDefaultOpenIddictClaimsPrincipalHandler>();
        });

        var preActions = context.Services.GetPreConfigureActions<AbpOpenIddictAspNetCoreOptions>();
        Configure<AbpOpenIddictAspNetCoreOptions>(options =>
        {
            preActions.Configure(options);
        });

        Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationFormats.Add("/Volo/Abp/OpenIddict/Views/{1}/{0}.cshtml");
        });

        ConfigureSecurityStampValidator(context.Services);
    }

    /// <summary>
    /// The <see cref="OpenIddictClaimsPrincipalContributor"/> adds the ambient authorization
    /// request's <c>client_id</c> claim to every principal built by
    /// <c>SignInManager.CreateUserPrincipalAsync</c>. That is correct for principals that OpenIddict
    /// signs into tokens, but the same method is also used by the cookie security-stamp validator to
    /// rebuild and re-issue the interactive authentication cookie. When the cookie happens to be
    /// refreshed during a <c>/connect/authorize</c> request, the <c>client_id</c> of the OAuth client
    /// being authorized leaks into the cookie and corrupts <c>ICurrentClient.Id</c> (and therefore
    /// audit-log client attribution) for every later cookie-authenticated request in that browser.
    ///
    /// The contributor cannot tell whether the principal it contributes to is destined for a token or
    /// for the cookie, so the claim is stripped here, at the only point where the cookie is actually
    /// re-written: the security-stamp <c>OnRefreshingPrincipal</c> callback. This never runs for token
    /// issuance (which signs into the OpenIddict scheme, not the cookie), so the token path is left
    /// untouched. The removal is chained after any previously registered callback (e.g. ABP Identity's
    /// <c>SecurityStampValidatorCallback.UpdatePrincipal</c>), so it also self-heals cookies that were
    /// already corrupted before this fix.
    /// </summary>
    internal static void ConfigureSecurityStampValidator(IServiceCollection services)
    {
        services.Configure<SecurityStampValidatorOptions>(options =>
        {
            var previousOnRefreshingPrincipal = options.OnRefreshingPrincipal;
            options.OnRefreshingPrincipal = async context =>
            {
                if (previousOnRefreshingPrincipal != null)
                {
                    await previousOnRefreshingPrincipal(context);
                }

                // Runs after any previously registered callback (e.g. ABP Identity's
                // SecurityStampValidatorCallback.UpdatePrincipal), so an already-corrupted
                // cookie that re-introduces client_id during the refresh still gets cleaned.
                RemoveClientIdClaimsFromRefreshedPrincipal(context);
            };
        });
    }

    internal static void RemoveClientIdClaimsFromRefreshedPrincipal(SecurityStampRefreshingPrincipalContext context)
    {
        if (context.NewPrincipal == null)
        {
            return;
        }

        foreach (var identity in context.NewPrincipal.Identities)
        {
            foreach (var clientIdClaim in identity.FindAll(AbpClaimTypes.ClientId).ToArray())
            {
                identity.RemoveClaim(clientIdClaim);
            }
        }
    }

    private void AddOpenIddictServer(IServiceCollection services)
    {
        var builderOptions = services.ExecutePreConfiguredActions<AbpOpenIddictAspNetCoreOptions>();

        if (builderOptions.UpdateAbpClaimTypes)
        {
            AbpClaimTypes.UserId = OpenIddictConstants.Claims.Subject;
            AbpClaimTypes.Role = OpenIddictConstants.Claims.Role;
            AbpClaimTypes.UserName = OpenIddictConstants.Claims.PreferredUsername;
            AbpClaimTypes.Name = OpenIddictConstants.Claims.GivenName;
            AbpClaimTypes.SurName = OpenIddictConstants.Claims.FamilyName;
            AbpClaimTypes.PhoneNumber = OpenIddictConstants.Claims.PhoneNumber;
            AbpClaimTypes.PhoneNumberVerified = OpenIddictConstants.Claims.PhoneNumberVerified;
            AbpClaimTypes.Email = OpenIddictConstants.Claims.Email;
            AbpClaimTypes.EmailVerified = OpenIddictConstants.Claims.EmailVerified;
            AbpClaimTypes.ClientId = OpenIddictConstants.Claims.ClientId;
        }

        var openIddictBuilder = services.AddOpenIddict()
            .AddServer(builder =>
            {
                builder
                    .SetAuthorizationEndpointUris("connect/authorize", "connect/authorize/callback")
                    // .well-known/oauth-authorization-server
                    // .well-known/openid-configuration
                    //.SetConfigurationEndpointUris()
                    // .well-known/jwks
                    //.SetCryptographyEndpointUris()
                    .SetDeviceAuthorizationEndpointUris("device")
                    .SetIntrospectionEndpointUris("connect/introspect")
                    .SetEndSessionEndpointUris("connect/endsession")
                    .SetPushedAuthorizationEndpointUris("connect/par")
                    .SetRevocationEndpointUris("connect/revocat")
                    .SetTokenEndpointUris("connect/token")
                    .SetUserInfoEndpointUris("connect/userinfo")
                    .SetEndUserVerificationEndpointUris("connect/verify");

                builder
                    .AllowAuthorizationCodeFlow()
                    .AllowHybridFlow()
                    .AllowImplicitFlow()
                    .AllowPasswordFlow()
                    .AllowClientCredentialsFlow()
                    .AllowRefreshTokenFlow()
                    .AllowDeviceAuthorizationFlow()
                    .AllowNoneFlow()
                    .AllowTokenExchangeFlow();

                builder.RegisterScopes(new[]
                {
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Phone,
                    OpenIddictConstants.Scopes.Roles,
                    OpenIddictConstants.Scopes.Address,
                    OpenIddictConstants.Scopes.OfflineAccess
                });

                builder.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserInfoEndpointPassthrough()
                    .EnableEndSessionEndpointPassthrough()
                    .EnableEndUserVerificationEndpointPassthrough()
                    .EnableStatusCodePagesIntegration();

                if (builderOptions.AddDevelopmentEncryptionAndSigningCertificate)
                {
                    builder
                        .AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
                }

                builder.DisableAccessTokenEncryption();

                var wildcardDomainsOptions = services.ExecutePreConfiguredActions<AbpOpenIddictWildcardDomainOptions>();
                if (wildcardDomainsOptions.EnableWildcardDomainSupport)
                {
                    var preActions = services.GetPreConfigureActions<AbpOpenIddictWildcardDomainOptions>();

                    Configure<AbpOpenIddictWildcardDomainOptions>(options =>
                    {
                        preActions.Configure(options);
                    });

                    builder.RemoveEventHandler(OpenIddictServerHandlers.Authentication.ValidateClientRedirectUri.Descriptor);
                    builder.AddEventHandler(AbpValidateClientRedirectUri.Descriptor);

                    builder.RemoveEventHandler(OpenIddictServerHandlers.Authentication.ValidateRedirectUriParameter.Descriptor);
                    builder.AddEventHandler(AbpValidateRedirectUriParameter.Descriptor);

                    builder.RemoveEventHandler(OpenIddictServerHandlers.Session.ValidateClientPostLogoutRedirectUri.Descriptor);
                    builder.AddEventHandler(AbpValidateClientPostLogoutRedirectUri.Descriptor);

                    builder.RemoveEventHandler(OpenIddictServerHandlers.Session.ValidatePostLogoutRedirectUriParameter.Descriptor);
                    builder.AddEventHandler(AbpValidatePostLogoutRedirectUriParameter.Descriptor);

                    builder.RemoveEventHandler(OpenIddictServerHandlers.Session.ValidateAuthorizedParty.Descriptor);
                    builder.AddEventHandler(AbpValidateAuthorizedParty.Descriptor);
                }

                builder.AddEventHandler(RemoveClaimsFromClientCredentialsGrantType.Descriptor);
                builder.AddEventHandler(AttachScopes.Descriptor);
                builder.AddEventHandler(AttachCultureInfo.Descriptor);

                services.ExecutePreConfiguredActions(builder);
            });
    }
}
