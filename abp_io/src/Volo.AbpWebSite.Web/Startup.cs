using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Volo.Abp;

namespace Volo.AbpWebSite
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AbpWebSiteWebModule>(options =>
            {
                options.UseAutofac();
                options.Configuration.UserSecretsAssembly = typeof(AbpWebSiteWebModule).Assembly;
            });

            return services.BuildServiceProviderFromFactory();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole()
                .AddDebug()
                .AddSerilog(new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.With(app.ApplicationServices.GetRequiredService<CorrelationIdLogEventEnricher>())
                    .WriteTo.File("Logs/logs.txt",outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
                    .CreateLogger()
                );

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            app.InitializeApplication();
        }
    }
}
