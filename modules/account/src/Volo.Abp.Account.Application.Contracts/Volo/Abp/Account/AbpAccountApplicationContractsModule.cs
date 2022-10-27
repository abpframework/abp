using Volo.Abp.Account.Localization;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Account;

[DependsOn(
    typeof(AbpIdentityApplicationContractsModule)
)]
public class AbpAccountApplicationContractsModule : AbpModule
{
    private readonly static OneTimeRunner OneTimeRunner = new OneTimeRunner();
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountApplicationContractsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AccountResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Volo/Abp/Account/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Volo.Account", typeof(AccountResource));
        });
    }
    
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.User,
                getApiTypes: new[] { typeof(ProfileDto) },
                updateApiTypes: new[] { typeof(UpdateProfileDto) }
            );
        });
    }
}
