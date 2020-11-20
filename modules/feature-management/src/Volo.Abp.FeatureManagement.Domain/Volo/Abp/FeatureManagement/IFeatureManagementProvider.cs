using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement
{
    public interface IFeatureManagementProvider
    {
        string Name { get; }

        //TODO: Other better method name.
        bool Compatible(string providerName);

        Task<string> GetOrNullAsync([NotNull] FeatureDefinition feature, [CanBeNull] string providerKey);

        Task SetAsync([NotNull] FeatureDefinition feature, [NotNull] string value, [CanBeNull] string providerKey);

        Task ClearAsync([NotNull] FeatureDefinition feature, [CanBeNull] string providerKey);
    }
}
