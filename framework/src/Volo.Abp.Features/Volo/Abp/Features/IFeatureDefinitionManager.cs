using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Features;

public interface IFeatureDefinitionManager
{
    [NotNull]
    Task<FeatureDefinition> GetAsync([NotNull] string name);

    Task<IReadOnlyList<FeatureDefinition>> GetAllAsync();

    Task<FeatureDefinition?> GetOrNullAsync(string name);

    Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync();
}
