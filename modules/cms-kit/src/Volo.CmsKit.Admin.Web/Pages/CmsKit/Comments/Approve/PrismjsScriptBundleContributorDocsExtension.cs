using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Comments.Approve;

public class PrismjsScriptBundleContributorDocsExtension : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        //AddPlugins(context);
        context.Files.Add("~/libs/markdown-it/markdown-it.min.js");
    }
    //private static void AddPlugins(IBundleConfigurationContext context)
    //{
    //    context.Files.AddIfNotContains("/libs/markdown-it/markdown-it.min.js");
    //}
}
