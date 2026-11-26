using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.OpenIddict.WildcardDomains;

public class AbpOpenIddictWildcardDomainValidator : ITransientDependency
{
    protected AbpOpenIddictWildcardDomainOptions Options { get; }
    protected IOpenIddictApplicationManager ApplicationManager { get; }
    protected ITenantConfigurationProvider TenantConfigurationProvider { get; }
    protected ITenantStore TenantStore { get; }

    public AbpOpenIddictWildcardDomainValidator(
        IOptions<AbpOpenIddictWildcardDomainOptions> options,
        IOpenIddictApplicationManager applicationManager,
        ITenantConfigurationProvider tenantConfigurationProvider,
        ITenantStore tenantStore)
    {
        Options = options.Value;
        ApplicationManager = applicationManager;
        TenantConfigurationProvider = tenantConfigurationProvider;
        TenantStore = tenantStore;
    }

    public virtual async Task<bool> IsValidAsync(string url, string clientId, bool postLogout = false)
    {
        return await IsValidAsync(url, clientId, postLogout, null, false);
    }

    public virtual async Task<bool> IsValidForPrincipalAsync(string url, string clientId, ClaimsPrincipal principal)
    {
        var application = await ApplicationManager.FindByClientIdAsync(clientId);
        if (application != null && await ApplicationManager.ValidateRedirectUriAsync(application, url))
        {
            return true;
        }

        var tenantId = principal.FindTenantId();
        var tenant = tenantId.HasValue ? await TenantStore.FindAsync(tenantId.Value) : null;
        return await IsValidAsync(url, clientId, false, tenant, true);
    }

    protected virtual async Task<bool> IsValidAsync(string url, string clientId, bool postLogout,
        TenantConfiguration principalTenant, bool usePrincipalTenant)
    {
        if (string.IsNullOrEmpty(clientId) ||
            !Uri.TryCreate(url, UriKind.Absolute, out var target) ||
            !string.IsNullOrEmpty(target.UserInfo) || !string.IsNullOrEmpty(target.Fragment))
        {
            return false;
        }

        var application = await ApplicationManager.FindByClientIdAsync(clientId);
        if (application == null)
        {
            return false;
        }

        var registeredUris = postLogout
            ? await ApplicationManager.GetPostLogoutRedirectUrisAsync(application)
            : await ApplicationManager.GetRedirectUrisAsync(application);

        foreach (var format in Options.WildcardDomainsFormat)
        {
            var tenantFormat = format.Contains("{0}");
            var wildcardFormat = format.Replace("{0}", "*");
            if (!UrlHelpers.IsSubdomainOf(url, wildcardFormat) ||
                !Uri.TryCreate(wildcardFormat.Replace("*.", ""), UriKind.Absolute, out var parent))
            {
                continue;
            }

            if (tenantFormat)
            {
                // OpenIddict validates requests before the multi-tenancy middleware has set CurrentTenant.
                TenantConfiguration tenant;
                try
                {
                    tenant = usePrincipalTenant ? principalTenant : await TenantConfigurationProvider.GetAsync();
                }
                catch (BusinessException)
                {
                    return false;
                }

                // Compare the host directly; a tenant name with URI delimiters must not turn into another host.
                if (tenant == null || !tenant.IsActive || string.IsNullOrEmpty(tenant.Name) ||
                    !string.Equals(target.Host, tenant.Name + "." + parent.Host, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
            }

            if (parent.PathAndQuery != "/" && target.PathAndQuery != parent.PathAndQuery)
            {
                continue;
            }

            foreach (var registered in registeredUris)
            {
                if (!Uri.TryCreate(registered, UriKind.Absolute, out var callback) ||
                    callback.Scheme != parent.Scheme || callback.Port != parent.Port ||
                    !string.Equals(callback.Host, parent.Host, StringComparison.OrdinalIgnoreCase) ||
                    !string.IsNullOrEmpty(callback.UserInfo) || !string.IsNullOrEmpty(callback.Fragment))
                {
                    continue;
                }

                var expanded = new UriBuilder(callback) { Host = target.Host }.Uri;
                if (string.Equals(expanded.AbsoluteUri, target.AbsoluteUri, StringComparison.Ordinal))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public virtual async Task<bool> IsValidPostLogoutAsync(string url, ClaimsPrincipal principal, bool ignoreEndpointPermissions)
    {
        var clients = new HashSet<string>(StringComparer.Ordinal);
        if (principal != null)
        {
            clients.UnionWith(principal.GetAudiences());
            clients.UnionWith(principal.GetPresenters());
        }
        else if (Uri.TryCreate(url, UriKind.Absolute, out var target))
        {
            foreach (var format in Options.WildcardDomainsFormat)
            {
                var wildcard = format.Replace("{0}", "*");
                if (!UrlHelpers.IsSubdomainOf(url, wildcard) ||
                    !Uri.TryCreate(wildcard.Replace("*.", ""), UriKind.Absolute, out var parent))
                {
                    continue;
                }

                var callback = new UriBuilder(target) { Host = parent.Host }.Uri;
                // Registrations are looked up by their original string, so a root callback may be stored without the trailing slash.
                var candidates = callback.AbsolutePath == "/" && string.IsNullOrEmpty(callback.Query)
                    ? new[] { callback.AbsoluteUri, callback.AbsoluteUri.TrimEnd('/') }
                    : new[] { callback.AbsoluteUri };
                foreach (var candidate in candidates)
                {
                    await foreach (var application in ApplicationManager.FindByPostLogoutRedirectUriAsync(candidate))
                    {
                        var clientId = await ApplicationManager.GetClientIdAsync(application);
                        if (clientId != null)
                        {
                            clients.Add(clientId);
                        }
                    }
                }
            }
        }

        foreach (var clientId in clients)
        {
            var application = await ApplicationManager.FindByClientIdAsync(clientId);
            if (application == null || (!ignoreEndpointPermissions &&
                !await ApplicationManager.HasPermissionAsync(application, OpenIddictConstants.Permissions.Endpoints.EndSession) &&
                !await ApplicationManager.HasPermissionAsync(application, "ept:logout")))
            {
                continue;
            }

            if (await IsValidAsync(url, clientId, postLogout: true))
            {
                return true;
            }
        }

        return false;
    }
}
