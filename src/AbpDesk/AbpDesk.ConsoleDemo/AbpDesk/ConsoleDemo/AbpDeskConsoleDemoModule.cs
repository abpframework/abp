using System.IO;
using AbpDesk.Blogging;
using AbpDesk.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace AbpDesk.ConsoleDemo
{
    [DependsOn(
        typeof(AbpDeskApplicationModule), 
        typeof(AbpDeskEntityFrameworkCoreModule), 
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpDeskMongoBlogModule))]
    public class AbpDeskConsoleDemoModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var configuration = BuildConfiguration(Directory.GetCurrentDirectory());

            AbpDeskDbConfigurer.Configure(services, configuration);

            services.AddAssemblyOf<AbpDeskConsoleDemoModule>();
        }

        private static IConfigurationRoot BuildConfiguration(string basePath)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
