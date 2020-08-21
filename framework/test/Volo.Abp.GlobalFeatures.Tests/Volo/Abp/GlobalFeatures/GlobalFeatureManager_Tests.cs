using Shouldly;
using Xunit;

namespace Volo.Abp.GlobalFeatures
{
    public class GlobalFeatureManager_Tests
    {
        private readonly GlobalFeatureManager _featureManeger;

        public GlobalFeatureManager_Tests()
        {
            _featureManeger = new GlobalFeatureManager();
        }

        [Fact]
        public void Enable_Feature_By_Name()
        {
            _featureManeger.IsEnabled("Feature1").ShouldBeFalse();
            _featureManeger.Enable("Feature1");
            _featureManeger.IsEnabled("Feature1").ShouldBeTrue();
        }
    }
}
