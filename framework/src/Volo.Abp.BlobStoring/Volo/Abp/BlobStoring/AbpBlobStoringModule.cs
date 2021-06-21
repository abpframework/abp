using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.BlobStoring
{
    [DependsOn(
        typeof(AbpMultiTenancyModule),
        typeof(AbpThreadingModule)
        )]
    public class AbpBlobStoringModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient(
                typeof(IBlobContainer<>),
                typeof(BlobContainer<>)
            );

            context.Services.AddTransient(
                typeof(IBlobContainer),
                serviceProvider => serviceProvider
                    .GetRequiredService<IBlobContainer<DefaultContainer>>()
            );
        }
    }
}