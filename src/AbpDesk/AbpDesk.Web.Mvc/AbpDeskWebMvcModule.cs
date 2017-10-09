using System;
using AbpDesk.EntityFrameworkCore;
using AbpDesk.Web.Mvc.Navigation;
using AbpDesk.Web.Mvc.Temp;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.EmbeddedFiles;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Autofac;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity.Web;
using Volo.Abp.Modularity;
using Volo.Abp.Ui.Navigation;

namespace AbpDesk.Web.Mvc
{
    [DependsOn(
        typeof(AbpAspNetCoreEmbeddedFilesModule),
        typeof(AbpAspNetCoreMvcUiBootstrapModule),
        typeof(AbpDeskApplicationModule),
        typeof(AbpDeskEntityFrameworkCoreModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpAccountWebModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpDeskWebMvcModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var hostingEnvironment = services.GetSingletonInstance<IHostingEnvironment>();
            var configuration = BuildConfiguration(hostingEnvironment);

            AbpDeskDbConfigurer.Configure(services, configuration);

            services.Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new MainMenuContributor());
            });

            //services.Configure<RemoteServiceOptions>(configuration); //Needed when we use Volo.Abp.Identity.HttpApi.Client

            services.AddMvc();

            services.AddAssemblyOf<AbpDeskWebMvcModule>();

            services.GetContainerBuilder().RegisterType<MyClassToTestAutofacCustomRegistration>();

            services.Configure<BundlingOptions>(options =>
            {
                options.ScriptBundles.Add("GlobalScripts", new[]
                {
                    "/AbpServiceProxies/GetAll?_v=" + DateTime.Now.Ticks
                });
            });

            services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(AbpDeskApplicationModule).Assembly);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            if (context.GetEnvironment().IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseEmbeddedFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
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
