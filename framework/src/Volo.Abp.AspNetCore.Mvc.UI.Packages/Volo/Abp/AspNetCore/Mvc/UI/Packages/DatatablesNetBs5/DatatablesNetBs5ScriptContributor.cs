using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.DatatablesNet;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.DatatablesNetBs5;

[DependsOn(typeof(DatatablesNetScriptContributor))]
[DependsOn(typeof(BootstrapScriptContributor))]
public class DatatablesNetBs5ScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/datatables.net-bs5/js/dataTables.bootstrap5.js");
    }
}
