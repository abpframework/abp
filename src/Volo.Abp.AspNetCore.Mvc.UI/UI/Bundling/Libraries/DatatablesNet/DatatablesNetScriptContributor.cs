using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.JQuery;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.DatatablesNet
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class DatatablesNetScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/datatables.net/js/jquery.dataTables.js");
        }
    }
}