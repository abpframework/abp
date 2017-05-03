using System.Linq;
using System.Reflection;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Volo.Abp.Application.Services;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Uow;

namespace Autofac.Builder
{
    public static class AbpRegistrationBuilderExtensions
    {
        public static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> ApplyAbpConcepts<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registrationBuilder)
            where TConcreteReflectionActivatorData : ConcreteReflectionActivatorData
        {
            //TODO: Refactor to an extensible way!

            var serviceType = registrationBuilder.RegistrationData.Services.OfType<IServiceWithType>().FirstOrDefault()?.ServiceType;

            if (serviceType != null)
            {
                if (typeof(IApplicationService).IsAssignableFrom(serviceType))
                {
                    if (serviceType.GetTypeInfo().IsInterface)
                    {
                        registrationBuilder = registrationBuilder.EnableInterfaceInterceptors();
                    }
                    else
                    {
                        registrationBuilder = registrationBuilder.EnableClassInterceptors();
                    }

                    registrationBuilder.InterceptedBy(
                        typeof(CastleAbpInterceptorAdapter<>).MakeGenericType(typeof(UnitOfWorkInterceptor))
                    );
                }
            }

            return registrationBuilder;
        }
    }
}
