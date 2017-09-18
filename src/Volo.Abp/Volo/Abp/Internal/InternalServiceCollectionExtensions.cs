using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.Internal
{
    internal static class InternalServiceCollectionExtensions
    {
        internal static void AddCoreAbpServices(this IServiceCollection services, IAbpApplication abpApplication)
        {
            var moduleLoader = new ModuleLoader();
            var assemblyFinder = new AssemblyFinder(abpApplication);
            var typeFinder = new TypeFinder(assemblyFinder);

            services.TryAddSingleton<IModuleLoader>(moduleLoader);
            services.TryAddSingleton<IAssemblyFinder>(assemblyFinder);
            services.TryAddSingleton<ITypeFinder>(typeFinder);
        }
    }
}