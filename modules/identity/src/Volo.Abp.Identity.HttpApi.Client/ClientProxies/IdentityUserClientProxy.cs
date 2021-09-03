using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Identity;

// ReSharper disable once CheckNamespace
namespace Volo.Abp.Identity.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IIdentityUserAppService), typeof(IdentityUserClientProxy))]
    public partial class IdentityUserClientProxy : ClientProxyBase<IIdentityUserAppService>, IIdentityUserAppService
    {
    }
}
