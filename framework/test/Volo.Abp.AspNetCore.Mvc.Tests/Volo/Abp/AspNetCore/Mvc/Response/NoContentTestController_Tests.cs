using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Response
{
    public class NoContentTestController_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task Should_Set_No_Content_For_Void_Action()
        {
            var result = await GetResponseAsync("/api/NoContent-Test/TestMethod", HttpStatusCode.NoContent)
                ;
            result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_Not_Set_No_Content_For_Not_Void_Action()
        {
            var result = await GetResponseAsync("/api/NoContent-Test/TestMethodWithReturn")
                ;
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Not_Set_No_Content_For_Custom_Http_Status_Code_Action()
        {
            var result = await GetResponseAsync("/api/NoContent-Test/TestCustomHttpStatusCodeMethod", HttpStatusCode.Redirect)
                ;
            result.StatusCode.ShouldBe(HttpStatusCode.Redirect);
        }

        [Fact]
        public async Task Should_Set_No_Content_For_Task_Action()
        {
            var result = await GetResponseAsync("/api/NoContent-Test/TestAsyncMethod", HttpStatusCode.NoContent)
                ;
            result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_Not_Set_No_Content_For_Not_Task_Action()
        {
            var result = await GetResponseAsync("/api/NoContent-Test/TestAsyncMethodWithReturn")
                ;
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Not_Set_No_Content_For_Custom_Http_Status_Code_Async_Action()
        {
            var result = await GetResponseAsync("/api/NoContent-Test/TestAsyncCustomHttpStatusCodeMethod", HttpStatusCode.Redirect)
                ;
            result.StatusCode.ShouldBe(HttpStatusCode.Redirect);
        }
    }
}
