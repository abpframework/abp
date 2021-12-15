using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Shouldly;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling;

public class ExceptionTestController_Tests : AspNetCoreMvcTestBase
{
    private IExceptionSubscriber _fakeExceptionSubscriber;

    private FakeUserClaims FakeRequiredService;

    public ExceptionTestController_Tests()
    {
        FakeRequiredService = GetRequiredService<FakeUserClaims>();
    }

    protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        base.ConfigureServices(context, services);

        _fakeExceptionSubscriber = Substitute.For<IExceptionSubscriber>();

        services.AddSingleton(_fakeExceptionSubscriber);
    }

    [Fact]
    public async Task Should_Return_RemoteServiceErrorResponse_For_UserFriendlyException_For_Void_Return_Value()
    {
        var result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/exception-test/UserFriendlyException1", HttpStatusCode.Forbidden);
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe("This is a sample exception!");

#pragma warning disable 4014
        _fakeExceptionSubscriber
            .Received()
            .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014
    }

    [Fact]
    public async Task Should_Not_Handle_Exceptions_For_ActionResult_Return_Values()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(
            async () => await GetResponseAsObjectAsync<RemoteServiceErrorResponse>(
                "/api/exception-test/UserFriendlyException2"
            )
        );

#pragma warning disable 4014
        _fakeExceptionSubscriber
            .DidNotReceive()
            .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014
    }

    [Fact]
    public virtual async Task Should_Handle_By_Cookie_AuthenticationScheme_For_AbpAuthorizationException_For_Void_Return_Value()
    {
        FakeRequiredService.Claims.AddRange(new[]
        {
                new Claim(AbpClaimTypes.UserId, Guid.NewGuid().ToString())
            });

        var result = await GetResponseAsync("/api/exception-test/AbpAuthorizationException", HttpStatusCode.Redirect);
        result.Headers.Location.ToString().ShouldContain("http://localhost/Account/AccessDenied");

#pragma warning disable 4014
        _fakeExceptionSubscriber
            .Received()
            .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014
    }

    [Fact]
    public virtual async Task Should_Handle_By_JwtBearer_AuthenticationScheme_For_AbpAuthorizationException_For_Void_Return_Value()
    {
        var result = await GetResponseAsync("/api/exception-test/AbpAuthorizationException", HttpStatusCode.Unauthorized);
        result.Headers.WwwAuthenticate.ToString().ShouldBe("Bearer");

#pragma warning disable 4014
        _fakeExceptionSubscriber
            .Received()
            .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014
    }
}
