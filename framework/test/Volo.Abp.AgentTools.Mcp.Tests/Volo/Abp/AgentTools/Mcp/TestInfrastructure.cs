using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AgentTools.Mcp;

/// <summary>
/// Test-only bearer scheme; no real tokens. "Authorization: Bearer test" plus the headers below describe the token:
/// user id, tenant id, scope (one space-separated claim, as a JWT has it) and OAuth client id. "Bearer invalid" fails.
/// </summary>
public class TestBearerAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public new const string Scheme = "TestBearer";

    public const string UserHeader = "X-Test-User";
    public const string TenantHeader = "X-Test-Tenant";
    public const string ScopeHeader = "X-Test-Scope";
    public const string ClientHeader = "X-Test-Client";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorization = Request.Headers.Authorization.ToString();
        if (!authorization.StartsWith("Bearer ", StringComparison.Ordinal))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        if (authorization != "Bearer test")
        {
            return Task.FromResult(AuthenticateResult.Fail("invalid token"));
        }

        var claims = new List<Claim>();
        Add(claims, UserHeader, AbpClaimTypes.UserId);
        Add(claims, TenantHeader, AbpClaimTypes.TenantId);
        Add(claims, ScopeHeader, "scope");
        Add(claims, ClientHeader, AbpClaimTypes.ClientId);
        if (claims.Any(c => c.Type == AbpClaimTypes.UserId))
        {
            claims.Add(new Claim(AbpClaimTypes.UserName, "token-user"));
        }

        var ticket = new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme)), Scheme);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private void Add(List<Claim> claims, string header, string claimType)
    {
        var value = Request.Headers[header].ToString();
        if (value.Length > 0)
        {
            claims.Add(new Claim(claimType, value));
        }
    }
}

/// <summary>Permission grants per user id (one host per test); nothing is granted by default.</summary>
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IPermissionStore), typeof(TestPermissionStore))]
public class TestPermissionStore : IPermissionStore, ISingletonDependency
{
    private readonly ConcurrentDictionary<(string Name, string ProviderKey), bool> _grants = new();

    public void GrantToUser(Guid userId, params string[] permissionNames)
    {
        foreach (var name in permissionNames)
        {
            _grants[(name, userId.ToString())] = true;
        }
    }

    public Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
    {
        return Task.FromResult(providerName == UserPermissionValueProvider.ProviderName && _grants.ContainsKey((name, providerKey)));
    }

    public async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
    {
        var result = new MultiplePermissionGrantResult();
        foreach (var name in names.Distinct())
        {
            result.Result[name] = await IsGrantedAsync(name, providerName, providerKey)
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined;
        }

        return result;
    }
}

/// <summary>Tenant feature values (provider "T"); unset features use their default value.</summary>
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IFeatureStore), typeof(TestFeatureStore))]
public class TestFeatureStore : IFeatureStore, ISingletonDependency
{
    private readonly ConcurrentDictionary<(string Name, string ProviderKey), string> _values = new();

    public void SetForTenant(Guid tenantId, string name, string value) => _values[(name, tenantId.ToString())] = value;

    public Task<string?> GetOrNullAsync(string name, string? providerName, string? providerKey)
    {
        return Task.FromResult(providerName == TenantFeatureValueProvider.ProviderName && providerKey != null
            ? _values.TryGetValue((name, providerKey), out var value) ? value : null
            : null);
    }
}

/// <summary>The audit logs the host saved.</summary>
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IAuditingStore), typeof(TestAuditingStore))]
public class TestAuditingStore : IAuditingStore, ISingletonDependency
{
    public ConcurrentQueue<AuditLogInfo> Logs { get; } = new();

    public Task SaveAsync(AuditLogInfo auditInfo)
    {
        Logs.Enqueue(auditInfo);
        return Task.CompletedTask;
    }
}

[ExposeServices(typeof(IAgentToolCallObserver), typeof(RecordingObserver))]
public class RecordingObserver : IAgentToolCallObserver, ISingletonDependency
{
    public ConcurrentQueue<AgentToolCallRecord> Records { get; } = new();

    public Task OnCallAsync(AgentToolCallRecord record)
    {
        Records.Enqueue(record);
        return Task.CompletedTask;
    }
}
