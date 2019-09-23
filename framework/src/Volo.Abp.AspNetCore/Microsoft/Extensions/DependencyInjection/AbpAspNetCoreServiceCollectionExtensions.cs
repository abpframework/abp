using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpAspNetCoreServiceCollectionExtensions
    {
        public static IWebHostEnvironment GetHostingEnvironment(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IWebHostEnvironment>();
        }

        public static IConfigurationRoot BuildConfiguration(this IServiceCollection services, ConfigurationBuilderOptions options = null)
        {
            return services.GetHostingEnvironment().BuildConfiguration(options);
        }

        public static IConfigurationRoot AddConfiguration(this IServiceCollection services, ConfigurationBuilderOptions options = null)
        {
            var configuration = services.BuildConfiguration(options);
            services.SetConfiguration(configuration);
            return configuration;
        }
    }
}
