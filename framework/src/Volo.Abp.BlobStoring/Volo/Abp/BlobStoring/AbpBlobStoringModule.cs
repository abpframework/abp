using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring
{
    public class AbpBlobStoringModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient(
                typeof(IBlobContainer<>),
                typeof(TypedBlobContainerWrapper<>)
            );

            context.Services.AddTransient(
                typeof(IBlobContainer),
                serviceProvider => serviceProvider
                    .GetRequiredService<IBlobContainer<DefaultContainer>>()
            );
        }
    }
}