using Volo.Abp.Modularity;

namespace Volo.Abp.Studio
{
    [DependsOn(
        typeof(AbpStudioDomainCommonServicesModule),
        typeof(AbpStudioModuleInstallerAbstractionsModule)
        )]
    public class AbpStudioModuleInstallerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
