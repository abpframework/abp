using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Docs.Bundling
{
    public class PrismjsStyleBundleContributorDocsExtension : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            ReplaceOne(context.Files, "/libs/prismjs/themes/prism.css","/libs/prismjs/themes/prism-okaidia.css");
            context.Files.AddIfNotContains("/libs/prismjs/plugins/line-highlight/prism-line-highlight.css");
            context.Files.AddIfNotContains("/libs/prismjs/plugins/toolbar/prism-toolbar.css");
            context.Files.AddIfNotContains("/libs/prismjs/plugins/diff-highlight/prism-diff-highlight.css");
        }
        
        private static void ReplaceOne(List<BundleFile> files, string oldFile, string newFile)
        {
            var index = files.FindIndex(x => x.FileName == oldFile);
            if (index >= 0)
            {
                files[index] = new BundleFile(newFile);
            }
        }
    }
}
