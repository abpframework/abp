using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace Autofac.Builder
{
    public static class AbpRegistrationBuilderExtensions
    {
        public static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> ConfigureAbpConventions<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registrationBuilder, 
                IModuleContainer moduleContainer, 
                ServiceRegistrationActionList registrationActionList)
            where TConcreteReflectionActivatorData : ConcreteReflectionActivatorData
        {
            var serviceType = registrationBuilder.RegistrationData.Services.OfType<IServiceWithType>().FirstOrDefault()?.ServiceType;
            if (serviceType == null)
            {
                return registrationBuilder;
            }

            var implementationType = registrationBuilder.ActivatorData.ImplementationType;
            if (implementationType == null)
            {
                return registrationBuilder;
            }

            registrationBuilder = registrationBuilder.EnablePropertyInjection(moduleContainer, implementationType);
            registrationBuilder = registrationBuilder.InvokeRegistrationActions(registrationActionList, serviceType, implementationType);

            return registrationBuilder;
        }

        private static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> InvokeRegistrationActions<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registrationBuilder, ServiceRegistrationActionList registrationActionList, Type serviceType, Type implementationType) 
            where TConcreteReflectionActivatorData : ConcreteReflectionActivatorData
        {
            var serviceRegistredArgs = new OnServiceRegistredContext(serviceType, implementationType);

            foreach (var registrationAction in registrationActionList)
            {
                registrationAction.Invoke(serviceRegistredArgs);
            }

            if (serviceRegistredArgs.Interceptors.Any())
            {
                registrationBuilder = registrationBuilder.AddInterceptors(
                    serviceType,
                    serviceRegistredArgs.Interceptors
                );
            }

            return registrationBuilder;
        }

        private static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> EnablePropertyInjection<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registrationBuilder, 
                IModuleContainer moduleContainer,
                Type implementationType) 
            where TConcreteReflectionActivatorData : ConcreteReflectionActivatorData
        {
            //Enable Property Injection only for types in an assembly containing an AbpModule
            if (moduleContainer.Modules.Any(m => m.Assembly == implementationType.Assembly))
            {
                registrationBuilder = registrationBuilder.PropertiesAutowired();
            }

            return registrationBuilder;
        }

        private static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> AddInterceptors<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registrationBuilder, 
            Type serviceType,
            IEnumerable<Type> interceptors)
            where TConcreteReflectionActivatorData : ConcreteReflectionActivatorData
        {
            registrationBuilder = serviceType.GetTypeInfo().IsInterface
                ? registrationBuilder.EnableInterfaceInterceptors()
                : registrationBuilder.EnableClassInterceptors();

            foreach (var interceptor in interceptors)
            {
                registrationBuilder.InterceptedBy(
                    typeof(CastleAbpInterceptorAdapter<>).MakeGenericType(interceptor)
                );
            }

            return registrationBuilder;
        }
    }
}
