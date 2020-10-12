﻿using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.EventBus;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
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
        typeof(AbpExceptionHandlingModule)
        )]
    public class AbpDddDomainModule : AbpModule
    {

    }
}
