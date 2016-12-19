using System.Collections.Generic;
using Volo.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /* TODO: String IConventionalRegistrar objects in a static Dictionary
     * may cause a performance problem if we create too many short lived IServiceCollection instances.
     * Normally, an application will have a single IServiceCollection instance, but we should consider edge cases is there are.
     */

    public static class ServiceCollectionConventionalRegistrationExtensions
    {
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
            var conventionalRegistrars = services.GetSingletonInstanceOrNull<IObjectAccessor<ConventionalRegistrarList>>()?.Value;
            if (conventionalRegistrars == null)
            {
                conventionalRegistrars = new ConventionalRegistrarList { new DefaultConventionalRegistrar() };
                services.AddObjectAccessor(conventionalRegistrars);
            }

            return conventionalRegistrars;
        }
    }
}