using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.BootstrapSelect
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class BootstrapSelectScriptContributor : BundleContributor
    {
        public const string PackageName = "bootstrap-select";

        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/bootstrap-select/bootstrap-select.min.js");
        }

        public override void ConfigureDynamicResources(BundleConfigurationContext context)
        {
            var fileName = context.LocalizationOptions.GetCurrentUICultureLanguageFilesMap(PackageName);
            var filePath = $"/libs/bootstrap-select/locales/defaults-{fileName}.js";
            if (context.FileProvider.GetFileInfo(filePath).Exists)
            {
                context.Files.AddIfNotContains(filePath);
            }
        }
    }
}