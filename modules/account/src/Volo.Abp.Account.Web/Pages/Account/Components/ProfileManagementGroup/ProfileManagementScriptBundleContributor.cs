using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup
{
    public class ProfileManagementScriptBundleContributor: BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/client-proxies/identity-proxy.js");
        }
    }
}
