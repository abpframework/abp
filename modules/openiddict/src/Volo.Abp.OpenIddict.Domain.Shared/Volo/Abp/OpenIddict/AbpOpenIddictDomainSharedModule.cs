using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.OpenIddict
{
    [DependsOn(
        typeof(AbpValidationModule),
        typeof(AbpIdentityDomainSharedModule)
    )]
    public class AbpOpenIddictDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpOpenIddictDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpOpenIddictResource>("en")
                    .AddVirtualJson("/Localization/OpenIddict");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("OpenIddict", typeof(AbpOpenIddictResource));
            });
        }
    }
}
