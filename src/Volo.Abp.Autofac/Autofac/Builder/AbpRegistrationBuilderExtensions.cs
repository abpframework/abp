using System;
using System.Linq;
using System.Reflection;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.DependencyInjection;

namespace Autofac.Builder
{
    public static class AbpRegistrationBuilderExtensions
    {
        public static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> ConfigureAbpConventions<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registrationBuilder,
            IServiceCollection services)
            where TConcreteReflectionActivatorData : ConcreteReflectionActivatorData
        {
            registrationBuilder = registrationBuilder.PropertiesAutowired();

            var serviceType = registrationBuilder.RegistrationData.Services.OfType<IServiceWithType>().FirstOrDefault()?.ServiceType;
            if (serviceType == null)
            {
                return registrationBuilder;
            }

            var serviceRegistredArgs = new OnServiceRegistredContext(serviceType);

            foreach (var registrationAction in services.GetRegistrationActionList())
            {
                registrationAction.Invoke(serviceRegistredArgs);
            }

            if (serviceRegistredArgs.Interceptors.Any())
            {
                registrationBuilder = registrationBuilder.AddInterceptors(serviceRegistredArgs.Interceptors.ToArray());
            }

            return registrationBuilder;
        }

        private static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> AddInterceptors<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registrationBuilder, 
            Type[] interceptors)
            where TConcreteReflectionActivatorData : ConcreteReflectionActivatorData
        {
            var serviceType = registrationBuilder.RegistrationData.Services.OfType<IServiceWithType>().FirstOrDefault()?.ServiceType;

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
