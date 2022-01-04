using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Docs.Bundling
{
    public class PrismjsStyleBundleContributorDocsExtension : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.ReplaceOne("/libs/prismjs/themes/prism.css","/libs/prismjs/themes/prism-okaidia.css");
            context.Files.AddIfNotContains("/libs/prismjs/plugins/line-highlight/prism-line-highlight.css");
            context.Files.AddIfNotContains("/libs/prismjs/plugins/toolbar/prism-toolbar.css");
        }
    }
}
