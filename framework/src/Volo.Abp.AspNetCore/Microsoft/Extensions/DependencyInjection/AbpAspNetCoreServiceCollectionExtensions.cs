using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpAspNetCoreServiceCollectionExtensions
    {
        public static IWebHostEnvironment GetHostingEnvironment(this IServiceCollection services)
        {
            if (!services.Any(d => d.ServiceType == typeof(IWebHostEnvironment)))
            {
                return new EmptyHostingEnvironment()
                {
                    EnvironmentName = Environments.Development
                };
            }

            return services.GetSingletonInstance<IWebHostEnvironment>();
        }
    }
}
