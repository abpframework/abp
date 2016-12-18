using System.Collections.Concurrent;
using System.Collections.Generic;
using Volo.DependencyInjection;
using Volo.ExtensionMethods.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    /* TODO: String IConventionalRegistrar objects in a static Dictionary
     * may cause a performance problem if we create too many short lived IServiceCollection instances.
     * Normally, an application will have a single IServiceCollection instance, but we should consider edge cases is there are.
     */

    public static class ServiceCollectionConventionalRegistrationExtensions
    {
        private static readonly ConcurrentDictionary<IServiceCollection, List<IConventionalRegistrar>> ConventionalRegistrars;

        static ServiceCollectionConventionalRegistrationExtensions()
        {
            ConventionalRegistrars = new ConcurrentDictionary<IServiceCollection, List<IConventionalRegistrar>>();
        }

        public static IServiceCollection AddConventionalRegistrar(this IServiceCollection services, IConventionalRegistrar registrar)
        {
            GetOrCreateRegistrarList(services).Add(registrar);
            return services;
        }
        
        internal static List<IConventionalRegistrar> GetConventionalRegistrars(this IServiceCollection services)
        {
            return GetOrCreateRegistrarList(services);
        }

        private static List<IConventionalRegistrar> GetOrCreateRegistrarList(IServiceCollection services)
        {
            return ConventionalRegistrars.GetOrAdd(
                services,
                () => new List<IConventionalRegistrar>
                {
                    new DefaultConventionalRegistrar()
                });
        }
    }
}