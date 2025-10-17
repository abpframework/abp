using Microsoft.Extensions.AI;
using Volo.Abp.AI;
using Volo.Abp.AI.Mocks;
using Volo.Abp.AI.Tests.Workspaces;
using Volo.Abp.Modularity;

namespace Volo.Abp.AutoMapper;

[DependsOn(
    typeof(AbpTestBaseModule),
    typeof(AbpAIModule)
)]
public class AbpAITestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAIWorkspaceOptions>(options =>
        {
            options.Workspaces.ConfigureDefault(options =>
            {
                options.ConfigureChatClient(clientOptions =>
                {
                    clientOptions.Builder = new ChatClientBuilder(new MockDefaultChatClient());
                });
            });

            options.Workspaces.Configure<WordCounter>(workspaceOptions =>
            {
                workspaceOptions.ConfigureChatClient(clientOptions =>
                {
                    clientOptions.Builder = new ChatClientBuilder(new MockChatClient());
                });
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}
