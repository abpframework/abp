using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Codemirror;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.HighlightJs;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Prismjs;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.TuiEditor;

[DependsOn(
    typeof(PrismjsStyleBundleContributor)
)]
public class TuiEditorStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/tui-editor/toastui-editor.min.css");
        context.Files.AddIfNotContains("/libs/tui-editor/toastui-editor-plugin-code-syntax-highlight.min.css");
    }
}
