using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using OpenIddict.Abstractions;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// OpenIddict defaults for the MCP endpoint (package Volo.Abp.AgentTools.Mcp). Read when the OpenIddict options are built, so set
/// them with <c>Configure&lt;AbpAgentToolsOpenIddictOptions&gt;</c> in ConfigureServices.
/// </summary>
public class AbpAgentToolsOpenIddictOptions
{
    // Scopes about the user, not about an API.
    private static readonly HashSet<string> StandardScopes =
    [
        OpenIddictConstants.Scopes.OpenId,
        OpenIddictConstants.Scopes.Profile,
        OpenIddictConstants.Scopes.Email,
        OpenIddictConstants.Scopes.Phone,
        OpenIddictConstants.Scopes.Address,
        OpenIddictConstants.Scopes.Roles,
        OpenIddictConstants.Scopes.OfflineAccess
    ];

    /// <summary>Configuration section of <see cref="AgentToolsMcpClientSeedContributor"/>: client id → client settings.</summary>
    public const string ClientsConfigurationSection = "AgentTools:Mcp:Clients";

    /// <summary>
    /// Absolute URL of the MCP endpoint, exactly as clients address it (RFC 8707 resource, RFC 9728 <c>resource</c>). It is
    /// registered with the OpenIddict server, granted as an <c>rsrc:</c> (resource) permission to seeded clients and (see
    /// <see cref="SyncMcpResourcePermission"/>) to every application with the scope and, by default, used as the scope's resource
    /// (the token audience). Default: configuration <c>App:SelfUrl</c> + "/mcp"; set it when the MCP path
    /// (AbpAgentToolsMcpOptions.Path) or the public URL differs. Null: nothing is registered and a web host fails to start
    /// (only a warning when <see cref="SyncMcpResourcePermission"/> is off).
    /// </summary>
    public string? McpResource { get; set; }

    /// <summary>The scope MCP clients request; keep it equal to AbpAgentToolsMcpOptions.RequiredScope. Default "AgentTools".</summary>
    public string ScopeName { get; set; } = "AgentTools";

    /// <summary>
    /// Resources stored on the seeded scope. ABP builds a token's <c>aud</c> from the requested scopes' resources, so these must
    /// include an audience the resource server accepts. Empty (default) = [<see cref="McpResource"/>]; the module adds that URL to
    /// the OpenIddict validation audiences of this host. Keep the default, and restrict the audiences that each API accepts
    /// (AddAudiences, as ABP's templates do): an audience that another API accepts makes every MCP token a full token of that API,
    /// which bypasses the MCP endpoint's rules. Only OpenIddict validation in a host with this module refuses a token with only this
    /// scope there (<see cref="ConfineScopeToMcpEndpoint"/>); a JwtBearer host accepts the MCP URL at the endpoint only: a second
    /// AddJwtBearer scheme with the MCP URL as its audience, set as AbpAgentToolsMcpOptions.AuthenticationScheme. Only used when the
    /// scope is created.
    /// </summary>
    public List<string> ScopeResources { get; } = new();

    /// <summary>
    /// Remove the "plain" PKCE method so the server accepts S256 only (OAuth 2.1, MCP authorization). Affects every client of the
    /// application, so opt-in. Default false.
    /// </summary>
    public bool RequireS256CodeChallengeMethod { get; set; }

    /// <summary>
    /// Turn on OpenIddict validation's token entry validation, so a token revoked individually (RFC 7009, or
    /// IOpenIddictTokenManager.TryRevokeAsync) is refused at once instead of at expiry; revoke the tokens of an application
    /// individually before you delete it. App-wide: one read of the token entry per bearer request. With more than one instance,
    /// configure a shared distributed cache. Default true. False leaves the app's own setting untouched.
    /// </summary>
    public bool EnableTokenEntryValidation { get; set; } = true;

    /// <summary>
    /// Keep the <c>rsrc:</c> (resource) permission for <see cref="McpResource"/> in step with the scope permission for
    /// <see cref="ScopeName"/> on every OpenIddict application: added when an application is created or updated with the scope
    /// (whatever creates it, e.g. a UI without a resources field), removed (only that permission) when the scope is taken away,
    /// and fixed for existing applications when data is seeded. OpenIddict refuses resource=&lt;MCP URL&gt; (RFC 8707, sent by MCP
    /// clients such as Claude Code) from a client without it. When on, <see cref="McpResource"/> must be known, otherwise a web
    /// host fails to start. Default true.
    /// </summary>
    public bool SyncMcpResourcePermission { get; set; } = true;

    /// <summary>
    /// Accept a token whose only API scope is <see cref="ScopeName"/> at the MCP endpoint only, whatever its audiences
    /// (<see cref="AgentToolsMcpTokenAudienceHandler"/>), and refuse a token request that leaves the scope out of a grant without
    /// another API scope (<see cref="AgentToolsScopeNarrowingHandler"/>). The standard scopes (openid, profile, email, phone, address,
    /// roles, offline_access) are no API scopes; a token with any other scope, or without this one, is judged by its audiences, so
    /// restrict the audiences the host accepts (AddAudiences, as ABP's templates do). Turn it off only when the scope itself is meant
    /// to open an API of the application, which then is one of <see cref="ScopeResources"/>: an MCP client can call that API as the
    /// user, without the MCP endpoint's rules. Default true.
    /// </summary>
    public bool ConfineScopeToMcpEndpoint { get; set; } = true;

    /// <summary><see cref="ConfineScopeToMcpEndpoint"/> is on and the principal's only API scope is <see cref="ScopeName"/>.</summary>
    internal bool IsConfinedByScope(ClaimsPrincipal principal)
    {
        var scopes = principal.GetScopes();
        return ConfineScopeToMcpEndpoint
               && scopes.Contains(ScopeName)
               && scopes.All(s => s == ScopeName || StandardScopes.Contains(s));
    }

    /// <summary><see cref="ScopeResources"/>, or [<see cref="McpResource"/>] when empty, or nothing when both are unset.</summary>
    public IReadOnlyList<string> GetScopeResources() =>
        ScopeResources.Count > 0 ? ScopeResources : McpResource != null ? [McpResource] : [];
}
