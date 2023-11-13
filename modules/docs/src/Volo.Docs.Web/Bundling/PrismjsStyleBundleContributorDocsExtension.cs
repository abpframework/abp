using System.Collections.Generic;
using System.Linq;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Docs.Bundling
{
    public class PrismjsStyleBundleContributorDocsExtension : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            var prismCss = context.Files.FirstOrDefault(x => x.FileName == "/libs/prismjs/themes/prism.css");
            if (prismCss != null)
            {
                prismCss.FileName = "/libs/prismjs/themes/prism-okaidia.css";
            }
            context.Files.AddIfNotContains("/libs/prismjs/plugins/line-highlight/prism-line-highlight.css");
            context.Files.AddIfNotContains("/libs/prismjs/plugins/toolbar/prism-toolbar.css");
            context.Files.AddIfNotContains("/libs/prismjs/plugins/diff-highlight/prism-diff-highlight.css");
        }
    }
}
