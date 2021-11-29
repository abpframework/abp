using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.SignalR
{
    [DependsOn(
        typeof(AbpAspNetCoreSignalRModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpAspNetCoreSignalRTestModule : AbpModule
    {
        
    }
}
