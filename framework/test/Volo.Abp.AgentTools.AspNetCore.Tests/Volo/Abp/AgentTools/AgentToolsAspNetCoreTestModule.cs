using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AgentTools.Sample;
using Volo.Abp.AgentTools.Sample.Binding;
using Volo.Abp.AgentTools.Sample.Diagnostics;
using Volo.Abp.AgentTools.Sample.Governance;
using Volo.Abp.AgentTools.SchemaSample;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AgentTools;

[DependsOn(
    typeof(AbpAgentToolsAspNetCoreModule),
    typeof(AbpAgentToolsTestModule),
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
)]
public class AgentToolsAspNetCoreTestModule : AbpModule
{
    // In-memory Sqlite kept open for the host's lifetime (disposed in OnApplicationShutdown).
    private AbpUnitTestSqliteDatabase? _database;

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // Sample services under Sample/ become conventional controllers: "AgentToolsSample" (root path "app"),
        // "OtherSample" (root path "other") and "DuplicateSample" (root path "duplicate").
        // Hand-written controllers in this assembly are picked up by MVC application parts automatically.
        var assembly = typeof(AgentToolsAspNetCoreTestModule).Assembly;
        PreConfigure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(assembly, o =>
            {
                o.RemoteServiceName = SampleRemoteServices.AgentToolsSample;
                // Hand-written controllers implementing app service interfaces are IRemoteService too: keep them out,
                // as a module's HttpApi assembly is never a conventional controller assembly.
                o.TypePredicate = t => !typeof(ControllerBase).IsAssignableFrom(t)
                                       && t != typeof(OtherSampleAppService) && t != typeof(DuplicateNameAppService)
                                       && t.Namespace != typeof(BookCrudAppService).Namespace
                                       && t.Namespace != typeof(DiagnosticsSampleAppService).Namespace
                                       && t.Namespace != typeof(BindingSampleAppService).Namespace;
            });
            options.ConventionalControllers.Create(assembly, o =>
            {
                o.RemoteServiceName = BindingSampleAppService.RemoteServiceName;
                o.RootPath = BindingSampleAppService.RootPath;
                o.TypePredicate = t => t == typeof(BindingSampleAppService);
            });
            options.ConventionalControllers.Create(assembly, o =>
            {
                o.RemoteServiceName = DiagnosticsSampleAppService.RemoteServiceName;
                o.RootPath = DiagnosticsSampleAppService.RootPath;
                o.TypePredicate = t => t == typeof(DiagnosticsSampleAppService);
            });
            options.ConventionalControllers.Create(assembly, o =>
            {
                o.RemoteServiceName = SampleRemoteServices.OtherSample;
                o.RootPath = SampleRemoteServices.OtherSampleRootPath;
                o.TypePredicate = t => t == typeof(OtherSampleAppService);
            });
            options.ConventionalControllers.Create(assembly, o =>
            {
                o.RemoteServiceName = SampleRemoteServices.DuplicateSample;
                o.RootPath = SampleRemoteServices.DuplicateSampleRootPath;
                o.TypePredicate = t => t == typeof(DuplicateNameAppService);
            });
            options.ConventionalControllers.Create(assembly, o =>
            {
                o.RemoteServiceName = GovernanceSampleConsts.RemoteServiceName;
                o.RootPath = GovernanceSampleConsts.RootPath;
                o.TypePredicate = t => t == typeof(BookCrudAppService);
            });
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
        // Integration services become controllers, so the catalogue's integration-service exclusion is exercised.
        Configure<AbpAspNetCoreMvcOptions>(options => options.ExposeIntegrationServices = true);
        Configure<AbpMultiTenancyOptions>(options => options.IsEnabled = true);

        context.Services.AddAbpDbContext<SampleDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        _database = new AbpUnitTestSqliteDatabase();
        _database.CreateTables(new SampleDbContext(
            new DbContextOptionsBuilder<SampleDbContext>()
                .UseSqlite(_database.ConnectionString)
                .AddAbpDbContextOptionsExtension()
                .Options));

        Configure<AbpDbConnectionOptions>(o => o.ConnectionStrings.Default = _database.ConnectionString);
        Configure<AbpDbContextOptions>(o => o.Configure(c => c.UseSqlite()));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        app.UseRouting();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _database?.Dispose();
    }
}
