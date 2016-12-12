using AbpDesk.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace AbpDesk
{
    //TODO: Rename project to AbpDesk.Web.Mvc

    [DependsOn(typeof(AbpAspNetCoreMvcModule), typeof(AbpDeskApplicationModule), typeof(AbpDeskEntityFrameworkCoreModule))]
    public class AbpDeskWebAppModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
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
    }
}
