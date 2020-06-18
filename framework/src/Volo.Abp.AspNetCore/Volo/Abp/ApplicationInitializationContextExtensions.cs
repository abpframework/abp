using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp
{
    public static class ApplicationInitializationContextExtensions
    {
        public static IApplicationBuilder GetApplicationBuilder(this ApplicationInitializationContext context)
        {
            return context.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;
        }

        public static IWebHostEnvironment GetEnvironment(this ApplicationInitializationContext context)
        {
            return context.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        }

        public static IConfiguration GetConfiguration(this ApplicationInitializationContext context)
        {
            return context.ServiceProvider.GetRequiredService<IConfiguration>();
        }

        public static ILoggerFactory GetLoggerFactory(this ApplicationInitializationContext context)
        {
            return context.ServiceProvider.GetRequiredService<ILoggerFactory>();
        }
    }
}
