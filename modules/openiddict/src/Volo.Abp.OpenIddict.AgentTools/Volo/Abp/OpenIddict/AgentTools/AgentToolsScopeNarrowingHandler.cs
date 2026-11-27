using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// Keeps the MCP scope in every token of a grant whose only API scope it is: a token request with a scope parameter that leaves the
/// scope out is refused (invalid_scope). <see cref="AgentToolsMcpTokenAudienceHandler"/> tells such a token by its scopes. Nothing
/// happens while <see cref="AbpAgentToolsOpenIddictOptions.ConfineScopeToMcpEndpoint"/> is off, nor to any other grant.
/// </summary>
public class AgentToolsScopeNarrowingHandler : IOpenIddictServerHandler<ValidateTokenRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; } =
        OpenIddictServerHandlerDescriptor.CreateBuilder<ValidateTokenRequestContext>()
            .UseSingletonHandler<AgentToolsScopeNarrowingHandler>()
            .SetOrder(OpenIddictServerHandlers.Exchange.ValidateGrantedScopes.Descriptor.Order + 1_000)
            .Build();

    private readonly IOptions<AbpAgentToolsOpenIddictOptions> _options;

    public AgentToolsScopeNarrowingHandler(IOptions<AbpAgentToolsOpenIddictOptions> options)
    {
        _options = options;
    }

    public ValueTask HandleAsync(ValidateTokenRequestContext context)
    {
        var grant = context.AuthorizationCodePrincipal ?? context.DeviceCodePrincipal ?? context.RefreshTokenPrincipal;
        if (grant != null
            && !string.IsNullOrEmpty(context.Request.Scope)
            && !context.Request.HasScope(_options.Value.ScopeName)
            && _options.Value.IsConfinedByScope(grant))
        {
            context.Reject(OpenIddictConstants.Errors.InvalidScope, $"The '{_options.Value.ScopeName}' scope cannot be left out.");
        }

        return default;
    }
}
