using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Admin.Pages;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Pages.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IPageAdminAppService), typeof(PageAdminClientProxy))]
    public partial class PageAdminClientProxy : ClientProxyBase<IPageAdminAppService>, IPageAdminAppService
    {
    }
}
