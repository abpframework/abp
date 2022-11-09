using Shouldly;
using Xunit;

namespace Volo.Abp.Localization;

public class CultureHelper_Tests
{
    [Fact]
    public void IsCompatibleCulture()
    {
        CultureHelper.IsCompatibleCulture("tr", "tr").ShouldBeTrue();
        CultureHelper.IsCompatibleCulture("tr", "tr-TR").ShouldBeTrue();

        CultureHelper.IsCompatibleCulture("en", "tr").ShouldBeFalse();
        CultureHelper.IsCompatibleCulture("en", "tr-TR").ShouldBeFalse();

        CultureHelper.IsCompatibleCulture("en-US", "en").ShouldBeFalse();
        CultureHelper.IsCompatibleCulture("en-US", "en-GB").ShouldBeFalse();

        CultureHelper.IsCompatibleCulture("zh", "zh-CN").ShouldBeTrue();
        CultureHelper.IsCompatibleCulture("zh", "zh-HK").ShouldBeTrue();
        CultureHelper.IsCompatibleCulture("zh", "zh-MO").ShouldBeTrue();
        CultureHelper.IsCompatibleCulture("zh", "zh-SG").ShouldBeTrue();
        CultureHelper.IsCompatibleCulture("zh", "zh-TW").ShouldBeTrue();
        CultureHelper.IsCompatibleCulture("zh", "zh-Hans").ShouldBeTrue();
        CultureHelper.IsCompatibleCulture("zh", "zh-Hant").ShouldBeTrue();
        CultureHelper.IsCompatibleCulture("zh-Hans", "zh-CN").ShouldBeTrue();
        CultureHelper.IsCompatibleCulture("zh-Hans", "zh-SG").ShouldBeTrue();
        CultureHelper.IsCompatibleCulture("zh-Hant", "zh-HK").ShouldBeTrue();
        CultureHelper.IsCompatibleCulture("zh-Hant", "zh-MO").ShouldBeTrue();
        CultureHelper.IsCompatibleCulture("zh-Hant", "zh-TW").ShouldBeTrue();

        CultureHelper.IsCompatibleCulture("zh-Hans", "zh-HK").ShouldBeFalse();
        CultureHelper.IsCompatibleCulture("zh-Hant", "zh-SG").ShouldBeFalse();
    }
}
