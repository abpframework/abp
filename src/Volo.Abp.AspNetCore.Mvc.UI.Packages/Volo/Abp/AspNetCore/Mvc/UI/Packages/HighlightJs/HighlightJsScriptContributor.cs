using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Core;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.HighlightJs
{
    [DependsOn(typeof(CoreScriptContributor))]
    public class HighlightJsScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/highlight.js/highlight.js");

            //TODO: Add related languages by configuration (these can be default!)
            context.Files.Add("/libs/highlight.js/languages/cs.js");
            context.Files.Add("/libs/highlight.js/languages/css.js");
            context.Files.Add("/libs/highlight.js/languages/javascript.js");
            context.Files.Add("/libs/highlight.js/languages/json.js");
            context.Files.Add("/libs/highlight.js/languages/xml.js");
        }
    }
}
