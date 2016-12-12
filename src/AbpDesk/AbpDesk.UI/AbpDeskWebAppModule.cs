using System.IO;
using AbpDesk.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace AbpDesk
{
    //TODO: Rename project to AbpDesk.Web.Mvc

    [DependsOn(typeof(AbpAspNetCoreMvcModule), typeof(AbpDeskApplicationModule), typeof(AbpDeskEntityFrameworkCoreModule))]
    public class AbpDeskWebAppModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var hostingEnvironment = services.GetSingletonInstance<IHostingEnvironment>();

            var configuration = BuildConfiguration(hostingEnvironment);

            services.Configure<DbConnectionOptions>(configuration);

            services.Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(context =>
                {
                    context.DbContextOptions.UseSqlServer(context.ConnectionString);
                });
            });

            services.AddDbContext<AbpDeskDbContext>(options =>
            {
                options.UseSqlServer("Server=localhost;Database=AbpDesk;Trusted_Connection=True;");
            });

            services.AddMvc();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            context.GetLoggerFactory().AddConsole();

            if (context.GetEnvironment().IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static IConfigurationRoot BuildConfiguration(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            return configuration;
        }
    }
}
