using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.SignalR.Auditing;

[Collection("AbpAspNetCoreSignalR")]
public class AbpSignalRAuditingMiddleware_Tests : AbpAspNetCoreTestBase<Program>
{
    [Theory]
    [InlineData("/signalr-hubs/regular-abp/negotiate?negotiateVersion=1")]
    [InlineData("/custom-route-hub/negotiate?negotiateVersion=1")]
    public async Task Should_Not_Begin_An_Audit_Scope_For_The_Requests_Of_The_Auto_Mapped_Hubs(string url)
    {
        var response = await Client.PostAsync(url, null);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        IsAuditScopeStarted(response).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Not_Begin_An_Audit_Scope_For_The_Requests_Of_The_Ignored_Hub_Types()
    {
        var response = await Client.PostAsync("/ignored-auditing-hub/negotiate?negotiateVersion=1", null);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        IsAuditScopeStarted(response).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Begin_An_Audit_Scope_For_The_Requests_Of_The_Other_Manually_Mapped_Hubs()
    {
        var response = await Client.PostAsync("/manually-mapped-hub/negotiate?negotiateVersion=1", null);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        IsAuditScopeStarted(response).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Begin_An_Audit_Scope_For_The_Other_Requests()
    {
        var response = await Client.GetAsync("/not-a-hub");

        IsAuditScopeStarted(response).ShouldBeTrue();
    }

    private static bool IsAuditScopeStarted(HttpResponseMessage response)
    {
        return bool.Parse(response.Headers.GetValues(AbpAspNetCoreSignalRTestModule.AuditScopeStartedHeaderName).Single());
    }
}
