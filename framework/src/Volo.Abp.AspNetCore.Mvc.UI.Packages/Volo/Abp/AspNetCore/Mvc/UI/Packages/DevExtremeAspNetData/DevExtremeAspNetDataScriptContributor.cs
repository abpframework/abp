using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.DevExtreme;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.DevExtremeAspNetData
{
    [DependsOn(typeof(DevExtremeScriptContributor))]
    public class DevExtremeAspNetDataScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/devextreme-aspnet-data/js/dx.aspnet.data.js");
            context.Files.AddIfNotContains("/libs/devextreme/aspnet.js");
        }
    }
}
