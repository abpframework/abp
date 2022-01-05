using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Shouldly;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling;

public class AbpAuthorizationExceptionTestController_Tests : AspNetCoreMvcTestBase
{
    protected IExceptionSubscriber FakeExceptionSubscriber;

    protected FakeUserClaims FakeRequiredService;

    public AbpAuthorizationExceptionTestController_Tests()
    {
        FakeRequiredService = GetRequiredService<FakeUserClaims>();
    }

    protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        base.ConfigureServices(context, services);

        FakeExceptionSubscriber = Substitute.For<IExceptionSubscriber>();

        services.AddSingleton(FakeExceptionSubscriber);

        services.Configure<AbpAuthorizationExceptionHandlerOptions>(options =>
        {
            options.AuthenticationScheme = "Cookie";
        });
    }

    [Fact]
    public virtual async Task Should_Handle_By_Cookie_AuthenticationScheme_For_AbpAuthorizationException()
    {
        var result = await GetResponseAsync("/api/exception-test/AbpAuthorizationException", HttpStatusCode.Redirect);
        result.Headers.Location.ToString().ShouldContain("http://localhost/Account/Login");

#pragma warning disable 4014
        FakeExceptionSubscriber
            .Received()
            .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014


        FakeRequiredService.Claims.AddRange(new[]
        {
                new Claim(AbpClaimTypes.UserId, Guid.NewGuid().ToString())
            });

        result = await GetResponseAsync("/api/exception-test/AbpAuthorizationException", HttpStatusCode.Redirect);
        result.Headers.Location.ToString().ShouldContain("http://localhost/Account/AccessDenied");

#pragma warning disable 4014
        FakeExceptionSubscriber
            .Received()
            .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014
    }
}
