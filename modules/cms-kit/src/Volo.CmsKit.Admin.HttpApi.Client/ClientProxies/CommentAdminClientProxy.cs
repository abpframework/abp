using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Admin.Comments;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Comments.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ICommentAdminAppService), typeof(CommentAdminClientProxy))]
    public partial class CommentAdminClientProxy : ClientProxyBase<ICommentAdminAppService>, ICommentAdminAppService
    {
    }
}
