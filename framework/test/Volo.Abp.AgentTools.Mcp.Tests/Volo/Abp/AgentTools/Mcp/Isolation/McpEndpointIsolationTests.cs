using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol;
using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Shouldly;
using Volo.Abp.AgentTools.Mcp.Sample;
using Xunit;

namespace Volo.Abp.AgentTools.Mcp;

/// <summary>An app whose only MCP use is ours.</summary>
public class McpEndpointIsolationTests : McpIsolationTestBase
{
    [Fact]
    public async Task Our_Endpoint_Needs_No_Mcp_Server_Of_The_App_And_Registers_None()
    {
        (await RpcAsync(CreateHttpClient(PermittedUser()), OurPath, RequestMethods.ToolsList)).Names("tools").ShouldBe(OurTools);

        GetRequiredService<IEnumerable<IConfigureOptions<McpServerOptions>>>().ShouldBeEmpty();
        GetRequiredService<IEnumerable<IPostConfigureOptions<McpServerOptions>>>().ShouldBeEmpty();
        GetRequiredService<IEnumerable<IConfigureOptions<HttpServerTransportOptions>>>().ShouldBeEmpty();
        GetRequiredService<IEnumerable<IHostedService>>()
            .ShouldNotContain(s => s.GetType().Assembly == typeof(HttpServerTransportOptions).Assembly);
    }

    [Fact]
    public async Task A_Server_The_App_Runs_From_Its_Own_Options_Without_An_Http_Context_Shows_Nothing_Of_Ours()
    {
        var options = GetRequiredService<IOptions<McpServerOptions>>().Value;
        options.ToolCollection.ShouldBeNull();
        options.Handlers.ListToolsHandler.ShouldBeNull();
        options.Handlers.CallToolHandler.ShouldBeNull();

        Pipe toServer = new(), toClient = new();
        using var stop = new CancellationTokenSource();
        await using var server = McpServer.Create(
            new StreamServerTransport(toServer.Reader.AsStream(), toClient.Writer.AsStream()), options, serviceProvider: ServiceProvider);
        var run = server.RunAsync(stop.Token);
        await using (var client = await McpClient.CreateAsync(new StreamClientTransport(toServer.Writer.AsStream(), toClient.Reader.AsStream())))
        {
            client.ServerCapabilities.Tools.ShouldBeNull();
            client.ServerInfo.Name.ShouldNotBe(OurServerName);
            (await Should.ThrowAsync<McpProtocolException>(async () => await client.ListToolsAsync())).ErrorCode.ShouldBe(McpErrorCode.MethodNotFound);
            await Should.ThrowAsync<McpProtocolException>(async () => await client.CallToolAsync(McpSampleAppService.GetNotes));
        }

        await stop.CancelAsync();
        await run.ContinueWith(_ => { }, TaskScheduler.Default);
        Observer.Records.ShouldBeEmpty();
    }

    [Fact]
    public void Our_Endpoint_Is_Once_In_The_Apps_Endpoints()
    {
        var endpoint = GetRequiredService<EndpointDataSource>().Endpoints.OfType<RouteEndpoint>()
            .Where(e => e.Metadata.Contains(AbpAgentToolsMcpOptions.EndpointMetadata)).ShouldHaveSingleItem();

        endpoint.RoutePattern.RawText.ShouldBe("/mcp/");
        endpoint.Metadata.GetMetadata<IHttpMethodMetadata>()!.HttpMethods.ShouldBe(["POST"]);
    }

    [Fact]
    public async Task An_Endpoint_Of_The_App_That_Executes_Ours_Does_Not_Skip_The_Guard()
    {
        var cookie = await CreateHttpClientAsync(McpCaller.Cookie);
        (await PostInitializeAsync(cookie, AliasStartupFilter.Alias)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var withoutScope = await CreateHttpClientAsync(McpCaller.TokenWithoutScope);
        (await PostInitializeAsync(withoutScope, AliasStartupFilter.Alias)).StatusCode.ShouldBe(HttpStatusCode.Forbidden);

        var withScope = await CreateHttpClientAsync(McpCaller.TokenWithScope);
        (await RpcAsync(withScope, AliasStartupFilter.Alias, RequestMethods.ToolsList)).Names("tools").ShouldBe(OurTools);
    }

    [Fact]
    public async Task The_Handlers_Run_In_The_Requests_Own_Scope()
    {
        await RpcAsync(CreateHttpClient(PermittedUser()), OurPath, RequestMethods.ToolsList);

        GetRequiredService<SpyLog>().RanInRequestScope.ShouldBe(true);
    }

    [Fact]
    public async Task An_Exception_In_A_Handler_Is_Logged_By_The_App_And_Its_Text_Is_Not_Sent()
    {
        var log = GetRequiredService<SpyLog>();
        log.Throw = true;

        var response = await RpcAsync(CreateHttpClient(PermittedUser()), OurPath, RequestMethods.ToolsList);

        response.ErrorCode.ShouldBe((int)McpErrorCode.InternalError);
        response.Body.ShouldNotContain("internal detail");
        log.Logged.ShouldContain(e => e.Message == "internal detail");
    }

    [Fact]
    public async Task An_Aborted_Request_Cancels_The_Handler()
    {
        var log = GetRequiredService<SpyLog>();
        log.Block = true;
        using var abort = new CancellationTokenSource();
        var request = new HttpRequestMessage(HttpMethod.Post, OurPath)
        {
            Content = new StringContent("""{"jsonrpc":"2.0","id":2,"method":"tools/list","params":{}}""", Encoding.UTF8, "application/json")
        };
        request.Headers.Accept.ParseAdd("application/json");
        request.Headers.Accept.ParseAdd("text/event-stream");

        var send = CreateHttpClient(PermittedUser()).SendAsync(request, abort.Token);
        await log.Started.Task.WaitAsync(TimeSpan.FromSeconds(30));
        await abort.CancelAsync();

        await log.Cancelled.Task.WaitAsync(TimeSpan.FromSeconds(30));
        await Should.ThrowAsync<OperationCanceledException>(() => send);
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddTransient<IStartupFilter, AliasStartupFilter>();
        services.AddSingleton<SpyLog>();
        services.AddSingleton<ILoggerProvider, SpyLoggerProvider>();
        services.AddScoped<SpyScope>();
        services.AddTransient<AgentToolProvider>();
        services.Replace(ServiceDescriptor.Transient<IAgentToolProvider, SpyToolProvider>());
    }
}

/// <summary>An app that gives our server no name: the SDK's default server info.</summary>
public class McpDefaultServerInfoTests : AgentToolsMcpTestBase
{
    [Fact]
    public async Task The_Default_Server_Info_Is_The_Entry_Assembly()
    {
        await using var client = await ConnectAsync(NewUser(TenantA));

        var assembly = Assembly.GetEntryAssembly()!.GetName();
        client.ServerInfo.Name.ShouldBe(assembly.Name);
        client.ServerInfo.Version.ShouldBe(assembly.Version!.ToString());
    }
}

public class McpServerVersionTests : AgentToolsMcpTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpAgentToolsMcpOptions>(o =>
        {
            o.ServerName = "ours";
            o.ServerVersion = "3.2";
        });
    }

    [Fact]
    public async Task The_Configured_Server_Name_And_Version_Are_Sent()
    {
        await using var client = await ConnectAsync(NewUser(TenantA));

        client.ServerInfo.Name.ShouldBe("ours");
        client.ServerInfo.Version.ShouldBe("3.2");
    }
}

/// <summary>An app with a plain MCP server (no tools, no handlers) on endpoints of its own.</summary>
public class McpPlainOtherServerTests : McpIsolationTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddMcpServer().WithHttpTransport();
        OtherMcp.Map(services);
    }

    [Theory]
    [InlineData(OtherMcp.Path, McpCaller.Anonymous)]
    [InlineData(OtherMcp.Path, McpCaller.Cookie)]
    [InlineData(OtherMcp.SecurePath, McpCaller.Cookie)]
    [InlineData(OtherMcp.SecurePath, McpCaller.TokenWithoutScope)]
    [InlineData(OtherMcp.SecurePath, McpCaller.TokenWithScope)]
    public async Task It_Has_No_Tools(string path, McpCaller caller)
    {
        var client = await CreateHttpClientAsync(caller);

        var server = (await InitializeAsync(client, path)).Result;
        server.GetProperty("capabilities").TryGetProperty("tools", out _).ShouldBeFalse();
        server.GetProperty("serverInfo").GetProperty("name").GetString().ShouldNotBe(OurServerName);
        (await RpcAsync(client, path, RequestMethods.ToolsList)).ErrorCode.ShouldBe((int)McpErrorCode.MethodNotFound);
        (await CallAsync(client, path, McpSampleAppService.GetNotes)).ErrorCode.ShouldNotBeNull();
        Observer.Records.ShouldBeEmpty();
    }
}

/// <summary>Another module configures the app's server and transport options as far as it can.</summary>
public class McpForeignOptionsTests : McpIsolationTestBase
{
    protected override Type StartupModule => typeof(AttributeToolsAfterModule);

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.Configure<HttpServerTransportOptions>(Override);
        services.ConfigureAll<HttpServerTransportOptions>(Override);
        services.PostConfigure<HttpServerTransportOptions>(Override);
        services.PostConfigureAll<HttpServerTransportOptions>(Override);

        services.PostConfigureAll<McpServerOptions>(options =>
        {
            options.ToolCollection ??= [];
            options.ToolCollection.TryAdd(McpServerTool.Create(() => "foreign", new McpServerToolCreateOptions { Name = "foreign_posted" }));
            options.ServerInstructions = "foreign instructions";
            options.ProtocolVersion = "2024-11-05";
            options.Capabilities = new ServerCapabilities { Experimental = new Dictionary<string, object> { ["foreign"] = new() } };
        });
        services.AddSingleton(McpServerTool.Create(() => "foreign", new McpServerToolCreateOptions { Name = "foreign_singleton" }));
    }

    private static void Override(HttpServerTransportOptions options)
    {
        options.Stateless = false;
        options.ConfigureSessionOptions = (http, server, _) =>
        {
            http.RequestServices.GetRequiredService<OtherMcpLog>().Entries.Enqueue("ConfigureSessionOptions");
            server.Handlers.ListToolsHandler = (_, _) =>
                new ValueTask<ListToolsResult>(new ListToolsResult { Tools = [new Tool { Name = "foreign_session" }] });
            return Task.CompletedTask;
        };
#pragma warning disable MCPEXP002 // RunSessionHandler is experimental.
        options.RunSessionHandler = (http, server, cancellationToken) =>
        {
            http.RequestServices.GetRequiredService<OtherMcpLog>().Entries.Enqueue("RunSessionHandler");
            return server.RunAsync(cancellationToken);
        };
#pragma warning restore MCPEXP002
    }

    [Fact]
    public async Task Our_Endpoint_Stays_Stateless_And_Serves_Only_Our_Tools()
    {
        var client = CreateHttpClient(PermittedUser());

        var initialize = await PostInitializeAsync(client, OurPath);
        initialize.StatusCode.ShouldBe(HttpStatusCode.OK);
        initialize.Headers.Contains("Mcp-Session-Id").ShouldBeFalse();
        var body = await initialize.Content.ReadAsStringAsync();
        body.ShouldNotContain("foreign");
        body.ShouldContain("2025-06-18");

        (await RpcAsync(client, OurPath, RequestMethods.ToolsList)).Names("tools").ShouldBe(OurTools);
        foreach (var foreign in new[] { "foreign_posted", "foreign_singleton", "foreign_session" })
        {
            (await CallAsync(client, OurPath, foreign)).ToolText.ShouldContain(AgentToolsErrorCodes.ToolNotFound);
        }

        OtherLog.Entries.ShouldBeEmpty();
    }

    [Fact]
    public async Task Sessions_Get_Delete_And_Sse_Do_Not_Exist_On_Our_Endpoint()
    {
        foreach (var client in new[] { CreateHttpClient(), CreateHttpClient(PermittedUser()) })
        {
            (await client.GetAsync("/mcp")).StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
            (await client.DeleteAsync("/mcp")).StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
            (await client.GetAsync("/mcp/sse")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
            (await client.PostAsync("/mcp/message", null)).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        OtherLog.Entries.ShouldBeEmpty();

        var withSession = CreateHttpClient(PermittedUser());
        var session = (await PostInitializeAsync(withSession, OtherMcp.Path)).Headers.GetValues("Mcp-Session-Id").Single();
        withSession.DefaultRequestHeaders.Add("Mcp-Session-Id", session);
        OtherLog.Entries.Clear();
        (await PostInitializeAsync(withSession, OurPath)).StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        OtherLog.Entries.ShouldBeEmpty();
    }

    [Fact]
    public async Task The_Other_Endpoint_Follows_Those_Options()
    {
        var list = await RpcAsync(CreateHttpClient(), OtherMcp.Path, RequestMethods.ToolsList);

        list.HasSession.ShouldBeTrue();
        list.Names("tools").ShouldContain("foreign_session");
        list.Names("tools").ShouldContain("foreign_posted");
        OtherLog.Entries.ShouldContain("ConfigureSessionOptions");
        OtherLog.Entries.ShouldContain("RunSessionHandler");
    }
}

/// <summary>Another module maps its server on our path: requests are ambiguous, neither server answers.</summary>
public class McpSamePathTests : McpIsolationTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddMcpServer().WithHttpTransport(o => o.Stateless = true).WithTools<OtherTools>();
        services.Configure<AbpEndpointRouterOptions>(options => options.EndpointConfigureActions.Add(context => context.Endpoints.MapMcp(OurPath)));
    }

    [Theory]
    [InlineData(McpCaller.Anonymous)]
    [InlineData(McpCaller.TokenWithScope)]
    public async Task Neither_Server_Answers(McpCaller caller)
    {
        var client = await CreateHttpClientAsync(caller);

        (await Should.ThrowAsync<Exception>(() => PostInitializeAsync(client, OurPath))).GetType().Name.ShouldBe("AmbiguousMatchException");
    }
}

/// <summary>A middleware of the app that executes our endpoint's delegate for <see cref="Alias"/>.</summary>
public class AliasStartupFilter : IStartupFilter
{
    public const string Alias = "/alias";

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => app =>
    {
        app.Use((http, following) => http.Request.Path != Alias
            ? following(http)
            : http.RequestServices.GetRequiredService<EndpointDataSource>().Endpoints
                .Single(e => e.Metadata.Contains(AbpAgentToolsMcpOptions.EndpointMetadata)).RequestDelegate!(http));
        next(app);
    };
}

public class SpyScope;

public class SpyLog
{
    public bool Throw { get; set; }

    public bool Block { get; set; }

    public bool? RanInRequestScope { get; set; }

    public TaskCompletionSource Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public TaskCompletionSource Cancelled { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public ConcurrentQueue<Exception> Logged { get; } = new();
}

/// <summary>The real provider behind what <see cref="SpyLog"/> asks for.</summary>
public class SpyToolProvider(AgentToolProvider inner, SpyScope scope, SpyLog log, IHttpContextAccessor http) : IAgentToolProvider
{
    public async Task<IReadOnlyList<AgentTool>> GetToolsAsync(AgentToolContext context, CancellationToken cancellationToken = default)
    {
        log.RanInRequestScope = ReferenceEquals(scope, http.HttpContext!.RequestServices.GetRequiredService<SpyScope>());
        if (log.Throw)
        {
            throw new InvalidOperationException("internal detail");
        }

        if (log.Block)
        {
            log.Started.TrySetResult();
            try
            {
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                log.Cancelled.TrySetResult();
                throw;
            }
        }

        return await inner.GetToolsAsync(context, cancellationToken);
    }
}

public sealed class SpyLoggerProvider(SpyLog log) : ILoggerProvider, ILogger
{
    public ILogger CreateLogger(string categoryName) => this;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (exception != null)
        {
            log.Logged.Enqueue(exception);
        }
    }

    public void Dispose()
    {
    }
}
