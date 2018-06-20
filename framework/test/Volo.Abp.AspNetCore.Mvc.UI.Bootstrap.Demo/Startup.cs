using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AbpAspNetCoreMvcUiBootstrapDemoModule>(options =>
            {
                options.UseAutofac();
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
                    .WriteTo.File("Logs/logs.txt")
                    .CreateLogger()
                );

            app.InitializeApplication();
        }
    }
}
