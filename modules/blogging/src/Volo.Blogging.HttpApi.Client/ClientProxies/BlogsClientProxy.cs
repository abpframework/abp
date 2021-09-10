// This file is part of BlogsClientProxy, you can customize it here
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Blogging.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.Blogging.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IBlogAppService), typeof(BlogsClientProxy))]
    public partial class BlogsClientProxy : ClientProxyBase<IBlogAppService>, IBlogAppService
    {
    }
}
