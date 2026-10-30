using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.SignalR.Auditing.TestApp;
using Volo.Abp.AspNetCore.SignalR.SampleHubs;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.SignalR;

[DependsOn(
    typeof(AbpAspNetCoreSignalRModule),
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(AbpAutofacModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
public class AbpAspNetCoreSignalRTestModule : AbpModule
{
    public const string UnitOfWorkReservedHeaderName = "X-Unit-Of-Work-Reserved";

    public const string AuditScopeStartedHeaderName = "X-Audit-Scope-Started";

    public static Exception UseConfiguredEndpointsException { get; set; }

    private AbpUnitTestSqliteDatabase _database;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SignalRAuditingTestDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        _database = new AbpUnitTestSqliteDatabase();
        _database.CreateTables(new SignalRAuditingTestDbContext(
            new DbContextOptionsBuilder<SignalRAuditingTestDbContext>()
                .UseSqlite(_database.ConnectionString)
                .AddAbpDbContextOptionsExtension()
                .Options));

        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = _database.ConnectionString;
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(configurationContext =>
            {
                configurationContext.UseSqlite();
            });
        });

        Configure<AbpAspNetCoreAuditingOptions>(options =>
        {
            options.IgnoredHubTypes.Add(typeof(IgnoredAuditingHub));
        });

        Configure<AbpAuditingOptions>(options =>
        {
            options.EntityHistorySelectors.Add(new NamedTypeSelector(nameof(AuditedNote), type => type == typeof(AuditedNote)));
        });
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _database?.Dispose();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseRouting();
        app.UseUnitOfWork();
        app.UseAuditing();
        app.Use(async (httpContext, next) =>
        {
            httpContext.Response.Headers[UnitOfWorkReservedHeaderName] =
                (httpContext.RequestServices.GetRequiredService<IAmbientUnitOfWork>().UnitOfWork?.IsReserved == true).ToString();
            httpContext.Response.Headers[AuditScopeStartedHeaderName] =
                (httpContext.RequestServices.GetRequiredService<IAuditingManager>().Current != null).ToString();

            await next();
        });

        UseConfiguredEndpointsException = null;
        try
        {
            app.UseConfiguredEndpoints(endpoints =>
            {
                endpoints.MapHub<DisableAutoHubMapHub>("/manually-mapped-hub");
                endpoints.MapHub<IgnoredAuditingHub>("/ignored-auditing-hub");
            });
        }
        catch (Exception e)
        {
            UseConfiguredEndpointsException = e;
        }
    }
}
