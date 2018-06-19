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

        public static Dictionary<string, string> LocalizationMapping { get; } = new Dictionary<string, string>
        {
            //TODO: Add all!
            {"ar", DefaultLocalizationFolder + "messages_ar.js"},
            {"tr", DefaultLocalizationFolder + "messages_tr.js"}
        };

        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/jquery-validation/jquery.validate.js");
            //context.PostDynamicFiles.AddIfNotContains("/libs/jquery-validation/localization/messages_" + CultureInfo.CurrentUICulture.Name + ".js");
        }

        public void ConfigureDynamic(BundleConfigurationContext context)
        {
            //TODO: !!!
            context.Files.AddIfNotContains("/libs/jquery-validation/localization/messages_" + CultureInfo.CurrentUICulture.Name + ".js");
        }
    }
}
