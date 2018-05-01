using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    public class UnitOfWorkMiddleware_Exception_Rollback_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task Should_Rollback_Transaction_For_Handled_Exceptions()
        {
            var result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/unitofwork-test/HandledException", HttpStatusCode.Forbidden);
            result.Error.ShouldNotBeNull();
            result.Error.Message.ShouldBe("This is a sample exception!");
        }

        [Fact]
        public async Task Should_Gracefully_Handle_Exceptions_On_Complete()
        {
            var response = await GetResponseAsync("/api/unitofwork-test/ExceptionOnComplete", HttpStatusCode.Forbidden);

            response.Headers.GetValues(AbpHttpConsts.AbpErrorFormat).FirstOrDefault().ShouldBe("true");

            var resultAsString = await response.Content.ReadAsStringAsync();

            var result = ServiceProvider.GetRequiredService<IJsonSerializer>().Deserialize<RemoteServiceErrorResponse>(resultAsString);

            result.Error.ShouldNotBeNull();
            result.Error.Message.ShouldBe(TestUnitOfWorkConfig.ExceptionOnCompleteMessage);
        }
    }
}