using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Features;
using Volo.Abp.Modularity;

namespace Volo.Abp.AgentTools;

[DependsOn(
    typeof(AbpAgentToolsAbstractionsModule),
    typeof(AbpFeaturesModule)
    )]
public class AbpAgentToolsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // PostConfigure: after everything the modules and the application set with Configure.
        context.Services.AddOptions<AbpAgentToolPolicyOptions>()
            .PostConfigure<IConfiguration>((options, configuration) => AgentToolPolicyConfigurationReader.Apply(configuration, options));

        // The configuration, else the code defaults; a module or the application replaces it with its own store.
        context.Services.TryAddTransient<IAgentToolPolicyStore, ConfigurationAgentToolPolicyStore>();
    }
}
