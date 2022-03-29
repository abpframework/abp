using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.OpenIddict;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class OpenIddictDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<OpenIddictDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpOpenIddictResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/OpenIddict");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("OpenIddict", typeof(AbpOpenIddictResource));
        });
    }
}
