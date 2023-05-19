using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement;

public class FeatureStore : IFeatureStore, ITransientDependency
{
    protected IFeatureManagementStore FeatureManagementStore { get; }

    public FeatureStore(IFeatureManagementStore featureManagementStore)
    {
        FeatureManagementStore = featureManagementStore;
    }

    public virtual Task<string> GetOrNullAsync(
        string name,
        string providerName,
        string providerKey)
    {
        return FeatureManagementStore.GetOrNullAsync(name, providerName, providerKey);
    }
}
