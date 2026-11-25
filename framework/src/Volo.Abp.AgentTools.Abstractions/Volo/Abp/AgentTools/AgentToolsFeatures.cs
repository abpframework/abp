namespace Volo.Abp.AgentTools;

public static class AgentToolsFeatures
{
    public const string GroupName = "AgentTools";

    /// <summary>Bool toggle, default "true". Off for a tenant: no tools, calls refused.</summary>
    public const string Enable = GroupName + ".Enable";
}
