using System.IO;
using AbpDesk.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace AbpDesk.ConsoleDemo
{
    [DependsOn(typeof(AbpDeskApplicationModule), typeof(AbpDeskEntityFrameworkCoreModule))]
    public class AbpDeskConsoleDemoModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var configuration = BuildConfiguration();

            services.Configure<DbConnectionOptions>(configuration);

            services.Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(context =>
                {
                    context.DbContextOptions.UseSqlServer(context.ConnectionString);
                });
            });

            services.AddAssemblyOf<AbpDeskConsoleDemoModule>();
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            return configuration;
        }
    }
}
