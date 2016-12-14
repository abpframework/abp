using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace Volo.Abp.Internal
{
    internal static class InternalServiceCollectionExtensions
    {
        internal static void AddCoreAbpServices(this IServiceCollection services, AbpApplication application)
        {
            services.AddSingleton(application);
            services.TryAddSingleton<IModuleLoader>(new ModuleLoader());
        }
    }
}