using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileExplorer.Web.Localization;

namespace Volo.Abp.VirtualFileExplorer.Web.Navigation;

public class VirtualFileExplorerMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var l = context.GetLocalizer<VirtualFileExplorerResource>();

        context.Menu.Items.Add(new ApplicationMenuItem(VirtualFileExplorerMenuNames.Index, l["Menu:VirtualFileExplorer"], icon: "fa fa-file", url: "~/VirtualFileExplorer"));

        return Task.CompletedTask;
    }
}
