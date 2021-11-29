using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Security.Headers
{
    public class SecurityHeadersTestController_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task SecurityHeaders_Should_Be_Added()
        {
            var responseMessage = await GetResponseAsync("/SecurityHeadersTest/Get");
            responseMessage.Headers.ShouldContain(x => x.Key == "X-Content-Type-Options" & x.Value.First().ToString() == "nosniff");
            responseMessage.Headers.ShouldContain(x => x.Key == "X-XSS-Protection" & x.Value.First().ToString() == "1; mode=block");
            responseMessage.Headers.ShouldContain(x => x.Key == "X-Frame-Options" & x.Value.First().ToString() == "SAMEORIGIN");
        }
    }
}
