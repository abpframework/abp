using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Admin.Menus;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Menus.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IMenuItemAdminAppService), typeof(MenuItemAdminClientProxy))]
    public partial class MenuItemAdminClientProxy : ClientProxyBase<IMenuItemAdminAppService>, IMenuItemAdminAppService
    {
    }
}
