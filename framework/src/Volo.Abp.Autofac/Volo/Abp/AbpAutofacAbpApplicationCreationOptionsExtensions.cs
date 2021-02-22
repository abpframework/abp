using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Autofac;

namespace Volo.Abp
{
    public static class AbpAutofacAbpApplicationCreationOptionsExtensions
    {
        static NullLogger<object> Logger = NullLogger<object>.Instance;

        public static void UseAutofac(this AbpApplicationCreationOptions options)
        {
            System.Diagnostics.Debug.WriteLine("装载 Autofac");
            options.Services.AddAutofacServiceProviderFactory();
        }

        public static AbpAutofacServiceProviderFactory AddAutofacServiceProviderFactory(this IServiceCollection services)
        {
            ContainerBuilder container = new ContainerBuilder();
            return services.AddAutofacServiceProviderFactory(container);
        }

        public static AbpAutofacServiceProviderFactory AddAutofacServiceProviderFactory(this IServiceCollection services, ContainerBuilder containerBuilder)
        {
            AbpAutofacServiceProviderFactory factory = new AbpAutofacServiceProviderFactory(containerBuilder);

            services.AddObjectAccessor(containerBuilder);
            services.AddSingleton((IServiceProviderFactory<ContainerBuilder>)factory);

            return factory;
        }
    }
}
