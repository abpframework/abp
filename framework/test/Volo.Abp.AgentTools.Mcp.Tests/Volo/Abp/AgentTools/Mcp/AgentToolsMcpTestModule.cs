using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AgentTools.Mcp.Sample;
using Volo.Abp.AgentTools.SchemaSample;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Volo.Abp.AgentTools.Mcp;

/// <summary>
/// MCP test host. The app's default scheme is a cookie scheme (so a cookie-only request is a real test); the endpoint uses the
/// header-driven <see cref="TestBearerAuthenticationHandler"/>. Two tenants and an inactive one in the default tenant store,
/// multi-tenancy middleware with ABP's header/query/route/cookie resolvers, auditing on, Sqlite in-memory, an in-memory policy
/// store.
/// </summary>
[DependsOn(
    typeof(AbpAgentToolsMcpModule),
    typeof(AbpAgentToolsTestModule),
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAutofacModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
)]
public class AgentToolsMcpTestModule : AbpModule
{
    public static readonly Guid TenantA = Guid.Parse("0a0a0a0a-0000-0000-0000-00000000000a");
    public static readonly Guid TenantB = Guid.Parse("0b0b0b0b-0000-0000-0000-00000000000b");
    public static readonly Guid InactiveTenant = Guid.Parse("0c0c0c0c-0000-0000-0000-00000000000c");

    private AbpUnitTestSqliteDatabase? _database;

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(AgentToolsMcpTestModule).Assembly, o =>
            {
                o.RemoteServiceName = McpSampleAppService.RemoteServiceName;
                o.TypePredicate = t => t == typeof(McpSampleAppService);
            });
            // Not included as tools here; McpSchemaTests includes it.
            options.ConventionalControllers.Create(typeof(SchemaSampleAppService).Assembly, o =>
            {
                o.RemoteServiceName = SchemaSampleAppService.RemoteServiceName;
                o.RootPath = SchemaSampleAppService.RootPath;
                o.TypePredicate = t => t == typeof(SchemaSampleAppService);
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie()
            .AddScheme<AuthenticationSchemeOptions, TestBearerAuthenticationHandler>(TestBearerAuthenticationHandler.Scheme, null);

        Configure<AbpAgentToolsMcpOptions>(options => options.AuthenticationScheme = TestBearerAuthenticationHandler.Scheme);
        Configure<AbpAgentToolOptions>(options => options.IncludedRemoteServiceNames.Add(McpSampleAppService.RemoteServiceName));

        Configure<AbpMultiTenancyOptions>(options => options.IsEnabled = true);
        Configure<AbpDefaultTenantStoreOptions>(options => options.Tenants =
        [
            new TenantConfiguration(TenantA, "tenant-a", "TENANT-A"),
            new TenantConfiguration(TenantB, "tenant-b", "TENANT-B"),
            new TenantConfiguration(InactiveTenant, "tenant-off", "TENANT-OFF") { IsActive = false }
        ]);

        context.Services.AddSingleton<InMemoryAgentToolPolicyStore>();
        context.Services.Replace(ServiceDescriptor.Singleton<IAgentToolPolicyStore>(sp => sp.GetRequiredService<InMemoryAgentToolPolicyStore>()));

        context.Services.AddAbpDbContext<McpSampleDbContext>(options => options.AddDefaultRepositories(includeAllEntities: true));
        _database = new AbpUnitTestSqliteDatabase();
        _database.CreateTables(new McpSampleDbContext(
            new DbContextOptionsBuilder<McpSampleDbContext>()
                .UseSqlite(_database.ConnectionString)
                .AddAbpDbContextOptionsExtension()
                .Options));
        Configure<AbpDbConnectionOptions>(o => o.ConnectionStrings.Default = _database.ConnectionString);
        Configure<AbpDbContextOptions>(o => o.Configure(c => c.UseSqlite()));

        // A browser-style session: /test/cookie-login signs in with the cookie scheme, /test/whoami shows ABP's current user.
        Configure<AbpEndpointRouterOptions>(options => options.EndpointConfigureActions.Add(endpointContext =>
        {
            endpointContext.Endpoints.MapGet("/test/cookie-login", async (HttpContext http, Guid userId, Guid? tenantId) =>
            {
                var claims = new List<Claim> { new(AbpClaimTypes.UserId, userId.ToString()), new(AbpClaimTypes.UserName, "cookie-user") };
                if (tenantId != null)
                {
                    claims.Add(new Claim(AbpClaimTypes.TenantId, tenantId.Value.ToString()));
                }

                await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
            });
            endpointContext.Endpoints.MapGet("/test/whoami", (ICurrentUser user, ICurrentTenant tenant) =>
                Results.Json(new { userId = user.Id, tenantId = tenant.Id }));
        }));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        // English unless the request asks for Turkish (Accept-Language), whatever the machine's culture.
        app.UseRequestLocalization(o => o.SetDefaultCulture("en").AddSupportedCultures("en", "tr").AddSupportedUICultures("en", "tr"));
        app.UseRouting();
        app.UseAuthentication();
        app.UseMultiTenancy();
        app.UseUnitOfWork();
        app.UseAuditing();
        app.UseAuthorization();
        app.UseConfiguredEndpoints();
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _database?.Dispose();
    }
}
