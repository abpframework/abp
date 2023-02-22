using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpWebAssemblyServiceCollectionExtensions
{
    public static WebAssemblyHostBuilder GetHostBuilder([NotNull] this IServiceCollection services)
    {
        Check.NotNull(services, nameof(services));

        return services.GetSingletonInstance<WebAssemblyHostBuilder>();
    }

    public static IWebAssemblyHostEnvironment GetWebAssemblyHostEnvironment(this IServiceCollection services)
    {
        var webAssemblyHostEnvironment = services.GetSingletonInstanceOrNull<IWebAssemblyHostEnvironment>();

        if (webAssemblyHostEnvironment == null)
        {
            return new EmptyWebAssemblyHostEnvironment()
            {
                Environment = Environments.Development
            };
        }

        return webAssemblyHostEnvironment;
    }
}
