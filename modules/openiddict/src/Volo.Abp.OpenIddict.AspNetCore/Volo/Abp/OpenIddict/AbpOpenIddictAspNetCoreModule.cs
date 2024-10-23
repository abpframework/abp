﻿using Microsoft.AspNetCore.Mvc.Razor;
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
            options.ClaimsPrincipalHandlers.Add<AbpDefaultOpenIddictClaimsPrincipalHandler>();
        });

        Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationFormats.Add("/Volo/Abp/OpenIddict/Views/{1}/{0}.cshtml");
        });
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
                    .AllowNoneFlow();

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

        services.ExecutePreConfiguredActions(openIddictBuilder);
    }
}
