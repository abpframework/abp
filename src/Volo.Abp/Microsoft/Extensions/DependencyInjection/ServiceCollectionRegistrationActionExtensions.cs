using System;
using Volo.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionRegistrationActionExtensions
    {
        public static void OnServiceRegistred(this IServiceCollection services, Action<IOnServiceRegistredArgs> registrationAction)
        {
            GetOrCreateServiceActionList(services).Add(registrationAction);
        }

        public static ServiceRegistrationActionList GetServiceRegistrationActionList(this IServiceCollection services)
        {
            return GetOrCreateServiceActionList(services);
        }

        private static ServiceRegistrationActionList GetOrCreateServiceActionList(IServiceCollection services)
        {
            var registrationActionList = services.GetSingletonInstanceOrNull<IObjectAccessor<ServiceRegistrationActionList>>()?.Value;
            if (registrationActionList == null)
            {
                registrationActionList = new ServiceRegistrationActionList();
                services.AddObjectAccessor(registrationActionList);
            }

            return registrationActionList;
        }
    }
}
