using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity.HttpApi.Host
{
    [DependsOn(typeof(AbpIdentityHttpApiModule), typeof(AbpIdentityEntityFrameworkCoreModule))]
    public class AbpIdentityHttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var hostingEnvironment = services.GetSingletonInstance<IHostingEnvironment>();
            var configuration = BuildConfiguration(hostingEnvironment);

            services.Configure<DbConnectionOptions>(configuration);

            services.Configure<AbpDbContextOptions>(options =>
            {
                //Configures all dbcontextes to use Sql Server with calculated connection string
                options.Configure(context =>
                {
                    context.DbContextOptions.UseSqlServer(context.ConnectionString);
                });
            });

            services.AddMvc();
            services.AddAssemblyOf<AbpIdentityHttpApiHostModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            context.GetLoggerFactory().AddConsole().AddDebug();

            if (context.GetEnvironment().IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(); //No need to a default route
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
