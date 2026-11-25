using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using ModelContextProtocol.Authentication;
using ModelContextProtocol.Client;
using Shouldly;
using Volo.Abp.AgentTools.Mcp.Sample;
using Xunit;

namespace Volo.Abp.AgentTools.Mcp;

/// <summary>
/// The C# SDK's OAuth client finds the authorization server of an endpoint. The app has a minimal authorization server (metadata
/// and a token endpoint that answers <see cref="AuthorizationCode"/> with a token of the test scheme) and, next to ours, an endpoint
/// that answers every request with a 401 that names no metadata (<see cref="OtherPath"/>).
/// </summary>
public abstract class McpOAuthDiscoveryTestBase : AgentToolsMcpTestBase
{
    protected const string BarePath = "/.well-known/oauth-protected-resource";
    protected const string OtherPath = "/other/mcp";
    private const string AuthorizationCode = "test-code";

    /// <summary>What the client sent the user to.</summary>
    protected List<Uri> AuthorizationRequests { get; } = [];

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpEndpointRouterOptions>(options => options.EndpointConfigureActions.Add(context => Map(context.Endpoints)));
    }

    protected virtual void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/.well-known/oauth-authorization-server", () => Results.Json(new
        {
            issuer = "http://localhost/",
            authorization_endpoint = "http://localhost/authorize",
            token_endpoint = "http://localhost/token",
            response_types_supported = new[] { "code" },
            code_challenge_methods_supported = new[] { "S256" }
        }));
        endpoints.MapPost("/token", async (HttpContext http) => (await http.Request.ReadFormAsync())["code"] == AuthorizationCode
            ? Results.Json(new { access_token = "test", token_type = "Bearer", expires_in = 3600 })
            : Results.BadRequest());
        endpoints.MapPost(OtherPath, (HttpContext http) =>
        {
            http.Response.Headers.WWWAuthenticate = "Bearer";
            return Results.Unauthorized();
        });
    }

    protected async Task<McpClient> ConnectWithOAuthAsync(string path)
    {
        var http = CreateHttpClient(NewUser(null));
        http.DefaultRequestHeaders.Authorization = null;
        var transport = new HttpClientTransport(
            new HttpClientTransportOptions
            {
                Endpoint = new Uri(http.BaseAddress!, path),
                TransportMode = HttpTransportMode.StreamableHttp,
                OAuth = new ClientOAuthOptions
                {
                    ClientId = "test-client",
                    RedirectUri = new Uri("http://localhost:1/callback"),
                    AuthorizationRedirectDelegate = (authorizationUri, _, _) =>
                    {
                        AuthorizationRequests.Add(authorizationUri);
                        return Task.FromResult<string?>(AuthorizationCode);
                    }
                }
            },
            http, NullLoggerFactory.Instance, ownsHttpClient: true);
        return await McpClient.CreateAsync(transport);
    }
}

public class McpOAuthDiscoveryTests : McpOAuthDiscoveryTestBase
{
    [Fact]
    public async Task The_Client_Finds_The_Authorization_Server_Of_Our_Endpoint_And_Connects()
    {
        await using var client = await ConnectWithOAuthAsync("/mcp");

        (await client.ListToolsAsync()).Select(t => t.Name).ShouldContain(McpSampleAppService.GetNotes);
        var request = AuthorizationRequests.ShouldHaveSingleItem();
        request.AbsolutePath.ShouldBe("/authorize");
        var query = QueryHelpers.ParseQuery(request.Query);
        query["resource"].ToString().ShouldBe("http://localhost/mcp");
        query["scope"].ToString().Split(' ').ShouldContain("AgentTools");
    }

    [Fact]
    public async Task The_Client_Does_Not_Get_Our_Metadata_For_An_Endpoint_Without_A_Document()
    {
        // The endpoint refuses every token.
        await Should.ThrowAsync<HttpRequestException>(() => ConnectWithOAuthAsync(OtherPath));

        AuthorizationRequests.ShouldHaveSingleItem().Query.ShouldNotContain("AgentTools");
    }
}

/// <summary>The app serves the bare metadata document, for its own endpoint.</summary>
public class McpOAuthDiscoveryWithABareDocumentTests : McpOAuthDiscoveryTestBase
{
    protected override void Map(IEndpointRouteBuilder endpoints)
    {
        base.Map(endpoints);
        endpoints.MapGet(BarePath, () => Results.Json(new
        {
            resource = "http://localhost/",
            authorization_servers = new[] { "http://localhost/" },
            scopes_supported = new[] { "Other" }
        }));
    }

    [Fact]
    public async Task Each_Endpoint_Has_Its_Own_Document()
    {
        var http = CreateHttpClient();

        (await http.GetFromJsonAsync<JsonElement>(BarePath)).GetProperty("resource").GetString().ShouldBe("http://localhost/");
        (await http.GetFromJsonAsync<JsonElement>(BarePath + "/mcp")).GetProperty("resource").GetString().ShouldBe("http://localhost/mcp");
    }

    [Fact]
    public async Task The_Client_Asks_For_The_Scope_Of_Each_Endpoint()
    {
        await using var client = await ConnectWithOAuthAsync("/mcp");
        await Should.ThrowAsync<HttpRequestException>(() => ConnectWithOAuthAsync(OtherPath));

        AuthorizationRequests.Select(r => QueryHelpers.ParseQuery(r.Query)["scope"].ToString()).ShouldBe(["AgentTools offline_access", "Other"]);
    }
}
