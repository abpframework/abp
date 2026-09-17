using System;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AI;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IChatClientAccessor))]
public class ChatClientAccessor : IChatClientAccessor, ITransientDependency
{
    public IChatClient? ChatClient { get; }

    public ChatClientAccessor(IServiceProvider serviceProvider)
    {
        ChatClient = serviceProvider.GetKeyedService<IChatClient>(
            AbpAIWorkspaceOptions.GetChatClientServiceKeyName(
                AbpAIModule.DefaultWorkspaceName));
    }
}

public class ChatClientAccessor<TWorkSpace> : IChatClientAccessor<TWorkSpace>
    where TWorkSpace : class
{
    public IChatClient? ChatClient { get; }

    public ChatClientAccessor(IServiceProvider serviceProvider)
    {
        ChatClient = serviceProvider.GetKeyedService<IChatClient>(
            AbpAIWorkspaceOptions.GetChatClientServiceKeyName(
                WorkspaceNameAttribute.GetWorkspaceName<TWorkSpace>()));

        // Fallback to default chat client if not configured for the workspace.
        if (ChatClient is null)
        {
            ChatClient = serviceProvider.GetKeyedService<IChatClient>(
                AbpAIWorkspaceOptions.GetChatClientServiceKeyName(
                    AbpAIModule.DefaultWorkspaceName));
        }
    }
}