using System;
using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AgentTools.Mcp;

public class AbpAgentToolsMcpOptions
{
    /// <summary>RFC 9728 well-known prefix; the path-specific metadata of the endpoint is this plus <see cref="Path"/>.</summary>
    public const string ResourceMetadataPathPrefix = "/.well-known/oauth-protected-resource";

    /// <summary>
    /// Endpoint metadata (this string itself) on every endpoint of the MCP route. Volo.Abp.OpenIddict.AgentTools accepts an
    /// MCP-only token only on an endpoint carrying it (AgentToolsMcpTokenAudienceHandler.McpEndpointMetadata, the same value).
    /// </summary>
    public const string EndpointMetadata = "Volo.Abp.AgentTools.Mcp.Endpoint";

    /// <summary>Path of the stateless Streamable HTTP endpoint. Default "/mcp".</summary>
    public string Path { get; set; } = "/mcp";

    /// <summary>
    /// The token must carry this scope ("scope" claims, space-separated values allowed), otherwise 403. Grant it only to MCP
    /// clients: ABP builds the audience from scopes, so this scope is what keeps a normal API token out. Default "AgentTools".
    /// </summary>
    public string RequiredScope { get; set; } = "AgentTools";

    /// <summary>
    /// The only scheme that authenticates the endpoint, whatever the app's default scheme is (cookies never do).
    /// Default: the OpenIddict validation scheme. Apps validating JWTs with JwtBearer set a JwtBearer scheme of its own whose
    /// audience is the MCP URL (a second AddJwtBearer call), not the app's API scheme. It must be a bearer scheme, or a
    /// browser session could authenticate the endpoint: the app does not start when the scheme does not exist, is a cookie
    /// scheme or a policy scheme.
    /// </summary>
    public string AuthenticationScheme { get; set; } = "OpenIddict.Validation.AspNetCore";

    /// <summary>MCP serverInfo name; null keeps the MCP SDK default (the entry assembly's name and version).</summary>
    public string? ServerName { get; set; }

    /// <summary>MCP serverInfo version, used when <see cref="ServerName"/> is set. Null = "1.0.0".</summary>
    public string? ServerVersion { get; set; }

    /// <summary>
    /// The authorization server (issuer) named in the protected resource metadata. Null = the app's own issuer, taken from the
    /// request: scheme, host and path base plus "/" (OpenIddict's issuer form). Behind a proxy that needs forwarded headers.
    /// </summary>
    public Uri? AuthorizationServer { get; set; }

    /// <summary>Which catalogue tools MCP may offer (on top of policy and permissions). Null = all of them.</summary>
    public Func<AgentToolDefinition, bool>? ToolFilter { get; set; }

    /// <summary>"/.well-known/oauth-protected-resource" + <see cref="Path"/> (RFC 9728 path-specific form).</summary>
    public string ResourceMetadataPath => ResourceMetadataPathPrefix + Path;

    /// <summary>The endpoint's absolute URL (the RFC 9728 / RFC 8707 resource identifier).</summary>
    public string GetResourceUrl(HttpRequest request) => GetBaseUrl(request) + Path;

    /// <summary>Absolute URL of the path-specific metadata, as named in the 401's <c>resource_metadata</c>.</summary>
    public string GetResourceMetadataUrl(HttpRequest request) => GetBaseUrl(request) + ResourceMetadataPath;

    public Uri GetAuthorizationServer(HttpRequest request) => AuthorizationServer ?? new Uri(GetBaseUrl(request) + "/");

    public bool IsExposed(AgentToolDefinition definition) => ToolFilter?.Invoke(definition) ?? true;

    private static string GetBaseUrl(HttpRequest request) => $"{request.Scheme}://{request.Host}{request.PathBase}";
}
