using System.Collections.Generic;
using System.Globalization;
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
            var cultureName = CultureInfo.CurrentUICulture.DateTimeFormat.Calendar.AlgorithmType ==
                              CalendarAlgorithmType.LunarCalendar
                ? "en"
                : CultureInfo.CurrentUICulture.Name;

            TryAddCultureFile(context, cultureName);
        }

        protected virtual bool TryAddCultureFile(BundleConfigurationContext context, string cultureName)
        {
            var fileName = context.LocalizationOptions.GetLanguageFilesMap(PackageName, cultureName);
            var filePath = $"/libs/bootstrap-datepicker/locales/bootstrap-datepicker.{fileName}.min.js";

            if (!context.FileProvider.GetFileInfo(filePath).Exists)
            {
                return false;
            }

            context.Files.AddIfNotContains(filePath);
            return true;
        }
    }
}
