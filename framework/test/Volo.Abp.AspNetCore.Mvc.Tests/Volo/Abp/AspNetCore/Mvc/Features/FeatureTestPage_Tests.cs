using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Features
{
    public class FeatureTestPage_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task Should_Allow_Enabled_Features()
        {
            await GetResponseAsStringAsync(
                "/Features/FeatureTestPage?handler=AllowedFeature"
            );
        }

        [Fact]
        public async Task Should_Not_Allow_Not_Enabled_Features()
        {
            await GetResponseAsStringAsync(
                "/Features/FeatureTestPage?handler=NotAllowedFeature",
                HttpStatusCode.Unauthorized
            );
        }

        [Fact]
        public async Task Should_Allow_Actions_With_No_Feature()
        {
            await GetResponseAsStringAsync(
                "/Features/FeatureTestPage?handler=NoFeature"
            );
        }
    }
}