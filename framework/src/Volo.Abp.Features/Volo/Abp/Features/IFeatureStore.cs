using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Features;

public interface IFeatureStore
{
    Task<string> GetOrNullAsync(
        [NotNull] string name,
        [CanBeNull] string providerName,
        [CanBeNull] string providerKey
    );
}
