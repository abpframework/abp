using System.Collections.Generic;
using System.Globalization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.Timeago
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class TimeagoScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/timeago/jquery.timeago.js");
        }

        public override void ConfigureDynamicResources(BundleConfigurationContext context)
        {
            var cultureName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            if (cultureName.StartsWith("en"))
            {
                return;
            }

            var cultureFileName = $"/libs/timeago/locales/jquery.timeago.{cultureName}.js";

            if (context.FileProvider.GetFileInfo(cultureFileName).Exists)
            {
                context.Files.Add(cultureFileName);
            }
        }
    }
}
