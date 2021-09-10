// This file is part of PostsClientProxy, you can customize it here
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Blogging.Posts;

// ReSharper disable once CheckNamespace
namespace Volo.Blogging.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IPostAppService), typeof(PostsClientProxy))]
    public partial class PostsClientProxy : ClientProxyBase<IPostAppService>, IPostAppService
    {
    }
}
