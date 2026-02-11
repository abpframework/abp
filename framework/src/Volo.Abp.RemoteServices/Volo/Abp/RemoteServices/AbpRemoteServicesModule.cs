using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.RemoteServices;

[DependsOn(typeof(AbpMultiTenancyAbstractionsModule))]
public class AbpRemoteServicesModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<AbpRemoteServiceOptions>(configuration);
    }
}