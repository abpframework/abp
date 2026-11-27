using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.AgentTools;

[DependsOn(
    typeof(AbpAgentToolsModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAutofacModule)
    )]
public class AbpAgentToolsTestModule : AbpModule
{
}
