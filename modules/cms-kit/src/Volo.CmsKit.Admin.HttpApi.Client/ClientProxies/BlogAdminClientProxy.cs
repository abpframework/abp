using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Admin.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Blogs.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IBlogAdminAppService), typeof(BlogAdminClientProxy))]
    public partial class BlogAdminClientProxy : ClientProxyBase<IBlogAdminAppService>, IBlogAdminAppService
    {
    }
}
