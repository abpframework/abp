using System;
using Autofac;
using JetBrains.Annotations;
using Volo;
using Volo.Abp;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpAutofacServiceCollectionExtensions
    {
        [NotNull]
        public static ContainerBuilder GetContainerBuilder([NotNull] this IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));

            var builder = services.GetObjectOrNull<ContainerBuilder>();
            if (builder == null)
            {
                throw new AbpException($"Could not find ContainerBuilder. Be sure that you have called {nameof(AbpAutofacAbpApplicationCreationOptionsExtensions.UseAutofac)} method before!");
            }

            return builder;
        }

        public static IServiceProvider BuildAutofacServiceProvider([NotNull] this IServiceCollection services, Action<ContainerBuilder> builderAction = null)
        {
            Check.NotNull(services, nameof(services));

            var serviceProviderFactory = services.GetSingletonInstanceOrNull<IServiceProviderFactory<ContainerBuilder>>();
            if (serviceProviderFactory == null)
            {
                throw new AbpException($"Could not find {typeof(IServiceProviderFactory<ContainerBuilder>).FullName} in {services}. Use {nameof(AbpAutofacAbpApplicationCreationOptionsExtensions.UseAutofac)} before!");
            }

            var builder = serviceProviderFactory.CreateBuilder(services);
            builderAction?.Invoke(builder);
            return serviceProviderFactory.CreateServiceProvider(builder);
        }
    }
}
