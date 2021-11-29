using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Bootstrap;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.DatatablesNetBs5
{
    [DependsOn(typeof(BootstrapStyleContributor))]
    public class DatatablesNetBs5StyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/datatables.net-bs5/css/dataTables.bootstrap5.css");
        }
    }
}
