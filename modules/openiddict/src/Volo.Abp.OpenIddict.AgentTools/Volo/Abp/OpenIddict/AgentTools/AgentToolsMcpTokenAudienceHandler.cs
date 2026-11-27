using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Validation;
using static OpenIddict.Validation.OpenIddictValidationEvents;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// Keeps a token of an MCP client at the MCP endpoint: on a request whose routed endpoint is not an MCP endpoint (or that has no
/// endpoint) OpenIddict validation refuses it (invalid_token), so an MCP client cannot use the app's other APIs with it as the user,
/// e.g. the application services behind tools the endpoint hides. The routed endpoint decides, not the path, which a forwarded prefix
/// (X-Forwarded-Prefix) can change; so authentication must run after UseRouting (as in ABP's templates), otherwise such a token is
/// refused everywhere. Such a token is
/// <list type="bullet">
/// <item>one whose only API scope is <see cref="AbpAgentToolsOpenIddictOptions.ScopeName"/>, whatever its audiences (unless
/// <see cref="AbpAgentToolsOpenIddictOptions.ConfineScopeToMcpEndpoint"/> is off);</item>
/// <item>one that this host accepts only for its audience <see cref="AbpAgentToolsOpenIddictOptions.McpResource"/>, whatever its
/// scopes: it has that audience and no other one that this host accepts (no other one at all when the host does not restrict
/// audiences). Not while the MCP URL is unknown.</item>
/// </list>
/// Any other token, for example one that also carries the scope of an API whose audience this host accepts, is not affected: this
/// covers tokens that carry the scope or have the MCP URL as their only accepted audience. So restrict the audiences the host
/// accepts too (AddAudiences, as ABP's templates do).
/// </summary>
public class AgentToolsMcpTokenAudienceHandler : IOpenIddictValidationHandler<ProcessAuthenticationContext>
{
    /// <summary>The endpoint metadata Volo.Abp.AgentTools.Mcp puts on its endpoints (AbpAgentToolsMcpOptions.EndpointMetadata, same value).</summary>
    public const string McpEndpointMetadata = "Volo.Abp.AgentTools.Mcp.Endpoint";

    public static OpenIddictValidationHandlerDescriptor Descriptor { get; } =
        OpenIddictValidationHandlerDescriptor.CreateBuilder<ProcessAuthenticationContext>()
            .UseSingletonHandler<AgentToolsMcpTokenAudienceHandler>()
            .SetOrder(OpenIddictValidationHandlers.ValidateAccessToken.Descriptor.Order + 1_000)
            .Build();

    private readonly IOptions<AbpAgentToolsOpenIddictOptions> _options;

    public AgentToolsMcpTokenAudienceHandler(IOptions<AbpAgentToolsOpenIddictOptions> options)
    {
        _options = options;
    }

    public ValueTask HandleAsync(ProcessAuthenticationContext context)
    {
        var request = context.Transaction.GetHttpRequest();
        var principal = context.AccessTokenPrincipal;
        if (request != null && principal != null
            && (_options.Value.IsConfinedByScope(principal) || IsConfinedByAudience(principal, context.Options.Audiences))
            && request.HttpContext.GetEndpoint()?.Metadata.Contains(McpEndpointMetadata) != true)
        {
            context.Reject(OpenIddictConstants.Errors.InvalidToken, "This token is only valid at the MCP endpoint.");
        }

        return default;
    }

    private bool IsConfinedByAudience(ClaimsPrincipal principal, HashSet<string> accepted)
    {
        var resource = _options.Value.McpResource;
        var audiences = principal.GetAudiences();
        // As OpenIddict validates audiences: an empty list accepts any.
        return resource != null
               && audiences.Contains(resource)
               && !audiences.Any(a => a != resource && (accepted.Count == 0 || accepted.Contains(a)));
    }
}
