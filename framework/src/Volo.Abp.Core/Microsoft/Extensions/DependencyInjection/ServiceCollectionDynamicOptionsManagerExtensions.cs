using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionDynamicOptionsManagerExtensions
    {
        public static IServiceCollection AddAbpDynamicOptions<TOptions, TManager>(this IServiceCollection services)
            where TOptions : class
            where TManager : AbpDynamicOptionsManager<TOptions>
        {
            services.Replace(ServiceDescriptor.Scoped(typeof(IOptions<TOptions>), typeof(TManager)));
            services.Replace(ServiceDescriptor.Scoped(typeof(IOptionsSnapshot<TOptions>), typeof(TManager)));

            return services;
        }
    }
}
