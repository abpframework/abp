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

        public static IConfigurationRoot BuildConfiguration(this IServiceCollection services, string fileName = "appsettings")
        {
            return services.GetHostingEnvironment().BuildConfiguration(fileName);
        }

        public static IConfigurationRoot AddConfiguration(this IServiceCollection services, string fileName = "appsettings")
        {
            var configuration = services.BuildConfiguration(fileName);
            services.AddConfiguration(configuration);
            return configuration;
        }
    }
}
