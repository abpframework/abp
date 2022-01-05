using Shouldly;
using Xunit;

namespace Volo.Abp.GlobalFeatures;

public class GlobalFeatureManager_Tests : GlobalFeatureTestBase
{
    private readonly GlobalFeatureManager _featureManager;

    public GlobalFeatureManager_Tests()
    {
        _featureManager = new GlobalFeatureManager();
    }

    [Fact]
    public void Enable_Feature_By_Name()
    {
        _featureManager.IsEnabled("Feature1").ShouldBeFalse();
        _featureManager.Enable("Feature1");
        _featureManager.IsEnabled("Feature1").ShouldBeTrue();
    }
}
