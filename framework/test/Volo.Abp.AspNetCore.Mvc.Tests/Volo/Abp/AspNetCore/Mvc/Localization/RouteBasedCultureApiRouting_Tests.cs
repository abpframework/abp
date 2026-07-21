using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.AspNetCore.App;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public class RouteBasedCultureApiRouting_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task Api_Route_Should_Not_Be_Intercepted_By_Culture_Route()
    {
        var response = await GetResponseAsync("api/json-result-test/json-result-action");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType.ShouldBe("application/json");
    }

    [Fact]
    public async Task Controller_Should_Work_With_Culture_Prefix()
    {
        var result = await GetResponseAsStringAsync(
            "/tr" + GetUrl<SimpleController>(nameof(SimpleController.Index)));
        result.ShouldBe("Index-Result");
    }

    [Fact]
    public async Task Controller_Should_Work_Without_Culture_Prefix()
    {
        var result = await GetResponseAsStringAsync(
            GetUrl<SimpleController>(nameof(SimpleController.Index)));
        result.ShouldBe("Index-Result");
    }

    [Fact]
    public async Task RazorPage_Should_Work_With_Culture_Prefix()
    {
        var response = await GetResponseAsync("/tr/Auditing/AuditTestPage");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RazorPage_Should_Work_Without_Culture_Prefix()
    {
        var response = await GetResponseAsync("/Auditing/AuditTestPage");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
