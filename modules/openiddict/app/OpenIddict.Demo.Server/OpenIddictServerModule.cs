using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Demo.Server.EntityFrameworkCore;
using OpenIddict.Demo.Server.ExtensionGrants;
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
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
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
using Volo.Abp.Uow;

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
        PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
        {
            //https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html
            options.AddDevelopmentEncryptionAndSigningCertificate = false;
        });

        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", "00000000-0000-0000-0000-000000000000");

            builder.Configure(openIddictServerOptions =>
            {
                openIddictServerOptions.GrantTypes.Add(MyTokenExtensionGrant.ExtensionGrantName);
            });
        });

        PreConfigure<AbpOpenIddictWildcardDomainOptions>(options =>
        {
            options.EnableWildcardDomainSupport = true;
            options.WildcardDomainsFormat.Add("https://*.abp.io");
        });

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("AbpAPIResource");

                options.UseLocalServer();

                options.UseAspNetCore();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
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

        Configure<AbpOpenIddictExtensionGrantsOptions>(options =>
        {
            options.Grants.Add(MyTokenExtensionGrant.ExtensionGrantName, new MyTokenExtensionGrant());
        });
    }

    public async override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        using var uow = context.ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin();
        {
            var dbContext = await context.ServiceProvider.GetRequiredService<IDbContextProvider<ServerDbContext>>().GetDbContextAsync();
            if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                await dbContext.Database.MigrateAsync();
            }

            await uow.CompleteAsync();
        }

        await context.ServiceProvider
            .GetRequiredService<IDataSeeder>()
            .SeedAsync();

        var tenantManager = context.ServiceProvider.GetRequiredService<TenantManager>();
        var tenantRepository = context.ServiceProvider.GetRequiredService<ITenantRepository>();
        var tenant = await tenantRepository.FindByNameAsync("Default") ??
                     await tenantRepository.InsertAsync(await tenantManager.CreateAsync("Default"));
        await context.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync(tenant.Id);
    }
}
