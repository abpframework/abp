using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.GlobalFeatures;

public class RequiresGlobalFeatureTestController_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task Should_404_If_Feature_Disabled()
    {
        var result = await GetResponseAsync("/api/DisabledGlobalFeatureTestController-Test/TestMethod", HttpStatusCode.NotFound);
        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Pass_Check_If_Feature_Enabled()
    {
        var result = await GetResponseAsync("/api/EnabledGlobalFeatureTestController-Test/TestMethod", HttpStatusCode.OK);
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
