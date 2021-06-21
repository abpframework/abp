using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.JQueryValidation
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class JQueryValidationScriptContributor : BundleContributor
    {
        public const string PackageName = "jquery-validation";

        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/jquery-validation/jquery.validate.js");
        }

        public override void ConfigureDynamicResources(BundleConfigurationContext context)
        {
            var fileName = context.LazyServiceProvider.LazyGetRequiredService<IOptions<AbpLocalizationOptions>>().Value.GetCurrentUICultureLanguageFilesMap(PackageName);
            var filePath = $"/libs/jquery-validation/localization/messages_{fileName}.js";
            if (context.FileProvider.GetFileInfo(filePath).Exists)
            {
                context.Files.AddIfNotContains(filePath);
            }
        }
    }
}
