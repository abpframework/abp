using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.AspNetCore.Uow;
using Xunit;

namespace Volo.Abp.OpenIddict.Integration;

// A real "/connect/token" (client_credentials) request through the OpenIddict server. A probe registered
// outside UseUnitOfWork reads the token count from an independent connection at response start.
public class OpenIddictTokenEndpoint_Integration_Tests : AbpWebApplicationFactoryIntegratedTest<Program>
{
    private AbpAspNetCoreUnitOfWorkOptions Options =>
        ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreUnitOfWorkOptions>>().Value;

    private long? TokenCountAtResponseStart =>
        ServiceProvider.GetRequiredService<TokenVisibilityRecorder>().TokenCountAtResponseStart;

    private Task<HttpResponseMessage> RequestTokenAsync()
    {
        return Client.PostAsync("/connect/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = "test-client",
            ["client_secret"] = "test-secret"
        }));
    }

    [Fact]
    public async Task Token_Row_Is_Committed_Before_The_Connect_Token_Response_Is_Sent()
    {
        // The OpenIddict module opts "/connect" in by default.
        var response = await RequestTokenAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldContain("access_token");
        TokenCountAtResponseStart.ShouldBe(1);
    }

    [Fact]
    public async Task Without_The_Opt_In_The_Token_Is_Not_Committed_When_The_Response_Starts()
    {
        // Negative control: without the opt-in the token is committed only at the end of the pipeline,
        // so the probe reads 0. This proves the positive case genuinely observes response-start timing.
        Options.CompleteUnitOfWorkOnResponseStartingUrls.Clear();
        Options.CompleteUnitOfWorkOnResponseStarting = false;

        var response = await RequestTokenAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        TokenCountAtResponseStart.ShouldBe(0);
    }
}
