using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.Globalize
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class GlobalizeScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/cldrjs/cldr.js");
            context.Files.AddIfNotContains("/libs/cldrjs/event.js");
            context.Files.AddIfNotContains("/libs/cldrjs/supplemental.js");
            context.Files.AddIfNotContains("/libs/globalize/globalize.js");
            context.Files.AddIfNotContains("/libs/globalize/globalize/message.js");
            context.Files.AddIfNotContains("/libs/globalize/globalize/number.js");
            context.Files.AddIfNotContains("/libs/globalize/globalize/currency.js");
            context.Files.AddIfNotContains("/libs/globalize/globalize/date.js");
        }
    }
}
