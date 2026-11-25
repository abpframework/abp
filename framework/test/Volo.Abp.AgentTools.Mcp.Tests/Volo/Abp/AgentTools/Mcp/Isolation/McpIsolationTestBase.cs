using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Protocol;
using Volo.Abp.AgentTools.Mcp.Sample;

namespace Volo.Abp.AgentTools.Mcp;

public enum McpCaller
{
    Anonymous,
    Cookie,
    TokenWithoutScope,
    TokenWithScope
}

/// <summary>Hosts with another module's MCP endpoints next to ours, and raw JSON-RPC requests to both.</summary>
public abstract class McpIsolationTestBase : AgentToolsMcpTestBase
{
    protected const string OurPath = "/mcp";
    protected const string OurServerName = "our-server";

    /// <summary>What <see cref="PermittedUser"/> may list.</summary>
    protected static readonly string[] OurTools = [McpSampleAppService.CreateNote, McpSampleAppService.GetNotes, McpSampleAppService.GetSecret];

    protected OtherMcpLog OtherLog => GetRequiredService<OtherMcpLog>();

    /// <summary>One of Program's startup modules; null = no other module.</summary>
    protected virtual Type? StartupModule => null;

    /// <summary>True makes the other module's endpoints stateless; false leaves the SDK's default (stateful).</summary>
    protected virtual bool OtherIsStateless => false;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        if (StartupModule != null)
        {
            builder.UseSetting(Program.StartupModuleSetting, StartupModule.Name);
        }
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpAgentToolsMcpOptions>(o => o.ServerName = OurServerName);
        if (OtherIsStateless)
        {
            services.Configure<HttpServerTransportOptions>(o => o.Stateless = true);
        }
    }

    protected Token PermittedUser() => NewUser(TenantA, McpSamplePermissions.Secret);

    protected async Task<HttpClient> CreateHttpClientAsync(McpCaller caller)
    {
        var token = PermittedUser();
        switch (caller)
        {
            case McpCaller.Cookie:
                var client = CreateHttpClient();
                client.DefaultRequestHeaders.Add("Cookie", await LoginWithCookieAsync(token.UserId!.Value, token.TenantId));
                return client;
            case McpCaller.TokenWithoutScope:
                return CreateHttpClient(token with { Scope = "openid" });
            case McpCaller.TokenWithScope:
                return CreateHttpClient(token);
            default:
                return CreateHttpClient();
        }
    }

    protected record RpcResponse(HttpStatusCode StatusCode, string Body, bool HasSession)
    {
        /// <summary>The JSON-RPC message: a JSON body, or the SSE stream's last event.</summary>
        public JsonElement Json => JsonSerializer.Deserialize<JsonElement>(
            Body.Split('\n').LastOrDefault(l => l.StartsWith("data: ", StringComparison.Ordinal))?["data: ".Length..] ?? Body);

        public JsonElement Result => Json.TryGetProperty("result", out var result) ? result : throw new InvalidOperationException(Body);

        public int? ErrorCode => Json.TryGetProperty("error", out var error) ? error.GetProperty("code").GetInt32() : null;

        public bool IsToolError => Result.TryGetProperty("isError", out var isError) && isError.GetBoolean();

        public string ToolText => Result.GetProperty("content")[0].GetProperty("text").GetString()!;

        public string[] Names(string collection)
        {
            return Result.GetProperty(collection).EnumerateArray()
                .Select(e => e.GetProperty("name").GetString()!).Order(StringComparer.Ordinal).ToArray();
        }
    }

    /// <summary>One request; the server is initialized first, for the session of a stateful one (a new session per request).</summary>
    protected static async Task<RpcResponse> RpcAsync(HttpClient client, string path, string method, object? @params = null)
    {
        string? session = null;
        if (method != RequestMethods.Initialize)
        {
            using var initialize = await PostInitializeAsync(client, path);
            session = initialize.Headers.TryGetValues("Mcp-Session-Id", out var values) ? values.Single() : null;
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, path)
        {
            Content = JsonContent.Create(new { jsonrpc = "2.0", id = 2, method, @params = @params ?? new { } })
        };
        request.Headers.Accept.ParseAdd("application/json");
        request.Headers.Accept.ParseAdd("text/event-stream");
        if (session != null)
        {
            request.Headers.Add("Mcp-Session-Id", session);
        }

        using var response = await client.SendAsync(request);
        return new RpcResponse(response.StatusCode, await response.Content.ReadAsStringAsync(), session != null);
    }

    protected static Task<RpcResponse> InitializeAsync(HttpClient client, string path)
    {
        return RpcAsync(client, path, RequestMethods.Initialize, new
        {
            protocolVersion = "2025-06-18",
            capabilities = new { },
            clientInfo = new { name = "raw", version = "1" }
        });
    }

    protected static Task<RpcResponse> CallAsync(HttpClient client, string path, string tool, object? arguments = null)
    {
        return RpcAsync(client, path, RequestMethods.ToolsCall, new { name = tool, arguments = arguments ?? new { } });
    }

    /// <summary>The protocol's requests to a server apart from initialize, ping, logging and tools.</summary>
    protected static readonly (string Method, object Params)[] OtherMethods =
    [
        (RequestMethods.PromptsList, new { }),
        (RequestMethods.PromptsGet, new { name = OtherMcp.Prompt, arguments = new { topic = "x" } }),
        (RequestMethods.ResourcesList, new { }),
        (RequestMethods.ResourcesTemplatesList, new { }),
        (RequestMethods.ResourcesRead, new { uri = OtherMcp.Resource }),
        (RequestMethods.ResourcesRead, new { uri = "other://items/7" }),
        (RequestMethods.ResourcesSubscribe, new { uri = OtherMcp.Resource }),
        (RequestMethods.ResourcesUnsubscribe, new { uri = OtherMcp.Resource }),
        (RequestMethods.CompletionComplete, new { @ref = new { type = "ref/prompt", name = OtherMcp.Prompt }, argument = new { name = "topic", value = "x" } }),
        ("tasks/list", new { }),
        ("tasks/get", new { taskId = "x" })
    ];
}
