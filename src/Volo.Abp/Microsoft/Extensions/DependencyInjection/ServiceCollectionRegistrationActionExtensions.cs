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
            var actionList = services.GetSingletonInstanceOrNull<IObjectAccessor<ServiceRegistrationActionList>>()?.Value;
            if (actionList == null)
            {
                actionList = new ServiceRegistrationActionList();
                services.AddObjectAccessor(actionList);
            }

            return actionList;
        }

        public static void OnServiceExposing(this IServiceCollection services, Action<IOnServiceExposingArgs> exposeAction)
        {
            GetOrCreateOnServiceExposingList(services).Add(exposeAction);
        }

        public static ServiceExposingActionList GetServiceExposingActionList(this IServiceCollection services)
        {
            return GetOrCreateOnServiceExposingList(services);
        }

        private static ServiceExposingActionList GetOrCreateOnServiceExposingList(IServiceCollection services)
        {
            var actionList = services.GetSingletonInstanceOrNull<IObjectAccessor<ServiceExposingActionList>>()?.Value;
            if (actionList == null)
            {
                actionList = new ServiceExposingActionList();
                services.AddObjectAccessor(actionList);
            }

            return actionList;
        }
    }
}
