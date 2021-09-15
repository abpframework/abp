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

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class AbpAuthorizationExceptionTestPage_Tests : AspNetCoreMvcTestBase
    {
        private IExceptionSubscriber _fakeExceptionSubscriber;

        private FakeUserClaims _fakeRequiredService;

        public AbpAuthorizationExceptionTestPage_Tests()
        {
            _fakeRequiredService = GetRequiredService<FakeUserClaims>();
        }

        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            base.ConfigureServices(context, services);

            _fakeExceptionSubscriber = Substitute.For<IExceptionSubscriber>();

            services.AddSingleton(_fakeExceptionSubscriber);

            services.Configure<AbpAuthorizationExceptionHandlerOptions>(options =>
            {
                options.AuthenticationScheme = "Cookie";
            });
        }

        [Fact]
        public virtual async Task Should_Handle_By_Cookie_AuthenticationScheme_For_AbpAuthorizationException()
        {
            var result = await GetResponseAsync("/ExceptionHandling/ExceptionTestPage?handler=AbpAuthorizationException", HttpStatusCode.Redirect);
            result.Headers.Location.ToString().ShouldContain("http://localhost/Account/Login");

#pragma warning disable 4014
            _fakeExceptionSubscriber
                .Received()
                .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014


            _fakeRequiredService.Claims.AddRange(new[]
            {
                new Claim(AbpClaimTypes.UserId, Guid.NewGuid().ToString())
            });

            result = await GetResponseAsync("/ExceptionHandling/ExceptionTestPage?handler=AbpAuthorizationException", HttpStatusCode.Redirect);
            result.Headers.Location.ToString().ShouldContain("http://localhost/Account/AccessDenied");

#pragma warning disable 4014
            _fakeExceptionSubscriber
                .Received()
                .HandleAsync(Arg.Any<ExceptionNotificationContext>());
#pragma warning restore 4014
        }
    }
}
