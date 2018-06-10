using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Bootstrap;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.DatatablesNetBs4
{
    [DependsOn(typeof(BootstrapStyleBundleContributor))]
    public class DatatablesNetBs4StyleBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/datatables.net-bs4/css/dataTables.bootstrap4.css");
        }
    }
}