using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore
{
    public class AbpSignInManager_Tests : AbpIdentityAspNetCoreTestBase
    {
        [Fact]
        public async Task Should_SignIn_With_Correct_Credentials()
        {
            var result = await GetResponseAsStringAsync(
                "api/signin-test/password?userName=admin&password=1q2w3E*"
            );

            result.ShouldBe("Succeeded");
        }
        [Fact]
        public async Task Should_Not_SignIn_With_Wrong_Credentials()
        {
            var result = await GetResponseAsStringAsync(
                "api/signin-test/password?userName=admin&password=WRONG_PASSWORD"
            );

            result.ShouldBe("Failed");
        }

        //TODO: Move to a better common place ----------------------------------------------------

        protected virtual async Task<string> GetResponseAsStringAsync(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var response = await GetResponseAsync(url, expectedStatusCode);
            return await response.Content.ReadAsStringAsync();
        }

        protected virtual async Task<HttpResponseMessage> GetResponseAsync(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var response = await Client.GetAsync(url);
            response.StatusCode.ShouldBe(expectedStatusCode);
            return response;
        }
    }
}
