using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using ProductManagement;
using Swashbuckle.AspNetCore.Swagger;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Blogging;

namespace PublicWebSiteGateway.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(BloggingHttpApiModule),
        typeof(ProductManagementHttpApiModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule)
        )]
    public class PublicWebSiteGatewayHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //TODO: Internal gateway may not need to authentication in the gateway level, we may remove this when we complete and use the other gateways

            context.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:64999";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "PublicWebSiteGateway";

                    //TODO: Should create an extension method for that (may require to create a new ABP package depending on the IdentityServer4.AccessTokenValidation)
                    options.InboundJwtClaimTypeMap["sub"] = AbpClaimTypes.UserId;
                    options.InboundJwtClaimTypeMap["role"] = AbpClaimTypes.Role;
                    options.InboundJwtClaimTypeMap["email"] = AbpClaimTypes.Email;
                    options.InboundJwtClaimTypeMap["email_verified"] = AbpClaimTypes.EmailVerified;
                    options.InboundJwtClaimTypeMap["phone_number"] = AbpClaimTypes.PhoneNumber;
                    options.InboundJwtClaimTypeMap["phone_number_verified"] = AbpClaimTypes.PhoneNumberVerified;
                    options.InboundJwtClaimTypeMap["name"] = AbpClaimTypes.UserName;
                });

            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "PublicWebSite Gateway API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });

            context.Services.AddOcelot(context.Services.GetConfiguration());

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseVirtualFiles();
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "PublicWebSite Gateway API");
            });

            app.MapWhen(ctx => ctx.Request.Path.ToString().StartsWith("/api/abp/") || ctx.Request.Path.ToString().StartsWith("/Abp/"),
                app2 => { app2.UseMvcWithDefaultRouteAndArea(); });

            app.UseOcelot().Wait();
        }
    }
}
