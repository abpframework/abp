using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.AgentTools.Mcp;

/// <summary>
/// Maps the stateless Streamable HTTP MCP endpoint (<see cref="AbpAgentToolsMcpOptions.Path"/>) through ABP's
/// <c>UseConfiguredEndpoints</c>; nothing to add to the app's pipeline. Stateless: no tools/list_changed notifications,
/// clients list the tools again themselves. The endpoint has an MCP server of its own: an <c>AddMcpServer</c> of the app or another
/// module, its options, tools and filters and its <c>MapMcp</c> endpoints share nothing with it.
/// </summary>
[DependsOn(typeof(AbpAgentToolsAspNetCoreModule))]
public class AbpAgentToolsMcpModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpEndpointRouterOptions>(options => options.EndpointConfigureActions.Add(endpointContext =>
            AgentToolsMcpEndpoint.Map(endpointContext.Endpoints)));
    }

    /// <summary>Fail closed: the endpoint's scheme must exist and must not be a cookie or policy scheme.</summary>
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var name = context.ServiceProvider.GetRequiredService<IOptions<AbpAgentToolsMcpOptions>>().Value.AuthenticationScheme;
        var schemes = context.ServiceProvider.GetService<IAuthenticationSchemeProvider>();
        var handler = schemes == null ? null : AsyncHelper.RunSync(() => schemes.GetSchemeAsync(name))?.HandlerType;
        if (handler == null || typeof(CookieAuthenticationHandler).IsAssignableFrom(handler) || typeof(PolicySchemeHandler).IsAssignableFrom(handler))
        {
            throw new AbpException(
                $"AbpAgentToolsMcpOptions.AuthenticationScheme is \"{name}\", which is {(handler == null ? "not a registered authentication scheme" : $"handled by {handler.Name}")}. " +
                "The MCP endpoint needs a bearer scheme (OpenIddict validation or JwtBearer); a cookie or policy scheme could let a browser session in.");
        }
    }
}
