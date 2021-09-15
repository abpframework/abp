// This file is part of BlogPostPublicClientProxy, you can customize it here
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Public.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Public.Blogs.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IBlogPostPublicAppService), typeof(BlogPostPublicClientProxy))]
    public partial class BlogPostPublicClientProxy : ClientProxyBase<IBlogPostPublicAppService>, IBlogPostPublicAppService
    {
    }
}
