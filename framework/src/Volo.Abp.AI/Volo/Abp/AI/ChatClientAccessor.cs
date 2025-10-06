using System;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AI;

[Dependency(ReplaceServices = true, TryRegister = true)]
[ExposeServices(typeof(IChatClientAccessor))]
public class ChatClientAccessor : IChatClientAccessor
{
    public IChatClient? ChatClient { get; }

    public ChatClientAccessor(IServiceProvider serviceProvider)
    {
        ChatClient = serviceProvider.GetKeyedService<IChatClient>(
            AbpAIWorkspaceOptions.GetChatClientServiceKeyName(
                AbpAIModule.DefaultWorkspaceName));
    }
}

[Dependency(ReplaceServices = true, TryRegister = true)]
[ExposeServices(typeof(IChatClientAccessor))]
public class ChatClientAccessor<TWorkSpace> : IChatClientAccessor<TWorkSpace>
    where TWorkSpace : class
{
    public IChatClient? ChatClient { get; }

    public ChatClientAccessor(IServiceProvider serviceProvider)
    {
        ChatClient = serviceProvider.GetKeyedService<IChatClient>(
            AbpAIWorkspaceOptions.GetChatClientServiceKeyName(
                WorkspaceNameAttribute.GetWorkspaceName<TWorkSpace>()));
    }
}