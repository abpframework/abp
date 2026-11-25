using Volo.Abp.AgentTools.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AgentTools;

[DependsOn(typeof(AbpLocalizationModule))]
public class AbpAgentToolsAbstractionsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAgentToolsAbstractionsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpAgentToolsResource>("en")
                .AddVirtualJson("/Volo/Abp/AgentTools/Localization");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("AgentTools", typeof(AbpAgentToolsResource));
        });
    }
}
