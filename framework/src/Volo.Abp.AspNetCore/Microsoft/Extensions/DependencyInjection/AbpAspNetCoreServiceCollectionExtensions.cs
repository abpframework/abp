using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpAspNetCoreServiceCollectionExtensions
    {
        public static IHostingEnvironment GetHostingEnvironment(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IHostingEnvironment>();
        }

        public static IConfigurationRoot BuildConfiguration(this IServiceCollection services, AbpAspNetCoreConfigurationOptions options = null)
        {
            return services.GetHostingEnvironment().BuildConfiguration(options);
        }

        public static IConfigurationRoot AddConfiguration(this IServiceCollection services, AbpAspNetCoreConfigurationOptions options = null)
        {
            var configuration = services.BuildConfiguration(options);
            services.SetConfiguration(configuration);
            return configuration;
        }
    }
}
