using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Volo.Abp;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.Autofac;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace IdentityService.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityApplicationModule)
        )]
    public class IdentityServiceHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureAuthentication(context);
            ConfigureSwagger(context);
            //TODO: Configure localization?
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseVirtualFiles();
            app.UseAuthentication();
            //app.UseAbpRequestLocalization(); //TODO: localization?
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service API");
            });
            app.UseAuditing();
            app.UseMvcWithDefaultRoute();
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context)
        {
            context.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:9999"; //TODO: Update
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "IdentityService";
                });
        }

        private static void ConfigureSwagger(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new Info { Title = "Identity Service API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                });
        }
    }
}
