using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.MultiLingualObjects;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Specifications;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain
{
    [DependsOn(
        typeof(AbpAuditingModule),
        typeof(AbpDataModule),
        typeof(AbpEventBusModule),
        typeof(AbpGuidsModule),
        typeof(AbpMultiTenancyModule),
        typeof(AbpThreadingModule),
        typeof(AbpTimingModule),
        typeof(AbpUnitOfWorkModule),
        typeof(AbpObjectMappingModule),
        typeof(AbpMultiLingualObjectsModule),
        typeof(AbpExceptionHandlingModule),
        typeof(AbpSpecificationsModule)
        )]
    public class AbpDddDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddConventionalRegistrar(new AbpRepositoryConventionalRegistrar());
        }
    }
}
