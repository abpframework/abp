using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Volo.Abp;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpWebAssemblyServiceCollectionExtensions
    {
        public static WebAssemblyHostBuilder GetHostBuilder(
            [NotNull] this IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));

            return services.GetSingletonInstance<WebAssemblyHostBuilder>();
        }
    }
}
