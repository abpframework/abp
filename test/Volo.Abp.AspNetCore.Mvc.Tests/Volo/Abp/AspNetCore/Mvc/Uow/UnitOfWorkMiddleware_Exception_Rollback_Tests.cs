using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Http;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    public class UnitOfWorkMiddleware_Exception_Rollback_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task Should_Rollback_Transaction_For_Handled_Exceptions()
        {
            var result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/unitofwork-test/HandledException", HttpStatusCode.InternalServerError);
            result.Error.ShouldNotBeNull();
            result.Error.Message.ShouldBe("This is a sample exception!");
        }

        [Fact]
        public async Task Should_Gracefully_Handle_Exceptions_On_Complete()
        {
            var result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/unitofwork-test/ExceptionOnComplete", HttpStatusCode.InternalServerError);
            result.Error.ShouldNotBeNull();
            result.Error.Message.ShouldBe(TestUnitOfWorkConfig.ExceptionOnCompleteMessage);
        }
    }
}