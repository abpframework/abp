using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement
{
    //TODO: Implement caching

    public class FeatureStore : IFeatureStore, ITransientDependency
    {
        protected IFeatureValueRepository FeatureValueRepository { get; }

        public FeatureStore(IFeatureValueRepository featureValueRepository)
        {
            FeatureValueRepository = featureValueRepository;
        }

        public async Task<string> GetOrNullAsync(
            string name, 
            string providerName, 
            string providerKey)
        {
            var featureValue = await FeatureValueRepository.FindAsync(name, providerName, providerKey);
            return featureValue?.Value;
        }
    }
}
