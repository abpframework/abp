// This file is part of CommentPublicClientProxy, you can customize it here
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Public.Comments;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Public.Comments.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ICommentPublicAppService), typeof(CommentPublicClientProxy))]
    public partial class CommentPublicClientProxy : ClientProxyBase<ICommentPublicAppService>, ICommentPublicAppService
    {
    }
}
