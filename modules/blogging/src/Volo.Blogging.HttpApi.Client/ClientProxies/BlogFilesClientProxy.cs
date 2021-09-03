using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Blogging.Files;

// ReSharper disable once CheckNamespace
namespace Volo.Blogging.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IFileAppService), typeof(BlogFilesClientProxy))]
    public partial class BlogFilesClientProxy : ClientProxyBase<IFileAppService>, IFileAppService
    {
    }
}
