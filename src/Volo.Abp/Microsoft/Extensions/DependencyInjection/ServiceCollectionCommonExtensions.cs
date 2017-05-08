using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Volo;
using Volo.Abp;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionCommonExtensions
    {
        public static T GetSingletonInstanceOrNull<T>(this IServiceCollection services)
        {
            return (T)services
                .FirstOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }
        
        public static T GetSingletonInstance<T>(this IServiceCollection services)
        {
            var service = services.GetSingletonInstanceOrNull<T>();
            if (service == null)
            {
                throw new InvalidOperationException("Could not find singleton service: " + typeof(T).AssemblyQualifiedName);
            }

            return service;
        }

	    public static IServiceProvider BuildServiceProviderFromFactory([NotNull] this IServiceCollection services)
	    {
		    Check.NotNull(services, nameof(services));

		    foreach (var service in services)
		    {
			    var factoryInterface = service.ImplementationInstance?.GetType()
				    .GetTypeInfo()
				    .GetInterfaces()
				    .FirstOrDefault(i => i.GetTypeInfo().IsGenericType &&
				                         i.GetGenericTypeDefinition() == typeof(IServiceProviderFactory<>));

			    if (factoryInterface == null)
			    {
				    continue;
			    }

			    var containerBuilderType = factoryInterface.GenericTypeArguments[0];
			    return (IServiceProvider)typeof(ServiceCollectionCommonExtensions)
					.GetTypeInfo()
				    .GetMethods()
					.Single(m => m.Name == nameof(BuildServiceProviderFromFactory) && m.IsGenericMethod)
				    .MakeGenericMethod(containerBuilderType)
				    .Invoke(null, new object[] { services, null });
		    }

		    return services.BuildServiceProvider();
	    }

	    public static IServiceProvider BuildServiceProviderFromFactory<TContainerBuilder>([NotNull] this IServiceCollection services, Action<TContainerBuilder> builderAction = null)
	    {
		    Check.NotNull(services, nameof(services));

		    var serviceProviderFactory = services.GetSingletonInstanceOrNull<IServiceProviderFactory<TContainerBuilder>>();
		    if (serviceProviderFactory == null)
		    {
			    throw new AbpException($"Could not find {typeof(IServiceProviderFactory<TContainerBuilder>).FullName} in {services}.");
		    }

		    var builder = serviceProviderFactory.CreateBuilder(services);
		    builderAction?.Invoke(builder);
		    return serviceProviderFactory.CreateServiceProvider(builder);
	    }
	}
}