using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Modularity
{
    public static class ApplicationInitializationContextExtensions
    {
        public static IApplicationBuilder GetApplicationBuilder(this ApplicationInitializationContext context)
        {
            return context.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;
        }

        public static IHostingEnvironment GetEnvironment(this ApplicationInitializationContext context)
        {
            return context.ServiceProvider.GetRequiredService<IHostingEnvironment>();
        }

        public static ILoggerFactory GetLoggerFactory(this ApplicationInitializationContext context)
        {
            return context.ServiceProvider.GetRequiredService<ILoggerFactory>();
        }
    }
}
