using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Features
{
    public class EditionFeatureValueProvider : FeatureValueProvider
    {
        public const string ProviderName = "E";

        public override string Name => ProviderName;

        protected ICurrentPrincipalAccessor PrincipalAccessor;

        public EditionFeatureValueProvider(IFeatureStore featureStore, ICurrentPrincipalAccessor principalAccessor)
            : base(featureStore)
        {
            PrincipalAccessor = principalAccessor;
        }

        public override async Task<string> GetOrNullAsync(FeatureDefinition feature)
        {
            var editionId = PrincipalAccessor.Principal?.FindEditionId();
            if (editionId == null)
            {
                return null;
            }

            return await FeatureStore.GetOrNullAsync(feature.Name, Name, editionId.Value.ToString("N"));
        }
    }
}