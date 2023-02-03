using Volo.Abp.Localization.TestResources.Base.Validation;
using Volo.Abp.Localization.TestResources.External;
using Volo.Abp.Localization.TestResources.Source;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Localization;

[DependsOn(typeof(AbpTestBaseModule))]
[DependsOn(typeof(AbpLocalizationModule))]
public class AbpLocalizationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpLocalizationTestModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.DefaultResourceType = typeof(LocalizationTestResource);

            options.Resources
                .Add<LocalizationTestValidationResource>("en")
                .AddVirtualJson("/Volo/Abp/Localization/TestResources/Base/Validation");

            options.Resources
                .Add("LocalizationTestCountryNames")
                .AddVirtualJson("/Volo/Abp/Localization/TestResources/Base/CountryNames");

            options.Resources
                .Add<LocalizationTestResource>("en")
                .AddVirtualJson("/Volo/Abp/Localization/TestResources/Source")
                .AddBaseResources("LocalizationTestCountryNames");

            options.Resources
                .Get<LocalizationTestResource>()
                .AddVirtualJson("/Volo/Abp/Localization/TestResources/SourceExt");

            options.GlobalContributors.Add<TestExternalLocalizationContributor>();
        });
    }
}
