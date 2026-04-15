using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Localization;

public class RouteBasedCulture_Tests : IAsyncLifetime
{
    private WebApplication _app;
    private HttpClient _client;

    public async Task InitializeAsync()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Host.UseAutofac();
        await builder.AddApplicationAsync<RouteBasedCultureTestModule>();
        _app = builder.Build();
        await _app.InitializeApplicationAsync();
        await _app.StartAsync();
        _client = ((IHost)_app).GetTestClient();
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        if (_app != null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }

    [Fact]
    public async Task RouteBasedCulture_SetsCultureCorrectly()
    {
        var response = await _client!.GetStringAsync("/tr/culture");
        response.ShouldBe("tr");
    }

    [Fact]
    public async Task RouteBasedCulture_SetsCookieOnResponse()
    {
        var response = await _client!.GetAsync("/tr/culture");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        response.Headers.Contains("Set-Cookie").ShouldBeTrue();
        var cookieValue = string.Join(";", response.Headers.GetValues("Set-Cookie"));
        cookieValue.ShouldContain(CookieRequestCultureProvider.DefaultCookieName);
        cookieValue.ShouldContain("tr");
    }

    [Fact]
    public async Task RouteBasedCulture_InvalidCultureCodeFallsThrough()
    {
        // "xyz1234" is not a valid culture - should fall through to the default culture "en"
        var response = await _client!.GetStringAsync("/xyz1234/culture");
        response.ShouldBe("en");
    }

    [Fact]
    public async Task RouteBasedCulture_ApiRoutesNotAffected()
    {
        // /api/data has no {culture} prefix route - falls through to the default culture "en"
        var response = await _client!.GetStringAsync("/api/data");
        response.ShouldBe("en");
    }
}
