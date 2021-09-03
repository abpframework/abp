using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Blogging.Tagging;

// ReSharper disable once CheckNamespace
namespace Volo.Blogging.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ITagAppService), typeof(TagsClientProxy))]
    public partial class TagsClientProxy : ClientProxyBase<ITagAppService>, ITagAppService
    {
    }
}
