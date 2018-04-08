using Microsoft.AspNetCore.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpAspNetCoreServiceCollectionExtensions
    {
        public static IHostingEnvironment GetHostingEnvironment(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IHostingEnvironment>();
        }
    }
}
