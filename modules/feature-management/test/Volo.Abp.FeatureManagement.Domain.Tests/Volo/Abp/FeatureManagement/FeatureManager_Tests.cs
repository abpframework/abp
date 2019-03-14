using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Features;
using Xunit;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureManager_Tests : FeatureManagementDomainTestBase
    {
        private readonly IFeatureManager _featureManager;

        public FeatureManager_Tests()
        {
            _featureManager = GetRequiredService<IFeatureManager>();
        }

        [Fact]
        public async Task Should_Get_A_FeatureValue_For_A_Provider()
        {
            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.BackupCount,
                TestEditionIds.Enterprise
            )).ShouldBe("5");
        }
    }
}
