using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Admin.Tags;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Tags.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IEntityTagAdminAppService), typeof(EntityTagAdminClientProxy))]
    public partial class EntityTagAdminClientProxy : ClientProxyBase<IEntityTagAdminAppService>, IEntityTagAdminAppService
    {
    }
}
