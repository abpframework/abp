using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Data;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// A new host per test. Derived classes pick the host's configuration (<see cref="Configuration"/>) and options
/// (<see cref="ConfigureOptions"/>); both are read while the base constructor builds the host, so they may only use field
/// initializers. Every log entry of the host, from startup on, is in <see cref="Logs"/>.
/// </summary>
public abstract class AgentToolsOpenIddictTestBase : AbpWebApplicationFactoryIntegratedTest<Program>
{
    public const string SelfUrl = "https://app.test";
    public const string McpUrl = SelfUrl + "/mcp";

    protected CapturingLoggerProvider Logs { get; } = new();

    /// <summary>The host's configuration. Default: App:SelfUrl only.</summary>
    protected virtual Dictionary<string, string?> Configuration => new() { ["App:SelfUrl"] = SelfUrl };

    /// <summary>False (default): the host accepts the audiences "TestApp" and the MCP URL. True: an app that calls no AddAudiences.</summary>
    protected virtual bool AcceptsAnyAudience => false;

    protected virtual void ConfigureOptions(AbpAgentToolsOpenIddictOptions options)
    {
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(Configuration));
        if (AcceptsAnyAudience)
        {
            builder.UseSetting(AgentToolsOpenIddictTestModule.AcceptAnyAudienceSetting, "true");
        }
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ILoggerProvider>(Logs);
        services.Configure<AbpAgentToolsOpenIddictOptions>(ConfigureOptions);
    }

    protected Task SeedAsync<TContributor>() where TContributor : IDataSeedContributor =>
        WithUnitOfWorkAsync(sp => sp.GetRequiredService<TContributor>().SeedAsync(new DataSeedContext()));

    protected async Task WithUnitOfWorkAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = Services.CreateScope();
        using var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin(requiresNew: true);
        await action(scope.ServiceProvider);
        await uow.CompleteAsync();
    }

    protected async Task<T> WithUnitOfWorkAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        var result = default(T)!;
        await WithUnitOfWorkAsync(async sp => { result = await action(sp); });
        return result;
    }

    protected Task CreateScopeAsync(string name, params string[] resources) => WithUnitOfWorkAsync(async sp =>
    {
        var descriptor = new OpenIddictScopeDescriptor { Name = name };
        descriptor.Resources.UnionWith(resources);
        await sp.GetRequiredService<IOpenIddictScopeManager>().CreateAsync(descriptor);
    });

    /// <summary>
    /// A client-credentials client allowed to request <paramref name="scope"/> (space-separated scopes); the secret exists only in
    /// this process.
    /// </summary>
    protected async Task<string> CreateClientCredentialsClientAsync(string clientId, string scope)
    {
        var secret = Guid.NewGuid().ToString();
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            DisplayName = clientId, // ABP's client credentials handler reads it
            ClientSecret = secret,
            ClientType = OpenIddictConstants.ClientTypes.Confidential,
            Permissions = { OpenIddictConstants.Permissions.Endpoints.Token, OpenIddictConstants.Permissions.GrantTypes.ClientCredentials }
        };
        descriptor.Permissions.UnionWith(scope.Split(' ').Select(s => OpenIddictConstants.Permissions.Prefixes.Scope + s));
        await WithUnitOfWorkAsync(sp => sp.GetRequiredService<IOpenIddictApplicationManager>().CreateAsync(descriptor).AsTask());
        return secret;
    }

    protected async Task<string> GetClientCredentialsTokenAsync(string clientId, string secret, string scope)
    {
        var body = await RequestTokenAsync(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = clientId,
            ["client_secret"] = secret,
            ["scope"] = scope
        });
        return body.GetProperty("access_token").GetString()!;
    }

    protected async Task<JsonElement> RequestTokenAsync(Dictionary<string, string> parameters)
    {
        var response = await Client.PostAsync("/connect/token", new FormUrlEncodedContent(parameters));
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        response.IsSuccessStatusCode.ShouldBeTrue(body.ToString());
        return body;
    }

    protected Task<HttpResponseMessage> GetWithBearerAsync(string path, string token, string? forwardedPrefix = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        request.Headers.Authorization = new("Bearer", token);
        if (forwardedPrefix != null)
        {
            request.Headers.Add("X-Forwarded-Prefix", forwardedPrefix);
        }

        return Client.SendAsync(request);
    }

    /// <summary>An MCP initialize POST to the real endpoint (AbpAgentToolsMcpModule) with the token.</summary>
    protected Task<HttpResponseMessage> PostMcpAsync(string token, string path = "/mcp")
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
        request.Headers.Authorization = new("Bearer", token);
        return Client.SendAsync(request);
    }

    /// <summary>A client credentials token that passes authentication and the scope check of the MCP endpoint's guard ends here.</summary>
    protected async Task ShouldBeRefusedForHavingNoUserAsync(string token, string path = "/mcp")
    {
        var response = await PostMcpAsync(token, path);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        (await response.Content.ReadAsStringAsync()).ShouldContain("client credentials");
    }

    protected List<string> WarningsOf<TCategory>() => MessagesOf<TCategory>(LogLevel.Warning);

    protected List<string> MessagesOf<TCategory>(LogLevel level) => Logs.Entries
        .Where(e => e.Category == typeof(TCategory).FullName && e.Level == level)
        .Select(e => e.Message)
        .ToList();
}

/// <summary>Keeps every log entry: category, level, formatted message and the structured values (exception included).</summary>
public class CapturingLoggerProvider : ILoggerProvider
{
    public record Entry(string Category, LogLevel Level, string Message, string Values);

    private readonly ConcurrentQueue<Entry> _entries = new();

    public IReadOnlyList<Entry> Entries => _entries.ToList();

    public void Clear() => _entries.Clear();

    public ILogger CreateLogger(string categoryName) => new Logger(categoryName, _entries);

    public void Dispose()
    {
    }

    private sealed class Logger(string category, ConcurrentQueue<Entry> entries) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var values = state is IEnumerable<KeyValuePair<string, object?>> pairs
                ? string.Join("|", pairs.Select(p => $"{p.Key}={p.Value}"))
                : state?.ToString() ?? "";
            entries.Enqueue(new Entry(category, logLevel, formatter(state, exception), values + "|" + exception));
        }
    }
}
