using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Localization;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.VirtualFileSystem;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.Account.Web
{
    [DependsOn(
        typeof(AbpAccountWebModule),
        typeof(AbpOpenIddictDomainModule)
        )]
    public class AbpAccountWebOpenIddictModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpIdentityAspNetCoreOptions>(options =>
            {
                options.ConfigureAuthentication = false;
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountWebOpenIddictModule).Assembly);
            });

            PreConfigure<OpenIddictBuilder>(openIddictBuilder =>
            {
                ConfigureOpenIddictServer(context.Services, openIddictBuilder);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAccountWebOpenIddictModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpOpenIddictWebResource>("en")
                    .AddBaseTypes(typeof(AccountResource))
                    .AddVirtualJson("/Localization/AbpOpenIddictWeb");
            });

            //TODO: Try to reuse from AbpIdentityAspNetCoreModule
            context.Services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();
        }

        private void ConfigureOpenIddictServer(IServiceCollection services, OpenIddictBuilder openIddictBuilder)
        {
            var hostEnvironment = services.GetHostingEnvironment();
            var builderOptions = services.ExecutePreConfiguredActions<AbpOpenIddictBuilderOptions>();

            openIddictBuilder
                // Register the OpenIddict server components.
                .AddServer(options =>
                {
                    // Enable the authorization, device, logout, token, userinfo and verification endpoints.
                    options.SetAuthorizationEndpointUris("/connect/authorize")
                           .SetDeviceEndpointUris("/connect/device")
                           .SetLogoutEndpointUris("/account/logout")
                           .SetTokenEndpointUris("/connect/token")
                           .SetUserinfoEndpointUris("/connect/userinfo")
                           .SetIntrospectionEndpointUris("/connect/introspect")
                           .SetVerificationEndpointUris("/connect/verify");

                    // Note: this sample uses the code, device code, password and refresh token flows, but you
                    // can enable the other flows if you need to support implicit or client credentials.
                    options.AllowAuthorizationCodeFlow()
                           .AllowHybridFlow()
                           .AllowClientCredentialsFlow()
                           .AllowDeviceCodeFlow()
                           .AllowPasswordFlow()
                           .AllowRefreshTokenFlow();

                    // Mark the "email", "profile", "roles", "phone" scopes as supported scopes.
                    options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, Scopes.Phone);

                    if (builderOptions.AddDeveloperSigningCredential)
                    {
                        // Register the signing and encryption credentials.
                        options
                            .AddDevelopmentEncryptionCertificate()
                            .AddDevelopmentSigningCertificate();
                    }

                    if (builderOptions.RequireProofKeyForCodeExchange)
                    {
                        // Force client applications to use Proof Key for Code Exchange (PKCE).
                        options.RequireProofKeyForCodeExchange();
                    }

                    // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                    var openIddictServerAspNetCoreBuilder = options
                           .UseAspNetCore()
                           .EnableStatusCodePagesIntegration()
                           .EnableAuthorizationEndpointPassthrough()
                           .EnableLogoutEndpointPassthrough()
                           .EnableTokenEndpointPassthrough()
                           .EnableUserinfoEndpointPassthrough()
                           .EnableVerificationEndpointPassthrough();


                    if (hostEnvironment.IsDevelopment())
                    {
                        openIddictServerAspNetCoreBuilder.DisableTransportSecurityRequirement();
                    }

                    options
                        .SetAccessTokenLifetime(TimeSpan.FromDays(1))
                        .SetRefreshTokenLifetime(TimeSpan.FromDays(365));

                    services.ExecutePreConfiguredActions(openIddictServerAspNetCoreBuilder);

                    services.ExecutePreConfiguredActions(options);
                })
                .AddValidation(options =>
                {
                    services.ExecutePreConfiguredActions(options.UseAspNetCore());

                    services.ExecutePreConfiguredActions(options);
                });
        }
    }
}
