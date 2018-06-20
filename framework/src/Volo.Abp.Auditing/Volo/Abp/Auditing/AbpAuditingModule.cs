using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security;
using Volo.Abp.Timing;

namespace Volo.Abp.Auditing
{
    //TODO: Can we remove AbpDataModule dependency since it only contains ISoftDelete related to auditing module!
    [DependsOn(
        typeof(AbpDataModule),
        typeof(AbpTimingModule),
        typeof(AbpSecurityModule),
        typeof(AbpMultiTenancyAbstractionsModule)
        )]
    public class AbpAuditingModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAuditingModule>();
        }
    }
}
