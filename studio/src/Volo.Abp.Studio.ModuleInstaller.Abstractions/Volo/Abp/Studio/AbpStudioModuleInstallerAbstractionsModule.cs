using Volo.Abp.Modularity;
using Volo.Abp.Studio.Analyzing;

namespace Volo.Abp.Studio;

[DependsOn(
    typeof(AbpStudioAnalyzingAbstractionsModule)
)]
public class AbpStudioModuleInstallerAbstractionsModule : AbpModule
{

}
