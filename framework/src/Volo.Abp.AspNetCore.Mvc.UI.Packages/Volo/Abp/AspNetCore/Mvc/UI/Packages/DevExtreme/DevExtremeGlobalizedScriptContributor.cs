using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Globalize;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JsZip;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.DevExtreme
{
    [DependsOn(typeof(JsZipScriptContributor))]
    [DependsOn(typeof(GlobalizeScriptContributor))]
    public class DevExtremeGlobalizedScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/cldr/cldr.js");
            context.Files.AddIfNotContains("/libs/cldr/event.js");
            context.Files.AddIfNotContains("/libs/cldr/supplemental.js");
            context.Files.AddIfNotContains("/libs/cldr/unresolved.js");
            context.Files.AddIfNotContains("/libs/globalize/globalize.js");
            context.Files.AddIfNotContains("/libs/globalize/message.js");
            context.Files.AddIfNotContains("/libs/globalize/number.js");
            context.Files.AddIfNotContains("/libs/globalize/currency.js");
            context.Files.AddIfNotContains("/libs/globalize/date.js");
        }
    }
}
