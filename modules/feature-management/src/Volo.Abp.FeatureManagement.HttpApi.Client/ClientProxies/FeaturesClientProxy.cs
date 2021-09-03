using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.FeatureManagement;

// ReSharper disable once CheckNamespace
namespace Volo.Abp.FeatureManagement.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IFeatureAppService), typeof(FeaturesClientProxy))]
    public partial class FeaturesClientProxy : ClientProxyBase<IFeatureAppService>, IFeatureAppService
    {
    }
}
