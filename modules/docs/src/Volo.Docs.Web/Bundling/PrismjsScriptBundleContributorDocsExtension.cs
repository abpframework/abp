using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Docs.Bundling
{
    public class PrismjsScriptBundleContributorDocsExtension : BundleContributor
    {
        private static readonly string[] SupportedLanguages = new[] {
            "csharp",
            "json",
            "aspnet",
            "bash",
            "css",
            "css-extras",
            "docker",
            "javascript",
            "less",
            "markdown",
            "nginx",
            "powershell",
            "regex",
            "sass",
            "scss",
            "sql",
            "typescript"
        };

        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            AddPlugins(context);
            AddLanguages(context);
        }

        private static void AddLanguages(IBundleConfigurationContext context)
        {
            const string componentsPath = "/libs/prismjs/components/prism-{0}.js";
            foreach (var language in SupportedLanguages)
            {
                context.Files.AddIfNotContains(string.Format(componentsPath, language));
            }
        }

        private static void AddPlugins(IBundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/prismjs/plugins/toolbar/prism-toolbar.js");
            context.Files.AddIfNotContains("/libs/prismjs/plugins/show-language/prism-show-language.js");
            context.Files.AddIfNotContains("/libs/prismjs/plugins/copy-to-clipboard/prism-copy-to-clipboard.js");
            context.Files.AddIfNotContains("/libs/prismjs/plugins/line-highlight/prism-line-highlight.js");
        }
    }
}
