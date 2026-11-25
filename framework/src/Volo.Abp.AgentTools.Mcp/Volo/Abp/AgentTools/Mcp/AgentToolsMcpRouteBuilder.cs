using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;

namespace Volo.Abp.AgentTools.Mcp;

/// <summary>
/// Maps into the app's routes with an MCP server of our own. The MCP SDK keeps its server options (tools, handlers, filters) and
/// its transport options (stateless) once per service container and serves them on every <c>MapMcp</c> endpoint. So ours come
/// from a small container of their own: no other endpoint of the app serves our tools, and nothing the app or another module
/// registers is served by, or changes, our endpoint.
/// </summary>
internal sealed class AgentToolsMcpRouteBuilder : IEndpointRouteBuilder
{
    private readonly IEndpointRouteBuilder _app;

    /// <summary>The MCP SDK's services only. The handlers get the request's own services, as always in stateless mode.</summary>
    public IServiceProvider ServiceProvider { get; }

    public ICollection<EndpointDataSource> DataSources => _app.DataSources;

    public AgentToolsMcpRouteBuilder(IEndpointRouteBuilder app, AbpAgentToolsMcpOptions options)
    {
        _app = app;

        var lifetime = app.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
        var services = new ServiceCollection();
        services.AddSingleton(lifetime);
        services.AddSingleton(app.ServiceProvider.GetRequiredService<ILoggerFactory>());
        services.AddLogging();
        services
            .AddMcpServer(server =>
            {
                if (options.ServerName != null)
                {
                    server.ServerInfo = new Implementation { Name = options.ServerName, Version = options.ServerVersion ?? "1.0.0" };
                }
            })
            // Its hosted service is not started: it only prunes the sessions of a stateful server.
            .WithHttpTransport(transport => transport.Stateless = true)
            .WithListToolsHandler(AgentToolsMcpEndpoint.ListToolsAsync)
            .WithCallToolHandler(AgentToolsMcpEndpoint.CallToolAsync);

        var provider = services.BuildServiceProvider();
        lifetime.ApplicationStopped.Register(provider.Dispose);
        ServiceProvider = provider;
    }

    public IApplicationBuilder CreateApplicationBuilder() => _app.CreateApplicationBuilder();
}
