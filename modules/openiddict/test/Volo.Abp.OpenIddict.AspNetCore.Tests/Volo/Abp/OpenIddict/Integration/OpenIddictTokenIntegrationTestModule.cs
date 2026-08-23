using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.AspNetCore.Uow;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Uow;
using Volo.Abp.Autofac;

namespace Volo.Abp.OpenIddict.Integration;

public class TokenVisibilityRecorder
{
    public long? TokenCountAtResponseStart { get; set; }
}

[DependsOn(
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(AbpOpenIddictAspNetCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqliteModule),
    typeof(AbpAutofacModule)
    )]
public class OpenIddictTokenIntegrationTestModule : AbpModule
{
    // File-based SQLite (not shared-cache in-memory) so an independent connection can read committed
    // state while another holds an open write transaction, without the shared-cache single-writer deadlock.
    private readonly string _databasePath = Path.Combine(Path.GetTempPath(), $"abp-oidc-uow-{Guid.NewGuid():N}.db");
    private string ConnectionString => $"Data Source={_databasePath};Pooling=False";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
        {
            options.AddDevelopmentEncryptionAndSigningCertificate = false;
        });

        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder.AddEphemeralEncryptionKey();
            builder.AddEphemeralSigningKey();
            builder.UseAspNetCore().DisableTransportSecurityRequirement();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<TokenVisibilityRecorder>();

        // A remapped token endpoint, so the tests can prove the opt-in list is derived from the configured
        // server endpoints (custom endpoints are followed) rather than a hardcoded "/connect" prefix.
        Configure<OpenIddictServerOptions>(options =>
        {
            options.TokenEndpointUris.Add(new Uri("my-custom/token", UriKind.Relative));
        });

        // The OpenIddict controllers (including the token endpoint) live in a referenced assembly.
        context.Services.GetSingletonInstance<ApplicationPartManager>()
            .ApplicationParts.AddIfNotContains(typeof(AbpOpenIddictAspNetCoreModule).Assembly);

        using (var dbContext = new OpenIddictDbContext(
                   new DbContextOptionsBuilder<OpenIddictDbContext>().UseSqlite(ConnectionString).Options))
        {
            dbContext.Database.EnsureCreated();
        }

        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = ConnectionString;
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(c => c.UseSqlite());
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedClientAsync(context.ServiceProvider).GetAwaiter().GetResult();

        var app = context.GetApplicationBuilder();
        app.UseRouting();

        // Registered before UseUnitOfWork so its OnStarting runs after the unit of work commits
        // (callbacks run in reverse order): reads the token count from an independent connection.
        app.Use(async (ctx, next) =>
        {
            if (ctx.Request.Path.StartsWithSegments("/connect/token"))
            {
                ctx.Response.OnStarting(async () =>
                {
                    var recorder = ctx.RequestServices.GetRequiredService<TokenVisibilityRecorder>();
                    var uowManager = ctx.RequestServices.GetRequiredService<IUnitOfWorkManager>();
                    using var uow = uowManager.Begin(requiresNew: true, isTransactional: false);
                    var repository = ctx.RequestServices.GetRequiredService<IRepository<OpenIddictToken, Guid>>();
                    recorder.TokenCountAtResponseStart = await repository.GetCountAsync();
                    await uow.CompleteAsync();
                });
            }

            await next();
        });

        app.UseAuthentication();
        app.UseUnitOfWork();
        app.UseAuthorization();
        app.UseConfiguredEndpoints();
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        if (File.Exists(_databasePath))
        {
            File.Delete(_databasePath);
        }
    }

    private static async Task SeedClientAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowManager.Begin();

        var applicationManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        if (await applicationManager.FindByClientIdAsync("test-client") == null)
        {
            await applicationManager.CreateAsync(new AbpApplicationDescriptor
            {
                ClientId = "test-client",
                ClientSecret = "test-secret",
                DisplayName = "Test Client",
                ClientType = OpenIddictConstants.ClientTypes.Confidential,
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials
                }
            });
        }

        await uow.CompleteAsync();
    }
}
