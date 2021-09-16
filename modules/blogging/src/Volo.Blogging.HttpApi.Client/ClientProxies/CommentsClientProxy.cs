// This file is part of CommentsClientProxy, you can customize it here
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Blogging.Comments;

// ReSharper disable once CheckNamespace
namespace Volo.Blogging.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ICommentAppService), typeof(CommentsClientProxy))]
    public partial class CommentsClientProxy : ClientProxyBase<ICommentAppService>, ICommentAppService
    {
    }
}
