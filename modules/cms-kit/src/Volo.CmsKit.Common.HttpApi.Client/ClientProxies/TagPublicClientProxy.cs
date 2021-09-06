using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Tags;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Public.Tags.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ITagAppService), typeof(TagPublicClientProxy))]
    public partial class TagPublicClientProxy : ClientProxyBase<ITagAppService>, ITagAppService
    {
    }
}
