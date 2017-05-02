using Autofac;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp
{
    public static class AbpApplicationCreationOptionsAutofacExtensions
    {
        public static void UseAutofac(this AbpApplicationCreationOptions options)
        {
            var builder = new ContainerBuilder();
            options.Services.AddObjectAccessor(builder);
            options.Services.AddSingleton((IServiceProviderFactory<ContainerBuilder>) new AbpAutofacServiceProviderFactory(builder));
        }

        [NotNull]
        public static ContainerBuilder GetContainerBuilder([NotNull] this IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));

            var builder = services.GetObjectOrNull<ContainerBuilder>();
            if (builder == null)
            {
                throw new AbpException($"Could not find ContainerBuilder. Be sure that you have called {nameof(UseAutofac)} method before!");
            }

            return builder;
        }
    }
}
