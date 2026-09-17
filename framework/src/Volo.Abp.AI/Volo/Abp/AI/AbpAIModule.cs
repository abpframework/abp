using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.SemanticKernel;
using Volo.Abp.Modularity;

namespace Volo.Abp.AI;

[DependsOn(
    typeof(AbpAIAbstractionsModule)
)]
public class AbpAIModule : AbpModule
{
    public const string DefaultWorkspaceName = "Default";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var options = context.Services.ExecutePreConfiguredActions<AbpAIWorkspaceOptions>();

        context.Services.Configure<AbpAIOptions>(workspaceOptions =>
        {
            workspaceOptions.ConfiguredWorkspaceNames.AddIfNotContains(
                options.Workspaces.Select(x => x.Key)
            );
        });

        foreach (var workspaceConfig in options.Workspaces.Values)
        {
            ConfigureChatClient(context, workspaceConfig);
            ConfigureKernel(context, workspaceConfig);
        }

        context.Services.TryAddTransient(typeof(IChatClient<>), typeof(TypedChatClient<>));
        context.Services.TryAddTransient(typeof(IChatClientAccessor<>), typeof(ChatClientAccessor<>));
        context.Services.TryAddTransient(typeof(IKernelAccessor<>), typeof(KernelAccessor<>));
    }

    private static void ConfigureKernel(ServiceConfigurationContext context, WorkspaceConfiguration workspaceConfig)
    {
        if (workspaceConfig.Kernel.Builder is null)
        {
            return;
        }

        foreach (var builderConfigurer in workspaceConfig.Kernel.BuilderConfigurers)
        {
            builderConfigurer.Action(workspaceConfig.Kernel.Builder!);
        }

        // TODO: Check if we can use transient instead of singleton for Kernel
        context.Services.AddKeyedTransient<Kernel>(
            AbpAIWorkspaceOptions.GetKernelServiceKeyName(workspaceConfig.Name),
            (provider, _) => workspaceConfig.Kernel.Builder!.Build());

        if (workspaceConfig.Name == DefaultWorkspaceName)
        {
            context.Services.AddTransient<Kernel>(sp => sp.GetRequiredKeyedService<Kernel>(
                    AbpAIWorkspaceOptions.GetKernelServiceKeyName(workspaceConfig.Name)
                )
            );
        }

        if (workspaceConfig.ChatClient?.Builder is null)
        {
            context.Services.AddKeyedTransient<IChatClient>(
                AbpAIWorkspaceOptions.GetChatClientServiceKeyName(workspaceConfig.Name),
                (sp, _) => sp.GetKeyedService<Kernel>(AbpAIWorkspaceOptions.GetKernelServiceKeyName(workspaceConfig.Name))?
                    .GetRequiredService<IChatClient>()
                        ?? throw new InvalidOperationException("Kernel or IChatClient not found with workspace name: " + workspaceConfig.Name)
            );
        }
    }

    private static void ConfigureChatClient(ServiceConfigurationContext context, WorkspaceConfiguration workspaceConfig)
    {
        if (workspaceConfig.ChatClient.Builder is null)
        {
            return;
        }

        foreach (var builderConfigurer in workspaceConfig.ChatClient.BuilderConfigurers)
        {
            builderConfigurer.Action(workspaceConfig.ChatClient.Builder);
        }

        var serviceName = AbpAIWorkspaceOptions.GetChatClientServiceKeyName(workspaceConfig.Name);

        context.Services.AddKeyedChatClient(
            serviceName,
            provider => workspaceConfig.ChatClient.Builder.Build(provider),
            ServiceLifetime.Transient
        );

        if (workspaceConfig.Name == DefaultWorkspaceName)
        {
            context.Services.AddTransient<IChatClient>(
                sp => sp.GetRequiredKeyedService<IChatClient>(serviceName)
            );
        }

        if (workspaceConfig.Kernel.Builder is null)
        {
            context.Services.AddKeyedTransient<Kernel>(
                AbpAIWorkspaceOptions.GetKernelServiceKeyName(workspaceConfig.Name),
                (sp, _) =>
                {
                    var chatClient = sp.GetRequiredKeyedService<IChatClient>(serviceName);
                    var builder = Kernel.CreateBuilder();
                    builder.Services.AddSingleton<IChatClient>(chatClient);
                    return builder.Build();
                }
            );
        }
    }
}