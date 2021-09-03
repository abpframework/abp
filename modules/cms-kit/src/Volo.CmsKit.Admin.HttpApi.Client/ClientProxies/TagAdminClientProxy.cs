using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Admin.Tags;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Tags.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ITagAdminAppService), typeof(TagAdminClientProxy))]
    public partial class TagAdminClientProxy : ClientProxyBase<ITagAdminAppService>, ITagAdminAppService
    {
    }
}
