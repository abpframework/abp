using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc.Bundling;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Http.Client;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity.Web;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.Web;
using Volo.Abp.Permissions;
using Volo.Abp.Permissions.EntityFrameworkCore;

namespace MicroserviceDemo.Web
{
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpHttpClientModule))]
    [DependsOn(typeof(AbpPermissionsEntityFrameworkCoreModule))]
    [DependsOn(typeof(AbpIdentityHttpApiModule))]
    [DependsOn(typeof(AbpIdentityWebModule))]
    [DependsOn(typeof(AbpIdentityEntityFrameworkCoreModule))]
    [DependsOn(typeof(AbpAccountWebModule))]
    [DependsOn(typeof(AbpMultiTenancyHttpApiClientModule))]
    [DependsOn(typeof(AbpMultiTenancyWebModule))]
    public class MicroservicesDemoWebModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<IMvcBuilder>(builder =>
            {
                builder.AddViewLocalization(); //TODO: To the framework!
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            var hostingEnvironment = services.GetSingletonInstance<IHostingEnvironment>();
            var configuration = BuildConfiguration(hostingEnvironment);

            services.Configure<DbConnectionOptions>(configuration);

            services.Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(context =>
                {
                    if (context.ExistingConnection != null)
                    {
                        context.DbContextOptions.UseSqlServer(context.ExistingConnection);
                    }
                    else
                    {
                        context.DbContextOptions.UseSqlServer(context.ConnectionString);
                    }
                });

                //TODO: This should not be neededn when we fix the conn string name problem for interfaces
                options.Configure<AbpPermissionsDbContext>(context =>
                {
                    context.DbContextOptions.UseSqlServer(configuration.GetConnectionString("AbpPermissions"));
                });
            });

            services.Configure<BundlingOptions>(options =>
            {
                //TODO: To the framework!
                options.ScriptBundles.Add("GlobalScripts", new[]
                {
                    "/Abp/ApplicationConfigurationScript?_v=" + DateTime.Now.Ticks,
                    "/Abp/ServiceProxyScript?_v=" + DateTime.Now.Ticks
                }); 
            });

            services.AddAuthentication();

            services.AddHttpClientProxies(typeof(AbpPermissionsApplicationContractsModule).Assembly, "AbpPermissions"); //TODO: Create permission http client module and remove this one
            services.Configure<RemoteServiceOptions>(configuration);

            services.AddAssemblyOf<MicroservicesDemoWebModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            if (context.GetEnvironment().IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseVirtualFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }

        private static IConfigurationRoot BuildConfiguration(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}