using DemoApp.Data;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Mapperly;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileExplorer.Web;

namespace DemoApp;

[DependsOn(
    // ABP Framework packages
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpMapperlyModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),

    // basic-theme
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),

    // VirtualFileExplorer module packages
    typeof(AbpVirtualFileExplorerWebModule),

    // Account module packages
    typeof(AbpAccountWebModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAccountApplicationModule),

    // Identity module packages
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),

    // Permission Management module packages
    typeof(AbpPermissionManagementWebModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule)
)]
public class DemoAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PermissionManagementOptions>(options =>
        {
            options.SaveStaticPermissionsToDatabase = false;
        });

        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
        });

        context.Services.AddAbpDbContext<DemoAppDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(configurationContext =>
            {
                configurationContext.UseSqlServer();
            });
        });
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
            .GetRequiredService<DemoAppDbContext>()
            .Database
            .MigrateAsync();

        await context.ServiceProvider
            .GetRequiredService<IDataSeeder>()
            .SeedAsync();

        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.MapAbpStaticAssets();

        app.UseRouting();
        app.UseUnitOfWork();
        app.UseAuthentication();
        app.UseMultiTenancy();
        app.UseAuthorization();
        app.UseConfiguredEndpoints();
    }
}
