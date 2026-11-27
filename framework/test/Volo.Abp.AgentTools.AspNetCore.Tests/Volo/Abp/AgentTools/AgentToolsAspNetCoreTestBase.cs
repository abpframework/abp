using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Http.Modeling;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AgentTools;

/// <summary>
/// Base class for tests that need the web test host (a new host per test). Use Services / GetRequiredService / Client.
/// Host options: override ConfigureServices (e.g. services.Configure&lt;AbpAgentToolOptions&gt;(...)).
/// Catalogue with other options and no new host: <see cref="CreateCatalog"/>.
/// </summary>
public abstract class AgentToolsAspNetCoreTestBase : AbpWebApplicationFactoryIntegratedTest<Program>
{
    protected TestPermissionStore PermissionStore => GetRequiredService<TestPermissionStore>();

    /// <summary>Acts as the given user until disposed (claims: user id, user name, tenant id when given).</summary>
    protected IDisposable LoginAs(Guid userId, Guid? tenantId = null, string userName = "test-user")
    {
        var claims = new List<Claim>
        {
            new(AbpClaimTypes.UserId, userId.ToString()),
            new(AbpClaimTypes.UserName, userName)
        };
        if (tenantId != null)
        {
            claims.Add(new Claim(AbpClaimTypes.TenantId, tenantId.Value.ToString()));
        }

        return GetRequiredService<ICurrentPrincipalAccessor>().Change(new ClaimsPrincipal(new ClaimsIdentity(claims, "Test")));
    }

    /// <summary>Switches ICurrentTenant (data filter, policy scope) until disposed; independent of <see cref="LoginAs"/>.</summary>
    protected IDisposable ChangeTenant(Guid? tenantId)
    {
        return GetRequiredService<ICurrentTenant>().Change(tenantId);
    }

    /// <summary>A catalogue over this host's API with explicit options (the registered one uses the host's options).</summary>
    protected AgentToolCatalog CreateCatalog(Action<AbpAgentToolOptions> configure, ILogger<AgentToolCatalog>? logger = null)
    {
        var options = new AbpAgentToolOptions();
        configure(options);
        return new AgentToolCatalog(
            Microsoft.Extensions.Options.Options.Create(options),
            GetRequiredService<IApiDescriptionModelProvider>(),
            GetRequiredService<IApiDescriptionGroupCollectionProvider>(),
            GetRequiredService<IOptions<AspNetCoreApiDescriptionModelProviderOptions>>(),
            GetRequiredService<IOptions<AbpApiDescriptionModelOptions>>(),
            GetRequiredService<IServiceScopeFactory>(),
            logger ?? NullLogger<AgentToolCatalog>.Instance);
    }
}
