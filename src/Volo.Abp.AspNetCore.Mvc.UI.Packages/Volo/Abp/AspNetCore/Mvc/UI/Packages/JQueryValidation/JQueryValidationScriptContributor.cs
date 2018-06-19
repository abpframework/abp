using System.Collections.Generic;
using System.Globalization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.JQueryValidation
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class JQueryValidationScriptContributor : BundleContributor
    {
        public const string DefaultLocalizationFolder = "/libs/jquery-validation/localization/";

        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/jquery-validation/jquery.validate.js");
        }

        public void ConfigureDynamic(BundleConfigurationContext context)
        {
            //TODO: Can we optimize this? Also refactor a bit to reduce duplication

            var currentCultureName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Replace('-', '_');
            var filePath = DefaultLocalizationFolder + "messages_" + currentCultureName + ".js";
            var fileInfo = context.FileProvider.GetFileInfo(filePath);
            if (fileInfo != null)
            {
                context.Files.AddIfNotContains(filePath);
                return;
            }

            if (!currentCultureName.Contains("_"))
            {
                return;
            }

            currentCultureName = currentCultureName.Substring(0, currentCultureName.IndexOf('_'));
            filePath = DefaultLocalizationFolder + "messages_" + currentCultureName + ".js";
            fileInfo = context.FileProvider.GetFileInfo(filePath);
            if (fileInfo != null)
            {
                context.Files.AddIfNotContains(filePath);
            }
        }
    }
}
