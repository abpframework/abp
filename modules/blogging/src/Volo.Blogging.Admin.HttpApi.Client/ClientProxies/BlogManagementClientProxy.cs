using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Blogging.Admin.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.Blogging.Admin.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IBlogManagementAppService), typeof(BlogManagementClientProxy))]
    public partial class BlogManagementClientProxy : ClientProxyBase<IBlogManagementAppService>, IBlogManagementAppService
    {
    }
}
