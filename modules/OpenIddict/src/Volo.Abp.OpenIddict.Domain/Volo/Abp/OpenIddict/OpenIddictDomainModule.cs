using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.OpenIddict;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpIdentityDomainModule),
    typeof(OpenIddictDomainSharedModule)
)]
public class OpenIddictDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        AddOpenIddict(context.Services);
    }

    private static void AddOpenIddict(IServiceCollection services)
    {
        var builderOptions = services.ExecutePreConfiguredActions<OpenIddictBuilderOptions>();

        if (builderOptions.UpdateAbpClaimTypes)
        {
            AbpClaimTypes.UserId = JwtClaimTypes.Subject;
            AbpClaimTypes.UserName = JwtClaimTypes.Name;
            AbpClaimTypes.Role = JwtClaimTypes.Role;
            AbpClaimTypes.Email = JwtClaimTypes.Email;
        }

        var openIddictBuilder = services.AddOpenIddict()
            .AddCore(builder =>
            {
                builder.Configure(options => options.DisableAdditionalFiltering = false);

                builder.SetDefaultApplicationEntity<OpenIddictApplication>()
                    .SetDefaultAuthorizationEntity<OpenIddictAuthorization>()
                    .SetDefaultScopeEntity<OpenIddictScope>()
                    .SetDefaultTokenEntity<OpenIddictToken>();

                services.ExecutePreConfiguredActions(builder);
            })
            .AddServer(builder =>
            {
                //builder.UseDataProtection();

                // Can be enable by Configure OpenIddictServerOptions.DisableAccessTokenEncryption
                builder.DisableAccessTokenEncryption();
                    
                builder
                    .SetAuthorizationEndpointUris("/connect/authorize")
                    // /.well-known/oauth-authorization-server
                    // /.well-known/openid-configuration
                    //.SetConfigurationEndpointUris()
                    // /.well-known/jwks
                    //.SetCryptographyEndpointUris()
                    .SetDeviceEndpointUris("/connect/device")
                    .SetIntrospectionEndpointUris("/connect/introspect")
                    .SetLogoutEndpointUris("/connect/logout")
                    .SetRevocationEndpointUris("/connect/revocat")
                    .SetTokenEndpointUris("/connect/token")
                    .SetUserinfoEndpointUris("/connect/userinfo")
                    .SetVerificationEndpointUris("/connect/verify");

                builder
                    .AllowAuthorizationCodeFlow()
                    .AllowHybridFlow()
                    .AllowImplicitFlow()
                    .AllowPasswordFlow()
                    .AllowClientCredentialsFlow()
                    .AllowRefreshTokenFlow()
                    .AllowDeviceCodeFlow()
                    .AllowNoneFlow();

                builder.RegisterScopes(new []
                {
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Roles,
                    OpenIddictConstants.Scopes.Address,
                    OpenIddictConstants.Scopes.OfflineAccess
                });

                if (builderOptions.AddDevelopmentEncryptionAndSigningCertificate)
                {
                    builder.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
                }

                //TODO:
                builder.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
                    .EnableVerificationEndpointPassthrough();

                services.ExecutePreConfiguredActions(builder);

            })
            .AddValidation(builder =>
            {
                builder.UseLocalServer();
               // builder.UseDataProtection();
                builder.UseAspNetCore();

                services.ExecutePreConfiguredActions(builder);
            });

        services.Configure<OpenIddictCoreOptions>(options =>
        {
            options.DefaultApplicationType = typeof(OpenIddictApplication);
            options.DefaultAuthorizationType = typeof(OpenIddictAuthorization);
            options.DefaultScopeType = typeof(OpenIddictScope);
            options.DefaultTokenType = typeof(OpenIddictToken);
        });

        services.ExecutePreConfiguredActions(openIddictBuilder);
    }
}
