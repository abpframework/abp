using System.Threading.Tasks;

namespace Volo.Abp.FeatureManagement;

public interface IStaticFeatureSaver
{
    Task SaveAsync();
}
