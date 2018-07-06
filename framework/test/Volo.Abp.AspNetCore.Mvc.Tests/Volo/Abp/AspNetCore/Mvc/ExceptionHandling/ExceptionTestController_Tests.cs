using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Http;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class ExceptionTestController_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task Should_Return_RemoteServiceErrorResponse_For_UserFriendlyException_For_Void_Return_Value()
        {
            var result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/exception-test/UserFriendlyException1", HttpStatusCode.Forbidden);
            result.Error.ShouldNotBeNull();
            result.Error.Message.ShouldBe("This is a sample exception!");
        }

        [Fact]
        public async Task Should_Not_Handle_Exceptions_For_ActionResult_Return_Values()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(
                async () => await GetResponseAsObjectAsync<RemoteServiceErrorResponse>(
                    "/api/exception-test/UserFriendlyException2"
                )
            );
        }
    }
}
