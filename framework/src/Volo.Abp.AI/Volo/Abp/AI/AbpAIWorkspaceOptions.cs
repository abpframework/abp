namespace Volo.Abp.AI;

/// <summary>
/// Pre-configured options for the AI workspaces. Not used via Options pattern. Use it with 'PreConfigure' method in a Module class.
/// In example:
/// <code>PreConfigure&lt;AbpAIWorkspaceOptions&gt;(options => { });</code>
/// </summary>
public class AbpAIWorkspaceOptions
{
    public const string ChatClientServiceKeyNamePrefix = "Abp.AI.ChatClient_";
    public const string KernelServiceKeyNamePrefix = "Abp.AI.Kernel_";
    
    public WorkspaceConfigurationDictionary Workspaces { get; } = new();

    public static string GetChatClientServiceKeyName(string name)
    {
        return $"{ChatClientServiceKeyNamePrefix}{name}";
    }

    public static string GetKernelServiceKeyName(string name)
    {
        return $"{KernelServiceKeyNamePrefix}{name}";
    }
}