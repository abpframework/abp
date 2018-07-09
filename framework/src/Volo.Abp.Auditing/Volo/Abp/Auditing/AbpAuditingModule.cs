using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.Auditing
{
    [DependsOn(
        typeof(AbpDataModule),
        typeof(AbpJsonModule),
        typeof(AbpTimingModule),
        typeof(AbpSecurityModule),
        typeof(AbpThreadingModule),
        typeof(AbpMultiTenancyAbstractionsModule)
        )]
    public class AbpAuditingModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(AuditingInterceptorRegistrar.RegisterIfNeeded);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpAuditingModule>();
        }
    }
}
