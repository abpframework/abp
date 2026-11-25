using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging.Abstractions;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using Volo.Abp.AgentTools.Mcp.Sample;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Volo.Abp.AgentTools.Mcp;

/// <summary>A new web host per test. <see cref="Token"/> describes a bearer token the test scheme turns into a principal.</summary>
public abstract class AgentToolsMcpTestBase : AbpWebApplicationFactoryIntegratedTest<Program>
{
    protected static readonly Guid TenantA = AgentToolsMcpTestModule.TenantA;
    protected static readonly Guid TenantB = AgentToolsMcpTestModule.TenantB;

    /// <summary>The required scope among others, space-separated as in a JWT.</summary>
    protected const string DefaultScope = "openid AgentTools offline_access";

    protected const string UserAgent = "test-agent/1.2";

    protected TestPermissionStore PermissionStore => GetRequiredService<TestPermissionStore>();

    protected TestFeatureStore FeatureStore => GetRequiredService<TestFeatureStore>();

    protected RecordingObserver Observer => GetRequiredService<RecordingObserver>();

    protected TestAuditingStore AuditingStore => GetRequiredService<TestAuditingStore>();

    protected AgentToolsMcpTestBase()
    {
        GetRequiredService<InMemoryAgentToolPolicyStore>().Set(McpSampleAppService.GetDisabled, null, false, null);
    }

    protected record Token(Guid? UserId, Guid? TenantId, string Scope = DefaultScope, string ClientId = "test-client");

    /// <summary>A token for a new user in the tenant (null = host) granted exactly the given permissions.</summary>
    protected Token NewUser(Guid? tenantId, params string[] permissions)
    {
        var userId = Guid.NewGuid();
        PermissionStore.GrantToUser(userId, permissions);
        return new Token(userId, tenantId);
    }

    /// <summary>An HttpClient on the test server without cookie handling or redirects; with the token's headers when given.</summary>
    protected HttpClient CreateHttpClient(Token? token = null)
    {
        var client = CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, HandleCookies = false });
        client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new("Bearer", "test");
            Set(client, TestBearerAuthenticationHandler.UserHeader, token.UserId?.ToString());
            Set(client, TestBearerAuthenticationHandler.TenantHeader, token.TenantId?.ToString());
            Set(client, TestBearerAuthenticationHandler.ScopeHeader, token.Scope);
            Set(client, TestBearerAuthenticationHandler.ClientHeader, token.ClientId);
        }

        return client;
    }

    /// <summary>An MCP client (C# SDK, Streamable HTTP) over the test server.</summary>
    protected async Task<McpClient> ConnectAsync(HttpClient http, string path = "/mcp")
    {
        var transport = new HttpClientTransport(
            new HttpClientTransportOptions { Endpoint = new Uri(http.BaseAddress!, path), TransportMode = HttpTransportMode.StreamableHttp },
            http, NullLoggerFactory.Instance, ownsHttpClient: true);
        return await McpClient.CreateAsync(transport, new McpClientOptions { ClientInfo = new Implementation { Name = "test-agent", Version = "1.2" } });
    }

    protected Task<McpClient> ConnectAsync(Token token) => ConnectAsync(CreateHttpClient(token));

    /// <summary>A raw MCP initialize POST, for status codes and headers.</summary>
    protected static Task<HttpResponseMessage> PostInitializeAsync(HttpClient client, string path = "/mcp")
    {
        var request = new HttpRequestMessage(HttpMethod.Post, path)
        {
            Content = JsonContent.Create(new
            {
                jsonrpc = "2.0",
                id = 1,
                method = "initialize",
                @params = new { protocolVersion = "2025-06-18", capabilities = new { }, clientInfo = new { name = "raw", version = "1" } }
            })
        };
        request.Headers.Accept.ParseAdd("application/json");
        request.Headers.Accept.ParseAdd("text/event-stream");
        return client.SendAsync(request);
    }

    protected static async Task<CallToolResult> CallAsync(McpClient client, string tool, object? arguments = null)
    {
        var args = arguments == null
            ? new Dictionary<string, object?>()
            : JsonSerializer.SerializeToElement(arguments).EnumerateObject().ToDictionary(p => p.Name, p => (object?)p.Value.Clone());
        return await client.CallToolAsync(tool, args);
    }

    protected static string Text(CallToolResult result) => result.Content.OfType<TextContentBlock>().Single().Text;

    /// <summary>Signs in with the app's cookie scheme; returns the "name=value" to send as a Cookie header.</summary>
    protected async Task<string> LoginWithCookieAsync(Guid userId, Guid? tenantId)
    {
        var response = await CreateHttpClient().GetAsync($"/test/cookie-login?userId={userId}&tenantId={tenantId}");
        response.EnsureSuccessStatusCode();
        return response.Headers.GetValues("Set-Cookie").Single().Split(';')[0];
    }

    protected async Task SeedNotesAsync(Guid? tenantId, params string[] texts)
    {
        using (GetRequiredService<ICurrentTenant>().Change(tenantId))
        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin(requiresNew: true))
        {
            foreach (var text in texts)
            {
                await GetRequiredService<IRepository<Note, Guid>>().InsertAsync(new Note { TenantId = tenantId, Text = text });
            }

            await uow.CompleteAsync();
        }
    }

    private static void Set(HttpClient client, string header, string? value)
    {
        if (value != null)
        {
            client.DefaultRequestHeaders.Add(header, value);
        }
    }
}
