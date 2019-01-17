using AuthServer.Host.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace AuthServer.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpIdentityAspNetCoreModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpAccountWebModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule)
        )]
    public class AuthServerHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AuthServerDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });

            context.Services.Configure<AbpDbContextOptions>(options =>
            {
                //Configures defaults for all EF Core DbContexts in this application
                options.Configure(opts =>
                {
                    opts.UseSqlServer();
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseVirtualFiles();
            //app.UseIdentityServer(); //TODO: Enable
            app.UseAbpRequestLocalization();
            app.UseAuditing();
            app.UseMvcWithDefaultRoute();
        }
    }
}
