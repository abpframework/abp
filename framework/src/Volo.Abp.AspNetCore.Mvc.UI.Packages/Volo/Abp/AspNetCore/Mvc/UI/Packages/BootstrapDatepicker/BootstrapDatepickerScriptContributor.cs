using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.BootstrapDatepicker
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class BootstrapDatepickerScriptContributor : BundleContributor
    {
        public const string PackageName = "bootstrap-datepicker";

        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/bootstrap-datepicker/bootstrap-datepicker.min.js");
        }

        public override void ConfigureDynamicResources(BundleConfigurationContext context)
        {
            var fileName = context.LazyServiceProvider.LazyGetRequiredService<IOptions<AbpLocalizationOptions>>().Value.GetCurrentUICultureLanguageFilesMap(PackageName);
            var filePath = $"/libs/bootstrap-datepicker/locales/bootstrap-datepicker.{fileName}.min.js";
            if (context.FileProvider.GetFileInfo(filePath).Exists)
            {
                context.Files.AddIfNotContains(filePath);
            }
        }
    }
}
