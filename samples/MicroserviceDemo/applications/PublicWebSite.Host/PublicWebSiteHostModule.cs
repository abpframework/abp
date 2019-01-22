using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ProductManagement;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.OAuth;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Blogging;

namespace PublicWebSite.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcClientModule),
        typeof(AbpAspNetCoreAuthenticationOAuthModule),
        typeof(AbpHttpClientIdentityModelModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(BloggingHttpApiClientModule),
        typeof(BloggingWebModule),
        typeof(ProductManagementHttpApiClientModule)
        )]
    public class PublicWebSiteHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
            });

            Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new PublicWebSiteMenuContributor());
            });

            context.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "http://localhost:64999";
                    options.RequireHttpsMetadata = false;
                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

                    options.ClientId = "public-website-client";
                    options.ClientSecret = "1q2w3e*";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("role");
                    options.Scope.Add("email");
                    options.Scope.Add("phone");
                    options.Scope.Add("PublicWebSiteGateway");
                    options.Scope.Add("ProductService");
                    options.Scope.Add("BloggingService");

                    options.ClaimActions.MapAbpClaimTypes();
                });

            context.Services.AddDistributedRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Configuration"];
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseVirtualFiles();
            app.UseAuthentication();
            app.UseAbpRequestLocalization();
            app.UseMvcWithDefaultRouteAndArea();
        }
    }
}
