using System.Collections.Generic;
using System.Globalization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.Timeago
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class BootstrapDatepickerScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/bootstrap-datepicker/bootstrap-datepicker.min.js");
        }

        public override void ConfigureDynamicResources(BundleConfigurationContext context)
        {
            var cultureName = CultureInfo.CurrentUICulture.DateTimeFormat.Calendar.AlgorithmType ==
                              CalendarAlgorithmType.LunarCalendar
                ? "en"
                : CultureInfo.CurrentUICulture.Name;

            if (TryAddCultureFile(context, cultureName))
            {
                return;
            }
        }

        protected virtual bool TryAddCultureFile(BundleConfigurationContext context, string cultureName)
        {
            var filePath = $"/libs/bootstrap-datepicker/locales/bootstrap-datepicker.{cultureName}.min.js";

            if (!context.FileProvider.GetFileInfo(filePath).Exists)
            {
                return false;
            }

            context.Files.AddIfNotContains(filePath);
            return true;
        }
    }
}
