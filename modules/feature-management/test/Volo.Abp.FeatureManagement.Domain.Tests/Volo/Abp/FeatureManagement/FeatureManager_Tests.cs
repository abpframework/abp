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
            ).ConfigureAwait(false)).ShouldBeNull();

            (await _featureManager.GetOrNullDefaultAsync(
                TestFeatureDefinitionProvider.DailyAnalysis
            ).ConfigureAwait(false)).ShouldBe(false.ToString().ToLowerInvariant());

            (await _featureManager.GetOrNullDefaultAsync(
                TestFeatureDefinitionProvider.ProjectCount
            ).ConfigureAwait(false)).ShouldBe("1");

            (await _featureManager.GetOrNullDefaultAsync(
                TestFeatureDefinitionProvider.BackupCount
            ).ConfigureAwait(false)).ShouldBe("0");

            //"Enterprise" edition values

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.SocialLogins,
                TestEditionIds.Enterprise
            ).ConfigureAwait(false)).ShouldBe(true.ToString().ToLowerInvariant());

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.DailyAnalysis,
                TestEditionIds.Enterprise
            ).ConfigureAwait(false)).ShouldBe(false.ToString().ToLowerInvariant());

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.ProjectCount,
                TestEditionIds.Enterprise
            ).ConfigureAwait(false)).ShouldBe("3");

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.BackupCount,
                TestEditionIds.Enterprise
            ).ConfigureAwait(false)).ShouldBe("5");

            //"Ultimate" edition values

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.SocialLogins,
                TestEditionIds.Ultimate
            ).ConfigureAwait(false)).ShouldBe(true.ToString().ToLowerInvariant());

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.DailyAnalysis,
                TestEditionIds.Ultimate
            ).ConfigureAwait(false)).ShouldBe(true.ToString().ToLowerInvariant());

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.ProjectCount,
                TestEditionIds.Ultimate
            ).ConfigureAwait(false)).ShouldBe("10");

            (await _featureManager.GetOrNullForEditionAsync(
                TestFeatureDefinitionProvider.BackupCount,
                TestEditionIds.Ultimate
            ).ConfigureAwait(false)).ShouldBe("10");
        }
    }
}
