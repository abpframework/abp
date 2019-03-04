using Shouldly;
using Xunit;

namespace Volo.Abp.Features
{
    public class FeatureDefinitionManager_Tests : FeatureTestBase
    {
        private readonly IFeatureDefinitionManager _featureDefinitionManager;

        public FeatureDefinitionManager_Tests()
        {
            _featureDefinitionManager = GetRequiredService<IFeatureDefinitionManager>();
        }

        [Fact]
        public void Should_Get_Defined_Features()
        {
            _featureDefinitionManager.GetOrNull("BooleanTestFeature1").ShouldNotBeNull();
            _featureDefinitionManager.Get("BooleanTestFeature1").Name.ShouldBe("BooleanTestFeature1");

            _featureDefinitionManager.GetOrNull("IntegerTestFeature1").ShouldNotBeNull();
            _featureDefinitionManager.Get("IntegerTestFeature1").Name.ShouldBe("IntegerTestFeature1");
        }

        [Fact]
        public void Should_Not_Get_Undefined_Features()
        {
            _featureDefinitionManager.GetOrNull("UndefinedFeature").ShouldBeNull();
            Assert.Throws<AbpException>(() =>
            {
                _featureDefinitionManager.Get("UndefinedFeature");
            });
        }
    }
}
