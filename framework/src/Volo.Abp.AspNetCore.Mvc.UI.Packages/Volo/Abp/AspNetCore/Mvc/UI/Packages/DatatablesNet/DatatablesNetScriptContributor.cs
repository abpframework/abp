using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.DatatablesNet;

[DependsOn(typeof(JQueryScriptContributor))]
public class DatatablesNetScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        if (context.FileProvider.GetFileInfo("/libs/datatables.net/js/dataTables.min.js").Exists)
        {
            context.Files.AddIfNotContains("/libs/datatables.net/js/dataTables.min.js");
        }
        else
        {
            //TODO: Compatible code, Remove it after 8.3 rc1.
            context.Files.AddIfNotContains("/libs/datatables.net/js/jquery.dataTables.js");
        }
    }
}
