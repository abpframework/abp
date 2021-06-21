using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.Timeago
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class TimeagoScriptContributor : BundleContributor
    {
        public const string PackageName = "jquery.timeago";

        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/timeago/jquery.timeago.js");
        }

        public override void ConfigureDynamicResources(BundleConfigurationContext context)
        {
            var fileName = context.LazyServiceProvider.LazyGetRequiredService<IOptions<AbpLocalizationOptions>>().Value.GetCurrentUICultureLanguageFilesMap(PackageName);
            var filePath = $"/libs/timeago/locales/jquery.timeago.{fileName}.js";
            if (context.FileProvider.GetFileInfo(filePath).Exists)
            {
                context.Files.Add(filePath);
            }
        }
    }
}
