using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Features
{
    public class HostFeatureValueProvider : FeatureValueProvider
    {
        public const string ProviderName = "H";

        public override string Name => ProviderName;

        protected ICurrentTenant CurrentTenant { get; }

        public HostFeatureValueProvider(IFeatureStore featureStore, ICurrentTenant currentTenant)
            : base(featureStore)
        {
            CurrentTenant = currentTenant;
        }

        public override async Task<string> GetOrNullAsync(FeatureDefinition feature)
        {
            if (CurrentTenant.Id.HasValue || !feature.IsAvailableToHost)
            {
                return null;
            }

            return await FeatureStore.GetOrNullAsync(feature.Name, Name, null);
        }
    }
}
