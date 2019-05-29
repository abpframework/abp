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
        typeof(DataModule),
        typeof(JsonModule),
        typeof(TimingModule),
        typeof(SecurityModule),
        typeof(ThreadingModule),
        typeof(MultiTenancyModule)
        )]
    public class AuditingModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(AuditingInterceptorRegistrar.RegisterIfNeeded);
        }
    }
}
