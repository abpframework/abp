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
            //Default values

            (await _featureManager.GetOrNullDefaultAsync(
                TestFeatureDefinitionProvider.SocialLogins
            )).ShouldBeNull();

            (await _featureManager.GetOrNullDefaultAsync(
                TestFeatureDefinitionProvider.DailyAnalysis
            )).ShouldBe(false.ToString().ToLowerInvariant());

            (await _featureManager.GetOrNullDefaultAsync(
                TestFeatureDefinitionProvider.ProjectCount
            )).ShouldBe("1");

            (await _featureManager.GetOrNullDefaultAsync(
                TestFeatureDefinitionProvider.BackupCount
            )).ShouldBe("0");

            //"Enterprise" edition values

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.SocialLogins,
                TestEditionIds.Enterprise
            )).ShouldBe(true.ToString().ToLowerInvariant());

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.DailyAnalysis,
                TestEditionIds.Enterprise
            )).ShouldBe(false.ToString().ToLowerInvariant());

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.ProjectCount,
                TestEditionIds.Enterprise
            )).ShouldBe("3");

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.BackupCount,
                TestEditionIds.Enterprise
            )).ShouldBe("5");

            //"Ultimate" edition values

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.SocialLogins,
                TestEditionIds.Ultimate
            )).ShouldBe(true.ToString().ToLowerInvariant());

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.DailyAnalysis,
                TestEditionIds.Ultimate
            )).ShouldBe(true.ToString().ToLowerInvariant());

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.ProjectCount,
                TestEditionIds.Ultimate
            )).ShouldBe("10");

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.BackupCount,
                TestEditionIds.Ultimate
            )).ShouldBe("10");
        }
    }
}
