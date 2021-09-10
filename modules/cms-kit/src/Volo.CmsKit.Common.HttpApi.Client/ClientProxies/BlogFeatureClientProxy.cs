// This file is part of BlogFeatureClientProxy, you can customize it here
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Blogs.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IBlogFeatureAppService), typeof(BlogFeatureClientProxy))]
    public partial class BlogFeatureClientProxy : ClientProxyBase<IBlogFeatureAppService>, IBlogFeatureAppService
    {
    }
}
