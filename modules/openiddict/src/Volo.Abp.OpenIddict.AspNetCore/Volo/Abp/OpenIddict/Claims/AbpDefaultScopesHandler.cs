using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.OpenIddict;

public class AbpDefaultScopesHandler : IAbpOpenIddictClaimsPrincipalHandler, ITransientDependency
{
    public ILogger<AbpDefaultScopesHandler> Logger { get; set; }
        = NullLogger<AbpDefaultScopesHandler>.Instance;

    public virtual async Task HandleAsync(AbpOpenIddictClaimsPrincipalHandlerContext context)
    {
        var options = context.ScopeServiceProvider
            .GetRequiredService<IOptions<AbpOpenIddictAspNetCoreOptions>>().Value;

        var request = context.OpenIddictRequest;
        if (!IsDefaultScopesEnabled(request, options))
        {
            return;
        }

        if (!context.Principal.GetScopes().IsDefaultOrEmpty)
        {
            return;
        }

        var clientId = request.ClientId;
        if (string.IsNullOrEmpty(clientId))
        {
            return;
        }

        var applicationManager = context.ScopeServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var scopeManager = context.ScopeServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        var application = await applicationManager.FindByClientIdAsync(clientId);
        if (application == null)
        {
            return;
        }

        var permissions = await applicationManager.GetPermissionsAsync(application);
        var prefix = OpenIddictConstants.Permissions.Prefixes.Scope;

        var scopes = permissions
            .Where(p => p.StartsWith(prefix, StringComparison.Ordinal))
            .Select(p => p[prefix.Length..])
            .ToImmutableArray();

        if (scopes.IsDefaultOrEmpty)
        {
            return;
        }

        Logger.LogDebug(
            "Injecting default scopes for client {ClientId} (grant_type {GrantType}): {Scopes}",
            clientId,
            request.GrantType,
            string.Join(", ", scopes));

        context.Principal.SetScopes(scopes);
        context.Principal.SetResources(await scopeManager.ListResourcesAsync(scopes).ToListAsync());
    }

    protected virtual bool IsDefaultScopesEnabled(OpenIddictRequest request, AbpOpenIddictAspNetCoreOptions options)
    {
        if (request.IsClientCredentialsGrantType())
        {
            return options.UseDefaultScopesForClientCredentials;
        }

        if (request.IsPasswordGrantType())
        {
            return options.UseDefaultScopesForPassword;
        }

        if (request.IsTokenExchangeGrantType())
        {
            return options.UseDefaultScopesForTokenExchange;
        }

        return false;
    }
}
