using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.PermissionManagement;

// ReSharper disable once CheckNamespace
namespace Volo.Abp.PermissionManagement.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IPermissionAppService), typeof(PermissionsClientProxy))]
    public partial class PermissionsClientProxy : ClientProxyBase<IPermissionAppService>, IPermissionAppService
    {
    }
}
