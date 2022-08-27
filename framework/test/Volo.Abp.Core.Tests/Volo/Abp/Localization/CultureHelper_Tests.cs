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
    }
}