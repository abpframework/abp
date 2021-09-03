using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Admin.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Blogs.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IBlogFeatureAdminAppService), typeof(BlogFeatureAdminClientProxy))]
    public partial class BlogFeatureAdminClientProxy : ClientProxyBase<IBlogFeatureAdminAppService>, IBlogFeatureAdminAppService
    {
    }
}
