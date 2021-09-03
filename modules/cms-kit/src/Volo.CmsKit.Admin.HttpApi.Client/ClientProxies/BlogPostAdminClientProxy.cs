using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Admin.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Blogs.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IBlogPostAdminAppService), typeof(BlogPostAdminClientProxy))]
    public partial class BlogPostAdminClientProxy : ClientProxyBase<IBlogPostAdminAppService>, IBlogPostAdminAppService
    {
    }
}
