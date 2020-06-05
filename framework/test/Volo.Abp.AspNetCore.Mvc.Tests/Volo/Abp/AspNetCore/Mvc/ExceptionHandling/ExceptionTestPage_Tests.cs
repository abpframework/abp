using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Shouldly;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class ExceptionTestPage_Tests : AspNetCoreMvcTestBase
    {
        private IExceptionSubscriber _fakeExceptionSubscriber;

        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            base.ConfigureServices(context, services);

            _fakeExceptionSubscriber = Substitute.For<IExceptionSubscriber>();

            services.AddSingleton(_fakeExceptionSubscriber);
        }

        [Fact]
        public async Task Should_Not_Handle_Exceptions_For_Void_Return_Values()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(
                async () => await GetResponseAsStringAsync(
                    "/ExceptionHandling/ExceptionTestPage?handler=UserFriendlyException_Void"
                )
            );

#pragma warning disable 4014
            _fakeExceptionSubscriber
                .DidNotReceive()
                .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014
        }

        [Fact]
        public async Task Should_Not_Handle_Exceptions_For_Task_Return_Values()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(
                async () => await GetResponseAsStringAsync(
                    "/ExceptionHandling/ExceptionTestPage?handler=UserFriendlyException_Task"
                )
            );

#pragma warning disable 4014
            _fakeExceptionSubscriber
                .DidNotReceive()
                .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014
        }

        [Fact]
        public async Task Should_Not_Handle_Exceptions_For_ActionResult_Return_Values()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(
                async () => await GetResponseAsStringAsync(
                    "/ExceptionHandling/ExceptionTestPage?handler=UserFriendlyException_ActionResult"
                )
            );

#pragma warning disable 4014
            _fakeExceptionSubscriber
                .DidNotReceive()
                .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014
        }

        [Fact]
        public async Task Should_Return_RemoteServiceErrorResponse_For_UserFriendlyException_For_Object_Return_Value()
        {
            var result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/ExceptionHandling/ExceptionTestPage?handler=UserFriendlyException_JsonResult", HttpStatusCode.Forbidden);
            result.Error.ShouldNotBeNull();
            result.Error.Message.ShouldBe("This is a sample exception!");

#pragma warning disable 4014
            _fakeExceptionSubscriber
                .Received()
                .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014
        }

        [Fact]
        public async Task Should_Return_RemoteServiceErrorResponse_For_UserFriendlyException_For_Task_Object_Return_Value()
        {
            var result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/ExceptionHandling/ExceptionTestPage?handler=UserFriendlyException_Task_JsonResult", HttpStatusCode.Forbidden);
            result.Error.ShouldNotBeNull();
            result.Error.Message.ShouldBe("This is a sample exception!");

#pragma warning disable 4014
            _fakeExceptionSubscriber
                .Received()
                .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014
        }
    }
}
