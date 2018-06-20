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

        public override void ConfigureDynamicResources(BundleConfigurationContext context)
        {
            //TODO: Can we optimize these points:
            // - Can we get rid of context.FileProvider.GetFileInfo call?
            // - What if the same contributer is used twice for a page.
            //   Duplication is prevented by the bundle manager, however the logic below will execute twice

            var cultureName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Replace('-', '_');

            if (TryAddCultureFile(context, cultureName))
            {
                return;
            }

            if (!cultureName.Contains("_"))
            {
                return;
            }

            TryAddCultureFile(context, cultureName.Substring(0, cultureName.IndexOf('_')));
        }

        protected virtual bool TryAddCultureFile(BundleConfigurationContext context, string cultureName)
        {
            var filePath = DefaultLocalizationFolder + "messages_" + cultureName + ".js";
            var fileInfo = context.FileProvider.GetFileInfo(filePath);

            if (!fileInfo.Exists)
            {
                return false;
            }

            context.Files.AddIfNotContains(filePath);
            return true;
        }
    }
}
