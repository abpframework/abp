using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.GlobalFeatures
{
    public class RequiresGlobalFeatureTestPage_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task Should_404_If_Feature_Disabled()
        {
            var result = await GetResponseAsync("/GlobalFeatures/DisabledGlobalFeatureTestPage", HttpStatusCode.NotFound);
            result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_Pass_Check_If_Feature_Enabled()
        {
            var result = await GetResponseAsync("/GlobalFeatures/EnabledGlobalFeatureTestPage", HttpStatusCode.OK);
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
