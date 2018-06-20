using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;

namespace Volo.Abp
{
    public static class AbpAutofacAbpApplicationCreationOptionsExtensions
    {
        public static void UseAutofac(this AbpApplicationCreationOptions options)
        {
            var builder = new ContainerBuilder();
            options.Services.AddObjectAccessor(builder);
            options.Services.AddSingleton((IServiceProviderFactory<ContainerBuilder>) new AbpAutofacServiceProviderFactory(builder));
        }
    }
}
