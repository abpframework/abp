using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp.AgentTools.Mcp;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.Uow;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// A real ABP OpenIddict host: server (ephemeral keys, HTTP allowed) and validation configured the way the app templates do it
/// (<see cref="AppAudience"/> unless <see cref="AcceptAnyAudienceSetting"/> is set, local server, ASP.NET Core), the OpenIddict and Identity stores on in-memory Sqlite, ABP's token endpoint, and
/// <c>/test/bearer</c> and the other <see cref="BearerProbePaths"/>, which answer 200 only for a request the OpenIddict validation
/// scheme authenticates, the real MCP endpoint (AbpAgentToolsMcpModule, default options) at <c>/mcp</c>, and forwarded headers that
/// trust any X-Forwarded-Prefix.
/// </summary>
[DependsOn(
    typeof(AbpOpenIddictAgentToolsModule),
    typeof(AbpAgentToolsMcpModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqliteModule),
    typeof(AbpAutofacModule)
)]
public class AgentToolsOpenIddictTestModule : AbpModule
{
    public const string AppAudience = "TestApp";

    /// <summary>A setting of the host: an app that calls no AddAudiences.</summary>
    public const string AcceptAnyAudienceSetting = "AcceptAnyAudience";

    /// <summary>Like <see cref="BearerProbePaths"/>, with the authentication before UseRouting.</summary>
    public const string BeforeRoutingPath = "/test/before-routing";

    /// <summary>Like <see cref="BearerProbePaths"/>, without an endpoint.</summary>
    public const string WithoutEndpointPath = "/test/without-endpoint";

    /// <summary>App endpoints that answer 200 only for a request the OpenIddict validation scheme authenticates.</summary>
    public static readonly string[] BearerProbePaths = ["/test/bearer", "/mcpx", "/mcp/other"];

    private AbpUnitTestSqliteDatabase? _database;

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpOpenIddictAspNetCoreOptions>(options => options.AddDevelopmentEncryptionAndSigningCertificate = false);

        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder.AddEphemeralEncryptionKey();
            builder.AddEphemeralSigningKey();
            builder.UseAspNetCore().DisableTransportSecurityRequirement();
            // A fixed issuer, as production hosts set it: otherwise it follows the request, and a forwarded prefix changes it.
            builder.SetIssuer(new Uri("http://localhost/"));
        });

        var acceptAnyAudience = context.Services.GetConfiguration()[AcceptAnyAudienceSetting] != null;
        PreConfigure<OpenIddictBuilder>(builder => builder.AddValidation(options =>
        {
            if (!acceptAnyAudience)
            {
                options.AddAudiences(AppAudience);
            }

            options.UseLocalServer();
            options.UseAspNetCore();
        }));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();
        Configure<AbpBackgroundWorkerOptions>(options => options.IsEnabled = false);

        // ABP's token endpoint (TokenController) lives in the OpenIddict module's assembly.
        context.Services.GetSingletonInstance<ApplicationPartManager>()
            .ApplicationParts.AddIfNotContains(typeof(AbpOpenIddictAspNetCoreModule).Assembly);

        _database = new AbpUnitTestSqliteDatabase();
        _database.CreateTables(
            new OpenIddictDbContext(new DbContextOptionsBuilder<OpenIddictDbContext>().UseSqlite(_database.ConnectionString).Options),
            new IdentityDbContext(new DbContextOptionsBuilder<IdentityDbContext>().UseSqlite(_database.ConnectionString).Options));
        Configure<AbpDbConnectionOptions>(options => options.ConnectionStrings.Default = _database.ConnectionString);
        Configure<AbpDbContextOptions>(options => options.Configure(c => c.UseSqlite()));

        // App endpoints next to the MCP endpoint, under its path and sharing its prefix.
        Configure<AbpEndpointRouterOptions>(options => options.EndpointConfigureActions.Add(endpointContext =>
        {
            foreach (var path in BearerProbePaths)
            {
                endpointContext.Endpoints.MapGet(path, ProbeAsync);
            }

            // The login page: a cookie for the authorization endpoint.
            endpointContext.Endpoints.MapPost("/test/login", async (SignInManager<IdentityUser> signInManager, string userName) =>
                await signInManager.SignInAsync((await signInManager.UserManager.FindByNameAsync(userName))!, isPersistent: false));
        }));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        // A pass-through proxy setup that trusts X-Forwarded-Prefix from anyone: the client chooses PathBase.
        var forwarded = new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedPrefix };
        forwarded.KnownIPNetworks.Clear();
        forwarded.KnownProxies.Clear();
        app.UseForwardedHeaders(forwarded);
        app.UseWhen(http => http.Request.Path == BeforeRoutingPath, branch =>
        {
            branch.UseAuthentication();
            branch.Run(ProbeAsync);
        });
        app.UseRouting();
        app.UseAuthentication();
        app.UseUnitOfWork();
        app.UseAuthorization();
        app.UseConfiguredEndpoints();
        app.Use((http, next) => http.Request.Path == WithoutEndpointPath ? ProbeAsync(http) : next(http));
    }

    private static async Task ProbeAsync(HttpContext http)
    {
        var result = await http.AuthenticateAsync(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        http.Response.StatusCode = result.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status401Unauthorized;
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _database?.Dispose();
    }
}
