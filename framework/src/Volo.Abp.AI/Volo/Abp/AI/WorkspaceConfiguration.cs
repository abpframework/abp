using System;

namespace Volo.Abp.AI;

public class WorkspaceConfiguration
{
    public string Name { get; }
    public ChatClientConfiguration ChatClient { get; } = new();
    public KernelConfiguration Kernel { get; } = new();

    public WorkspaceConfiguration(string name)
    {
        Name = name;
    }

    public WorkspaceConfiguration ConfigureChatClient(Action<ChatClientConfiguration> configureAction)
    {
        configureAction(ChatClient);
        return this;
    }
    

    public WorkspaceConfiguration ConfigureKernel(Action<KernelConfiguration> configureAction)
    {
        configureAction(Kernel);
        return this;
    }
}