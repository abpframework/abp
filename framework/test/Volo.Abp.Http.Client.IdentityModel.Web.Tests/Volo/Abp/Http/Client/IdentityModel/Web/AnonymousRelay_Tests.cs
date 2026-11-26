using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.IdentityModel;
using Xunit;

namespace Volo.Abp.Http.Client.IdentityModel.Web;

public class AnonymousRelay_Tests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(false, true)]
    public async Task Anonymous_Http_Relay_Does_Not_Acquire_Service_Identity_Without_OptIn(bool? useCurrentToken, bool allowClientCredentials)
    {
        using var api = new TestServer(new WebHostBuilder().Configure(app => app.Run(context =>
        {
            context.Response.StatusCode = context.Request.Headers.Authorization == "Bearer service-token"
                ? StatusCodes.Status200OK : StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        })));
        using (var control = api.CreateClient())
        {
            (await control.GetAsync("/users")).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            control.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "service-token");
            (await control.GetAsync("/users")).StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        var authentication = Substitute.For<IIdentityModelAuthenticationService>();
        authentication.TryAuthenticateAsync(Arg.Any<HttpClient>(), Arg.Any<string>()).Returns(call =>
        {
            call.Arg<HttpClient>().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "service-token");
            return Task.FromResult(true);
        });
        using var web = new TestServer(new WebHostBuilder()
            .ConfigureServices(services => services.AddHttpContextAccessor())
            .Configure(app => app.Run(async context =>
            {
                using var client = api.CreateClient();
                using var request = new HttpRequestMessage(HttpMethod.Get, "/users");
                var accessor = context.RequestServices.GetRequiredService<IHttpContextAccessor>();
                accessor.HttpContext.ShouldBe(context);
                var authenticator = new HttpContextIdentityModelRemoteServiceHttpClientAuthenticator(authentication,
                    new HttpContextAbpAccessTokenProvider(accessor), accessor);
                var configuration = new RemoteServiceConfiguration("https://api.test")
                    .SetUseCurrentAccessToken(useCurrentToken)
                    .SetAllowClientCredentialsInHttpContext(allowClientCredentials);
                await authenticator.Authenticate(new RemoteServiceHttpClientAuthenticateContext(client, request, configuration, "Users"));
                using var response = await client.SendAsync(request);
                context.Response.StatusCode = (int)response.StatusCode;
            })));
        using var visitor = web.CreateClient();
        using var result = await visitor.GetAsync("/api/users");
        result.StatusCode.ShouldBe(allowClientCredentials ? HttpStatusCode.OK : HttpStatusCode.Unauthorized);
        await authentication.Received(allowClientCredentials ? 1 : 0).TryAuthenticateAsync(Arg.Any<HttpClient>(), "Users");
    }
}
