using Microsoft.AspNetCore.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpAspNetCoreServiceCollectionExtensions
{
    public static IWebHostEnvironment GetHostingEnvironment(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IWebHostEnvironment>();
    }
}
