using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Features;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.FeatureManagement
{
    public abstract class FeatureValueRepository_Tests<TStartupModule> : FeatureManagementTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IFeatureValueRepository Repository { get; set; }

        protected FeatureValueRepository_Tests()
        {
            Repository = GetRequiredService<IFeatureValueRepository>();
        }

        [Fact]
        public async Task FindAsync()
        {
            //feature value does exists

            var featureValue = await Repository.FindAsync(
                TestFeatureDefinitionProvider.ProjectCount,
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Enterprise.ToString()
            );

            featureValue.ShouldNotBeNull();
            featureValue.Value.ShouldBe("3");

            //feature value does not exists
            featureValue = await Repository.FindAsync(
                TestFeatureDefinitionProvider.ProjectCount,
                EditionFeatureValueProvider.ProviderName,
                "undefined-edition-id"
            );

            featureValue.ShouldBeNull();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var featureValues = await Repository.GetListAsync(
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Enterprise.ToString()
            );

            featureValues.Count.ShouldBeGreaterThan(0);

            featureValues.ShouldContain(
                fv => fv.Name == TestFeatureDefinitionProvider.SocialLogins &&
                      fv.Value == "true"
            );

            featureValues.ShouldContain(
                fv => fv.Name == TestFeatureDefinitionProvider.ProjectCount &&
                      fv.Value == "3"
            );
        }
    }
}
