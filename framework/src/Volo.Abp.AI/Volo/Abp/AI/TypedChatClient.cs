using System;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.AI;

public class TypedChatClient<TWorkSpace> : DelegatingChatClient, IChatClient<TWorkSpace>
    where TWorkSpace : class
{
    public TypedChatClient(IServiceProvider serviceProvider)
        : base(
            serviceProvider.GetRequiredKeyedService<IChatClient>(
                AbpAIWorkspaceOptions.GetChatClientServiceKeyName(
                    WorkspaceNameAttribute.GetWorkspaceName<TWorkSpace>()))
        )
    {
    }
}