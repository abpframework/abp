using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol;
using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Volo.Abp.AgentTools.Mcp.Sample;
using Volo.Abp.AspNetCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.AgentTools.Mcp;

/// <summary>
/// Another module of the app with MCP endpoints of its own: <see cref="Path"/> (open) and <see cref="SecurePath"/> (a signed-in
/// user, by cookie or token). Both forms serve <see cref="ToolNames"/>, one prompt, one resource, one template and completions.
/// </summary>
public static class OtherMcp
{
    public const string Path = "/other-mcp";
    public const string SecurePath = "/other-mcp-secure";
    public const string ServerName = "other-server";
    public const string Echo = "other_echo";
    public const string Prompt = "other_prompt";
    public const string Resource = "other://resource";
    public const string Template = "other://items/{id}";

    /// <summary>The first one has the name of one of our tools.</summary>
    public static readonly string[] ToolNames = [McpSampleAppService.GetSecret, Echo];

    public static void Map(IServiceCollection services)
    {
        services.Configure<AbpEndpointRouterOptions>(options => options.EndpointConfigureActions.Add(context =>
        {
            context.Endpoints.MapMcp(Path);
            context.Endpoints.MapMcp(SecurePath).RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, TestBearerAuthenticationHandler.Scheme)
                .RequireAuthenticatedUser());
        }));
    }

    /// <summary>What every form registers: server info, filters, a task store and a session migration handler.</summary>
    public static IMcpServerBuilder AddServer(IServiceCollection services)
    {
#pragma warning disable MCPEXP001 // Tasks are experimental.
        services.AddSingleton<IMcpTaskStore>(new InMemoryMcpTaskStore());
#pragma warning restore MCPEXP001
        services.AddSingleton<ISessionMigrationHandler, OtherSessionMigrationHandler>();

        return services
            .AddMcpServer(options => options.ServerInfo = new Implementation { Name = ServerName, Version = "9.9" })
            .WithHttpTransport()
            .WithRequestFilters(filters => filters
                .AddListToolsFilter(next => (context, cancellationToken) =>
                {
                    Log(context.Services, "filter tools/list");
                    return next(context, cancellationToken);
                })
                .AddCallToolFilter(next => (context, cancellationToken) =>
                {
                    Log(context.Services, "filter tools/call");
                    return next(context, cancellationToken);
                }))
            .WithMessageFilters(filters => filters.AddIncomingFilter(next => (context, cancellationToken) =>
            {
                Log(context.Services, "filter message");
                return next(context, cancellationToken);
            }));
    }

    private static void Log(IServiceProvider? services, string entry) => services!.GetRequiredService<OtherMcpLog>().Entries.Enqueue(entry);
}

/// <summary>What the other module's filters and delegates saw.</summary>
public class OtherMcpLog : ISingletonDependency
{
    public ConcurrentQueue<string> Entries { get; } = new();
}

public class OtherSessionMigrationHandler(OtherMcpLog log) : ISessionMigrationHandler
{
    public ValueTask OnSessionInitializedAsync(HttpContext context, string sessionId, InitializeRequestParams initializeParams, CancellationToken cancellationToken)
    {
        return default;
    }

    public ValueTask<InitializeRequestParams?> AllowSessionMigrationAsync(HttpContext context, string sessionId, CancellationToken cancellationToken)
    {
        log.Entries.Enqueue("session migration");
        return default;
    }
}

[McpServerToolType]
public class OtherTools
{
    [McpServerTool(Name = OtherMcp.Echo), Description("The other module's tool.")]
    public static string Echo(string text) => "other: " + text;

    [McpServerTool(Name = McpSampleAppService.GetSecret), Description("The other module's tool with the name of one of ours.")]
    public static string SameNameAsOurs() => "other: not the secret";
}

/// <summary>Not a tool type: only the tests of [Authorize] register it.</summary>
public class OtherAuthorizedTools
{
    public const string Name = "other_authorized";

    [McpServerTool(Name = Name), Authorize]
    public static string Authorized() => "other: authorized";
}

[McpServerPromptType]
public class OtherPrompts
{
    [McpServerPrompt(Name = OtherMcp.Prompt)]
    public static string Prompt(string topic) => "other: " + topic;
}

[McpServerResourceType]
public class OtherResources
{
    [McpServerResource(UriTemplate = OtherMcp.Resource, Name = "other_resource")]
    public static string Resource() => "other: resource";

    [McpServerResource(UriTemplate = OtherMcp.Template, Name = "other_item")]
    public static string Item(string id) => "other: item " + id;
}

[DependsOn(typeof(AbpAspNetCoreModule))]
public class OtherAttributeToolsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        OtherMcp.AddServer(context.Services)
            .WithToolsFromAssembly(typeof(OtherTools).Assembly)
            .WithPromptsFromAssembly(typeof(OtherTools).Assembly)
            .WithResourcesFromAssembly(typeof(OtherTools).Assembly)
            .WithCompleteHandler(OtherHandlers.CompleteAsync);
        OtherMcp.Map(context.Services);
    }
}

[DependsOn(typeof(AbpAspNetCoreModule))]
public class OtherOwnHandlersModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        OtherMcp.AddServer(context.Services)
            .WithListToolsHandler((_, _) => new ValueTask<ListToolsResult>(new ListToolsResult
            {
                Tools = OtherMcp.ToolNames.Select(name => new Tool { Name = name }).ToList()
            }))
            .WithCallToolHandler((request, _) => new ValueTask<CallToolResult>(OtherHandlers.Call(request.Params!)))
            .WithListPromptsHandler((_, _) => new ValueTask<ListPromptsResult>(new ListPromptsResult { Prompts = [new Prompt { Name = OtherMcp.Prompt }] }))
            .WithGetPromptHandler((request, _) => new ValueTask<GetPromptResult>(new GetPromptResult
            {
                Messages = [new PromptMessage { Role = Role.User, Content = new TextContentBlock { Text = "other: " + request.Params!.Arguments!["topic"] } }]
            }))
            .WithListResourcesHandler((_, _) => new ValueTask<ListResourcesResult>(new ListResourcesResult
            {
                Resources = [new Resource { Uri = OtherMcp.Resource, Name = "other_resource" }]
            }))
            .WithListResourceTemplatesHandler((_, _) => new ValueTask<ListResourceTemplatesResult>(new ListResourceTemplatesResult
            {
                ResourceTemplates = [new ResourceTemplate { UriTemplate = OtherMcp.Template, Name = "other_item" }]
            }))
            .WithReadResourceHandler((request, _) => new ValueTask<ReadResourceResult>(new ReadResourceResult
            {
                Contents = [new TextResourceContents { Uri = request.Params!.Uri, Text = "other: " + request.Params.Uri }]
            }))
            .WithSubscribeToResourcesHandler((_, _) => new ValueTask<EmptyResult>(new EmptyResult()))
            .WithUnsubscribeFromResourcesHandler((_, _) => new ValueTask<EmptyResult>(new EmptyResult()))
            .WithSetLoggingLevelHandler((_, _) => new ValueTask<EmptyResult>(new EmptyResult()))
            .WithCompleteHandler(OtherHandlers.CompleteAsync);
        OtherMcp.Map(context.Services);
    }
}

public static class OtherHandlers
{
    public static ValueTask<CompleteResult> CompleteAsync(RequestContext<CompleteRequestParams> request, CancellationToken cancellationToken)
    {
        return new ValueTask<CompleteResult>(new CompleteResult { Completion = new Completion { Values = ["other: completion"] } });
    }

    public static CallToolResult Call(CallToolRequestParams call)
    {
        var text = call.Name switch
        {
            OtherMcp.Echo => OtherTools.Echo(call.Arguments!["text"].GetString()!),
            McpSampleAppService.GetSecret => OtherTools.SameNameAsOurs(),
            _ => null
        };
        return new CallToolResult { IsError = text == null, Content = [new TextContentBlock { Text = text ?? "other: unknown tool" }] };
    }
}

// The order in DependsOn decides whose services are registered first.

[DependsOn(typeof(OtherAttributeToolsModule), typeof(AgentToolsMcpTestModule))]
public class AttributeToolsBeforeModule : AbpModule;

[DependsOn(typeof(AgentToolsMcpTestModule), typeof(OtherAttributeToolsModule))]
public class AttributeToolsAfterModule : AbpModule;

[DependsOn(typeof(OtherOwnHandlersModule), typeof(AgentToolsMcpTestModule))]
public class OwnHandlersBeforeModule : AbpModule;

[DependsOn(typeof(AgentToolsMcpTestModule), typeof(OtherOwnHandlersModule))]
public class OwnHandlersAfterModule : AbpModule;
