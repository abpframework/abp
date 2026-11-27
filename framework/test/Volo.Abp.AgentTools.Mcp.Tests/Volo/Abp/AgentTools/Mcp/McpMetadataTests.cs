using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.AgentTools.Mcp;

public class McpMetadataTests : AgentToolsMcpTestBase
{
    [Fact]
    public async Task Metadata_Is_Served_At_The_Path_Of_The_Endpoint_With_The_Requests_Issuer_And_The_Scope()
    {
        var cookie = await LoginWithCookieAsync(Guid.NewGuid(), TenantA);
        foreach (var withCookie in new[] { false, true })
        {
            var client = CreateHttpClient();
            if (withCookie)
            {
                client.DefaultRequestHeaders.Add("Cookie", cookie);
            }

            var response = await client.GetAsync("/.well-known/oauth-protected-resource/mcp");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            response.Content.Headers.ContentType!.MediaType.ShouldBe("application/json");
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            json.GetProperty("resource").GetString().ShouldBe("http://localhost/mcp");
            Strings(json, "authorization_servers").ShouldBe(["http://localhost/"]);
            Strings(json, "scopes_supported").ShouldBe(["AgentTools", "offline_access"]);
            Strings(json, "bearer_methods_supported").ShouldBe(["header"]);
            json.TryGetProperty("resource_name", out _).ShouldBeFalse();
        }
    }

    [Fact]
    public async Task The_Bare_Path_Is_Not_Served()
    {
        (await CreateHttpClient().GetAsync("/.well-known/oauth-protected-resource")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    internal static string[] Strings(JsonElement json, string name) =>
        json.GetProperty(name).EnumerateArray().Select(e => e.GetString()!).ToArray();
}

public class McpConfiguredMetadataTests : AgentToolsMcpTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpAgentToolsMcpOptions>(o =>
        {
            o.Path = "/agent-mcp";
            o.AuthorizationServer = new Uri("https://auth.example.test/");
            o.RequiredScope = "CustomScope";
            o.ServerName = "Test MCP";
        });
    }

    [Fact]
    public async Task Configured_Metadata_Is_Served_At_The_Path_Of_The_Endpoint_And_The_401_Hint_Leads_To_It()
    {
        var client = CreateHttpClient();

        var json = await client.GetFromJsonAsync<JsonElement>("/.well-known/oauth-protected-resource/agent-mcp");
        json.GetProperty("resource").GetString().ShouldBe("http://localhost/agent-mcp");
        json.GetProperty("resource_name").GetString().ShouldBe("Test MCP");
        McpMetadataTests.Strings(json, "authorization_servers").ShouldBe(["https://auth.example.test/"]);
        McpMetadataTests.Strings(json, "scopes_supported").ShouldBe(["CustomScope", "offline_access"]);

        var unauthorized = await PostInitializeAsync(client, "/agent-mcp");
        unauthorized.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        var header = unauthorized.Headers.WwwAuthenticate.ToString();
        header.ShouldContain("resource_metadata=\"http://localhost/.well-known/oauth-protected-resource/agent-mcp\"");
        header.ShouldNotContain("oauth-protected-resource/mcp\"");

        var hinted = new Uri(header.Split("resource_metadata=\"")[1].TrimEnd('"'));
        (await client.GetAsync(hinted)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await client.GetAsync("/.well-known/oauth-protected-resource/mcp")).StatusCode.ShouldNotBe(HttpStatusCode.OK);
        (await client.GetAsync("/.well-known/oauth-protected-resource")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}

/// <summary>An endpoint at the root of the app: the bare path is its own path-specific one.</summary>
public abstract class McpRootMetadataTests : AgentToolsMcpTestBase
{
    protected abstract string Path { get; }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpAgentToolsMcpOptions>(o => o.Path = Path);
    }

    [Fact]
    public async Task Metadata_Is_Served_Once_At_The_Bare_Path_And_The_401_Hint_Leads_To_It()
    {
        var client = CreateHttpClient();

        var json = await client.GetFromJsonAsync<JsonElement>("/.well-known/oauth-protected-resource");
        json.GetProperty("resource").GetString().ShouldBe("http://localhost" + Path);

        var unauthorized = await PostInitializeAsync(client, "/");
        unauthorized.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        var hint = unauthorized.Headers.WwwAuthenticate.ToString().Split("resource_metadata=\"")[1].TrimEnd('"');
        hint.ShouldBe("http://localhost/.well-known/oauth-protected-resource" + Path);
        (await client.GetAsync(new Uri(hint))).StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}

public class McpSlashPathMetadataTests : McpRootMetadataTests
{
    protected override string Path => "/";
}

public class McpEmptyPathMetadataTests : McpRootMetadataTests
{
    protected override string Path => "";
}
