using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Features;

public class FeatureTestController_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task Should_Allow_Enabled_Features()
    {
        await GetResponseAsStringAsync(
            "/api/feature-test/allowed-feature", HttpStatusCode.NoContent
        );
    }

    [Fact]
    public async Task Should_Not_Allow_Not_Enabled_Features()
    {
        await GetResponseAsStringAsync(
            "/api/feature-test/not-allowed-feature",
            HttpStatusCode.Unauthorized
        );
    }

    [Fact]
    public async Task Should_Allow_Actions_With_No_Feature()
    {
        await GetResponseAsStringAsync(
            "/api/feature-test/no-feature"
        );
    }
}
