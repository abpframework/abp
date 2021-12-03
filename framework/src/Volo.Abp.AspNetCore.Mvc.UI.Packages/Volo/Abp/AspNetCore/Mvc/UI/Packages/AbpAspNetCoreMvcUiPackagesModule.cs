using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.BootstrapDatepicker;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQueryValidation;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Timeago;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule),
    typeof(AbpLocalizationModule)
    )]
public class AbpAspNetCoreMvcUiPackagesModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
                //BootstrapDatepicker
                options.AddLanguagesMapOrUpdate(BootstrapDatepickerScriptContributor.PackageName,
                new NameValue("zh-Hans", "zh-CN"),
                new NameValue("zh-Hant", "zh-TW"));

            options.AddLanguageFilesMapOrUpdate(BootstrapDatepickerScriptContributor.PackageName,
                new NameValue("zh-Hans", "zh-CN"),
                new NameValue("zh-Hant", "zh-TW"));

                //Timeago
                options.AddLanguageFilesMapOrUpdate(TimeagoScriptContributor.PackageName,
                new NameValue("zh-Hans", "zh-CN"),
                new NameValue("zh-Hant", "zh-TW"));

                //JQueryValidation
                options.AddLanguageFilesMapOrUpdate(JQueryValidationScriptContributor.PackageName,
                new NameValue("zh-Hans", "zh"),
                new NameValue("zh-Hant", "zh_TW"));
        });
    }
}
