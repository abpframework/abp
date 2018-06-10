using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.DatatablesNetBs4;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.FontAwesome;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Toastr;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling
{
    [DependsOn(
        typeof(BootstrapStyleContributor),
        typeof(FontAwesomeStyleContributor),
        typeof(ToastrStyleBundleContributor),
        typeof(DatatablesNetBs4StyleContributor)
    )]
    public class SharedThemeGlobalStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/abp/aspnetcore.mvc.ui.theme.shared/datatables/datatables.css");
        }
    }
}