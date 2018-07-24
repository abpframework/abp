using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.DemoApp
{
    [DependsOn(
        typeof(BackgroundJobsEntityFrameworkCoreModule),
        typeof(AbpAutofacModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule)
        )]
    public class DemoAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = BuildConfiguration();
            context.Services.AddConfiguration(configuration);

            context.Services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = configuration.GetConnectionString("Default");
            });

            context.Services.Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(opts =>
                {
                    opts.UseSqlServer();
                });
            });

            context.Services.Configure<BackgroundJobOptions>(options =>
            {
                options.JobPollPeriod = 1000;
            });

            context.Services.AddAssemblyOf<DemoAppModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context
                .ServiceProvider
                .GetRequiredService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);
        }

        private static IConfigurationRoot BuildConfiguration(string fileName = "appsettings")
        {
            var fileNameWithExtension = fileName + ".json";
            var directory = Directory.GetCurrentDirectory();

            while (!File.Exists(Path.Combine(directory, fileNameWithExtension)))
            {
                var parentDirectory = new DirectoryInfo(directory).Parent;
                if (parentDirectory == null)
                {
                    break;
                }

                directory = parentDirectory.FullName;
            }

            if(File.Exists(Path.Combine(directory, fileNameWithExtension)))
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(directory)
                    .AddJsonFile(fileNameWithExtension, optional: false);

                return builder.Build();
            }

            return null;
        }
    }
}
