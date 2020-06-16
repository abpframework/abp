using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.BootstrapDatepicker;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiBundlingModule))]
    public class AbpAspNetCoreMvcUiPackagesModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.AddLanguageFilesMap(BootstrapDatepickerScriptContributor.PackageName,
                    new NameValue("zh-Hans", "zh-CN"));
            });
        }
    }
}
