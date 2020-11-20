using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.Luxon
{
    public class LuxonScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/luxon/luxon.min.js");
            context.Files.AddIfNotContains("/libs/abp/luxon/abp.luxon.js");
        }
    }
}
