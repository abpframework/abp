using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.DatatablesNet;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.DatatablesNetBs4;

[DependsOn(typeof(DatatablesNetScriptContributor))]
[DependsOn(typeof(BootstrapScriptContributor))]
public class DatatablesNetBs4ScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/datatables.net-bs4/js/dataTables.bootstrap4.js");
    }
}
