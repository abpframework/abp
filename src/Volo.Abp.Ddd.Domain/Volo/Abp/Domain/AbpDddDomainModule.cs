using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EventBus;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain
{
    [DependsOn(
        typeof(AbpDataModule),
        typeof(AbpEventBusModule),
        typeof(AbpGuidsModule),
        typeof(AbpMultiTenancyAbstractionsModule),
        typeof(AbpThreadingModule),
        typeof(AbpTimingModule),
        typeof(AbpUnitOfWorkModule))]
    public class AbpDddDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpDddDomainModule>();
        }
    }
}
