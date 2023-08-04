using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity;

[DependsOn(
    typeof(AbpIdentityEntityFrameworkCoreTestModule),
    typeof(AbpIdentityTestBaseModule),
    typeof(AbpPermissionManagementDomainIdentityModule)
    )]
public class AbpIdentityDomainTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.AutoEventSelectors.Add<IdentityUser>();
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpIdentityDomainTestModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<IdentityResource>()
                .AddVirtualJson("/Volo/Abp/Identity/LocalizationExtensions");
        });

        Configure<PermissionManagementOptions>(options =>
        {
            options.IsDynamicPermissionStoreEnabled = false;
            options.SaveStaticPermissionsToDatabase = false;
        });

        Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
        });

        Configure<AbpSettingOptions>(options =>
        {
            options.ValueProviders.Add<TestSettingValueProvider>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            AsyncHelper.RunSync(() => scope.ServiceProvider
                .GetRequiredService<TestPermissionDataBuilder>()
                .Build());
        }
    }
}
