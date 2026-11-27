namespace Volo.Abp.AgentTools;

/// <summary>What a tool does, derived from the HTTP verb unless <see cref="AgentToolAttribute"/> says otherwise.</summary>
public enum AgentToolKind
{
    /// <summary>GET: reads data, no side effects.</summary>
    Read = 0,

    /// <summary>POST, PUT, PATCH: changes data.</summary>
    Write = 1,

    /// <summary>DELETE: removes data.</summary>
    Destructive = 2
}
