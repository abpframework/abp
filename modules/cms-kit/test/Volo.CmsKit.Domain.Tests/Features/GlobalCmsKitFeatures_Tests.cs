using Shouldly;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Xunit;

namespace Volo.CmsKit.Features;

public class GlobalCmsKitFeatures_Tests : CmsKitDomainTestBase
{
    private readonly GlobalCmsKitFeatures _cmsKitFeatures;

    public GlobalCmsKitFeatures_Tests()
    {
        _cmsKitFeatures = new GlobalCmsKitFeatures(GlobalFeatureManager.Instance);
    }

    [Fact]
    public void Page_Feature_Should_Enable_Dependent_Features()
    {
        _cmsKitFeatures.DisableAll();
        _cmsKitFeatures.User.IsEnabled.ShouldBeFalse();
        _cmsKitFeatures.Pages.IsEnabled.ShouldBeFalse();

        _cmsKitFeatures.Pages.Enable();
        _cmsKitFeatures.User.IsEnabled.ShouldBeTrue();
        _cmsKitFeatures.Pages.IsEnabled.ShouldBeTrue();
    }
}
