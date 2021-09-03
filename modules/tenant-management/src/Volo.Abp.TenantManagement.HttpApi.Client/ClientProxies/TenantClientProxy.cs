using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.TenantManagement;

// ReSharper disable once CheckNamespace
namespace Volo.Abp.TenantManagement.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ITenantAppService), typeof(TenantClientProxy))]
    public partial class TenantClientProxy : ClientProxyBase<ITenantAppService>, ITenantAppService
    {
    }
}
