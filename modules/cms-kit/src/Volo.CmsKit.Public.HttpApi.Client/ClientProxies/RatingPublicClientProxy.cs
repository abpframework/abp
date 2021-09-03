using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Public.Ratings;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Public.Ratings.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IRatingPublicAppService), typeof(RatingPublicClientProxy))]
    public partial class RatingPublicClientProxy : ClientProxyBase<IRatingPublicAppService>, IRatingPublicAppService
    {
    }
}
