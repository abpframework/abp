using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Public.Menus;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Public.Menus.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IMenuItemPublicAppService), typeof(MenuItemPublicClientProxy))]
    public partial class MenuItemPublicClientProxy : ClientProxyBase<IMenuItemPublicAppService>, IMenuItemPublicAppService
    {
    }
}
