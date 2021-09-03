using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Public.Pages;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Public.Pages.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IPagePublicAppService), typeof(PagesPublicClientProxy))]
    public partial class PagesPublicClientProxy : ClientProxyBase<IPagePublicAppService>, IPagePublicAppService
    {
    }
}
