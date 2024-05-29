using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Shared.Components.Comments;

public class CommentSettingScriptBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/client-proxies/cms-kit-admin-proxy.js");
        context.Files.AddIfNotContains("/Pages/CmsKit/Shared/Components/Comments/default.js");
    }
}
