using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.DevExtreme
{
    [DependsOn(typeof(DevExtremeStyleContributor))]
    public class DevExtremeLightCompactStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/devextreme/dist/css/dx.light.compact.css");
        }
    }
}