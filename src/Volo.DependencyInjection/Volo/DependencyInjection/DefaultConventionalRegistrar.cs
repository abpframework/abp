using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Volo.DependencyInjection
{
    public class DefaultConventionalRegistrar : ConventionalRegistrarBase
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsDefined(typeof(DisableConventionalRegistrationAttribute), true))
            {
                return;
            }

            var dependencyAttribute = typeInfo.GetCustomAttribute<DependencyAttribute>(true);

            var lifeTime = dependencyAttribute?.Lifetime ?? GetServiceLifetimeFromInterfaces(type);
            if (lifeTime == null)
            {
                return;
            }

            //TODO: Should have a list of ignored interfaces for exposed services. Say that MyTransientDependency class should not registered for ITransientDependency interface.

            foreach (var serviceType in AutoRegistrationHelper.GetExposedServices(type))
            {
                var serviceDescriptor = ServiceDescriptor.Describe(serviceType, type, lifeTime.Value);

                if (dependencyAttribute?.TryRegister == true)
                {
                    services.TryAdd(serviceDescriptor);
                }
                else
                {
                    services.Add(serviceDescriptor);
                }
            }
        }

        protected virtual ServiceLifetime? GetServiceLifetimeFromInterfaces(Type type)
        {
            if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Transient;
            }

            if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Singleton;
            }

            if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Scoped;
            }

            return null;
        }
    }
}