using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Features;

public interface IStaticFeatureDefinitionStore
{
    Task<FeatureDefinition> GetOrNullAsync(string name);

    Task<IReadOnlyList<FeatureDefinition>> GetFeaturesAsync();

    Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync();
}
