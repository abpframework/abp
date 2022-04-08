using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Demo.Server.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity.Web;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.OpenIddict.WildcardDomains;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.Web;

namespace OpenIddict.Demo.Server;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpAspNetCoreMultiTenancyModule),

    typeof(AbpOpenIddictAspNetCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),

    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAccountWebOpenIddictModule),

    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpTenantManagementWebModule),

    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpIdentityWebModule),

    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),

    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpFeatureManagementWebModule),

    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpSettingManagementWebModule)
)]
public class OpenIddictServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            //https://documentation.openiddict.com/configuration/token-formats.html#disabling-jwt-access-token-encryption
            //https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html
            builder.AddSigningKey(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Abp_OpenIddict_Demo_C40DBB176E78")));
            builder.AddEncryptionKey(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Abp_OpenIddict_Demo_87E33FC57D80")));
        });

        PreConfigure<AbpOpenIddictWildcardDomainOptions>(options =>
        {
            options.EnableWildcardDomainSupport = true;
            options.WildcardDomainFormat = "https://{0}.abp.io/signin-oidc";
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpOpenIddictOptions>(options =>
        {
            options.AddDevelopmentEncryptionAndSigningCertificate = false;
        });

        context.Services.AddAbpDbContext<ServerDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseSqlServer();
        });

        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
        });
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var dbContext = context.ServiceProvider
            .GetRequiredService<ServerDbContext>();

        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            await dbContext.Database.MigrateAsync();

            await context.ServiceProvider
                .GetRequiredService<IDataSeeder>()
                .SeedAsync();
        }

        var tenantManager = context.ServiceProvider.GetRequiredService<TenantManager>();
        var tenantRepository = context.ServiceProvider.GetRequiredService<ITenantRepository>();
        if (await tenantRepository.FindByNameAsync("Default") == null)
        {
            var tenant = await tenantRepository.InsertAsync( await tenantManager.CreateAsync("Default"));
            await context.ServiceProvider
                .GetRequiredService<IDataSeeder>()
                .SeedAsync(tenant.Id);
        }
    }
}
