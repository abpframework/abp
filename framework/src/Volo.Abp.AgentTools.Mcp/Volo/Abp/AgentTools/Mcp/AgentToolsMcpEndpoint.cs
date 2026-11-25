using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Auditing;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace Volo.Abp.AgentTools.Mcp;

internal static class AgentToolsMcpEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        var options = endpoints.ServiceProvider.GetRequiredService<IOptions<AbpAgentToolsMcpOptions>>().Value;
        // RFC 9728 protected resource metadata, path-specific (the bare path is the one of a resource at the root of the app); anonymous,
        // no tenant, outside the MCP guard.
        endpoints.MapGet(options.ResourceMetadataPath, WriteMetadata).AllowAnonymous();

        new AgentToolsMcpRouteBuilder(endpoints, options).MapMcp(options.Path)
            // The guard below authenticates; the app's authorization (a fallback policy, a cookie challenge's 302) must not
            // answer first.
            .AllowAnonymous()
            .WithMetadata(AbpAgentToolsMcpOptions.EndpointMetadata)
            .Add(endpoint =>
            {
                var inner = endpoint.RequestDelegate!;
                endpoint.RequestDelegate = http => GuardAsync(http, inner);
            });
    }

    private static IResult WriteMetadata(HttpContext http)
    {
        var options = http.RequestServices.GetRequiredService<IOptions<AbpAgentToolsMcpOptions>>().Value;
        var document = new Dictionary<string, object?>
        {
            ["resource"] = options.GetResourceUrl(http.Request),
            ["authorization_servers"] = new[] { options.GetAuthorizationServer(http.Request).ToString() },
            ["scopes_supported"] = new[] { options.RequiredScope, "offline_access" },
            ["bearer_methods_supported"] = new[] { "header" }
        };
        if (options.ServerName != null)
        {
            document["resource_name"] = options.ServerName;
        }

        return Results.Json(document);
    }

    /// <summary>
    /// Bearer only, the required scope, a user, the token's tenant: then the request runs as the token's user in the token's
    /// tenant, whatever the default scheme (e.g. a cookie) and ABP's tenant middleware made of it before.
    /// </summary>
    private static async Task GuardAsync(HttpContext http, RequestDelegate next)
    {
        var services = http.RequestServices;
        var options = services.GetRequiredService<IOptions<AbpAgentToolsMcpOptions>>().Value;
        var metadata = $"resource_metadata=\"{options.GetResourceMetadataUrl(http.Request)}\"";

        // Header only (no token in a URL or form body, where it would end up in logs); a cookie never counts.
        var hasBearerHeader = http.Request.Headers.Authorization.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase);
        var principal = hasBearerHeader ? (await http.AuthenticateAsync(options.AuthenticationScheme)).Principal : null;
        if (principal?.Identity?.IsAuthenticated != true)
        {
            await RefuseAsync(http, StatusCodes.Status401Unauthorized,
                hasBearerHeader ? $"Bearer error=\"invalid_token\", {metadata}" : $"Bearer {metadata}",
                "invalid_token", "A valid bearer token in the Authorization header is required.");
            return;
        }

        // "scope" as in the JWT (space-separated), "oi_scp" as OpenIddict's validation maps it (one claim per scope).
        if (!principal.FindAll(c => c.Type is "scope" or "oi_scp")
                .SelectMany(c => c.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Contains(options.RequiredScope))
        {
            await RefuseAsync(http, StatusCodes.Status403Forbidden,
                $"Bearer error=\"insufficient_scope\", scope=\"{options.RequiredScope}\", {metadata}",
                "insufficient_scope", $"The token does not carry the '{options.RequiredScope}' scope that this endpoint requires.");
            return;
        }

        if (principal.FindUserId() == null)
        {
            await RefuseAsync(http, StatusCodes.Status403Forbidden, null,
                "access_denied", "The endpoint runs tools as a user; a token without a user (e.g. client credentials) is not accepted.");
            return;
        }

        var tokenTenantId = principal.FindTenantId();
        var tenantStore = services.GetRequiredService<ITenantStore>();
        var tenant = tokenTenantId == null ? null : await tenantStore.FindAsync(tokenTenantId.Value);
        if (tokenTenantId != null && tenant?.IsActive != true)
        {
            await RefuseAsync(http, StatusCodes.Status403Forbidden, null, "access_denied", "The token's tenant does not exist or is not active.");
            return;
        }

        if (!await RequestTenantMatchesAsync(services, tenantStore, tokenTenantId))
        {
            await RefuseAsync(http, StatusCodes.Status403Forbidden, null, "access_denied",
                "The request names a tenant (header, query string, cookie, route or domain) other than the token's tenant.");
            return;
        }

        var principalAccessor = services.GetRequiredService<ICurrentPrincipalAccessor>();
        using (services.GetRequiredService<ICurrentTenant>().Change(tokenTenantId, tenant?.Name))
        {
            // As AbpDynamicClaimsMiddleware does for the default scheme: fresh claims, and a revoked session ends here.
            if (services.GetRequiredService<IOptions<AbpClaimsPrincipalFactoryOptions>>().Value.IsDynamicClaimsEnabled)
            {
                using (principalAccessor.Change(principal))
                {
                    principal = await services.GetRequiredService<IAbpClaimsPrincipalFactory>().CreateDynamicAsync(principal);
                }

                if (principal.Identity?.IsAuthenticated != true)
                {
                    await RefuseAsync(http, StatusCodes.Status401Unauthorized, $"Bearer error=\"invalid_token\", {metadata}",
                        "invalid_token", "The token's session is no longer valid.");
                    return;
                }
            }

            using (principalAccessor.Change(principal))
            {
                http.User = principal;
                SetAuditLogCaller(services, principal, tokenTenantId, tenant?.Name);
                await next(http);
            }
        }
    }

    /// <summary>
    /// The tenant ABP's resolvers find in the request itself (the current user is left out, it is the token's): none, or the
    /// token's own. A fallback tenant is not named by the request and is ignored.
    /// </summary>
    private static async Task<bool> RequestTenantMatchesAsync(IServiceProvider services, ITenantStore tenantStore, Guid? tokenTenantId)
    {
        TenantResolveResult result;
        using (services.GetRequiredService<ICurrentPrincipalAccessor>().Change(new ClaimsPrincipal(new ClaimsIdentity())))
        {
            result = await services.GetRequiredService<ITenantResolver>().ResolveTenantIdOrNameAsync();
        }

        if (result.TenantIdOrName == null || result.AppliedResolvers.Contains(TenantResolverNames.FallbackTenant))
        {
            return true;
        }

        var named = Guid.TryParse(result.TenantIdOrName, out var id)
            ? await tenantStore.FindAsync(id)
            : await tenantStore.FindAsync(services.GetRequiredService<ITenantNormalizer>().NormalizeName(result.TenantIdOrName)!);
        return named != null && named.Id == tokenTenantId;
    }

    /// <summary>The request's audit log was started (UseAuditing) before the guard ran, with the caller before the switch.</summary>
    private static void SetAuditLogCaller(IServiceProvider services, ClaimsPrincipal principal, Guid? tenantId, string? tenantName)
    {
        var log = services.GetRequiredService<IAuditingManager>().Current?.Log;
        if (log == null)
        {
            return;
        }

        log.TenantId = tenantId;
        log.TenantName = tenantName;
        log.UserId = principal.FindUserId();
        log.UserName = principal.FindFirst(AbpClaimTypes.UserName)?.Value;
        log.ClientId = principal.FindClientId();
        log.ImpersonatorUserId = principal.FindImpersonatorUserId();
        log.ImpersonatorTenantId = principal.FindImpersonatorTenantId();
    }

    private static Task RefuseAsync(HttpContext http, int statusCode, string? wwwAuthenticate, string error, string description)
    {
        http.Response.StatusCode = statusCode;
        if (wwwAuthenticate != null)
        {
            http.Response.Headers.WWWAuthenticate = wwwAuthenticate;
        }

        return http.Response.WriteAsJsonAsync(new { error, error_description = description });
    }

    /// <summary>The provider's tools for the current user without approval support: approval-required tools are absent.</summary>
    public static async ValueTask<ListToolsResult> ListToolsAsync(RequestContext<ListToolsRequestParams> request, CancellationToken cancellationToken)
    {
        var services = request.Services!;
        var options = services.GetRequiredService<IOptions<AbpAgentToolsMcpOptions>>().Value;
        var tools = await services.GetRequiredService<IAgentToolProvider>().GetToolsAsync(CreateContext(request), cancellationToken);

        return new ListToolsResult
        {
            Tools = tools
                // Defence in depth: the provider already drops them (SupportsApproval = false).
                .Where(t => !t.RequiresApproval && options.IsExposed(t.Definition))
                .Select(t => new Tool
                {
                    Name = t.Definition.Name,
                    Description = t.Definition.Description,
                    InputSchema = t.Function.JsonSchema,
                    Annotations = new ToolAnnotations
                    {
                        ReadOnlyHint = t.Definition.ReadOnly,
                        DestructiveHint = t.Definition.Destructive,
                        IdempotentHint = t.Definition.Idempotent
                    }
                })
                .ToList()
        };
    }

    /// <summary>
    /// Through the invoker, which refuses unknown, disabled, approval-required and feature-off tools by name and lets ABP refuse
    /// a denied one. Every refusal and failure is a tool error result with the invoker's "ERROR &lt;type&gt;: &lt;message&gt;" text,
    /// so the model reads why and the connection goes on.
    /// </summary>
    public static async ValueTask<CallToolResult> CallToolAsync(RequestContext<CallToolRequestParams> request, CancellationToken cancellationToken)
    {
        var services = request.Services!;
        var name = request.Params?.Name ?? "";
        var definition = services.GetRequiredService<IAgentToolCatalog>().Find(name);

        string result;
        if (definition != null && !services.GetRequiredService<IOptions<AbpAgentToolsMcpOptions>>().Value.IsExposed(definition))
        {
            // Not offered over MCP: answered exactly like an unknown tool (the invoker checks the feature first, then the name),
            // and recorded like one (log, audit comment, observers), since the invoker is skipped.
            var enabled = await services.GetRequiredService<IFeatureChecker>().IsEnabledAsync(AgentToolsFeatures.Enable);
            var (code, message) = enabled
                ? (AgentToolsErrorCodes.ToolNotFound, $"There is no agent tool named '{name}'.")
                : (AgentToolsErrorCodes.FeatureDisabled, "Agent tools are not enabled for the current tenant.");
            result = await RefuseHiddenAsync(services, code, message, name, CreateContext(request));
        }
        else
        {
            var arguments = request.Params?.Arguments?.ToDictionary(a => a.Key, a => (object?)a.Value) ?? [];
            result = await services.GetRequiredService<IAgentToolInvoker>().InvokeAsync(name, arguments, CreateContext(request), cancellationToken);
        }

        // A successful result is JSON, which never starts with "ERROR ".
        return new CallToolResult
        {
            IsError = result.StartsWith("ERROR ", StringComparison.Ordinal),
            Content = [new TextContentBlock { Text = result }]
        };
    }

    /// <summary>
    /// The invoker's unknown-tool refusal for a hidden tool: its "ERROR" text (ABP's localized message, then the code), the log
    /// entry, the audit comment and the observer record. Arguments are not recorded (the invoker's redaction is not reusable).
    /// </summary>
    private static async Task<string> RefuseHiddenAsync(IServiceProvider services, string code, string message, string name, AgentToolContext context)
    {
        var startTime = services.GetRequiredService<IClock>().Now;
        var error = new BusinessException(code, message).WithData("ToolName", name);
        var text = services.GetRequiredService<IExceptionToErrorInfoConverter>().Convert(error, o => o.SendStackTraceToClients = false).Message
                   + $" (code: {code})";
        var result = $"ERROR {nameof(BusinessException)}: {text}";

        services.GetRequiredService<IAuditingManager>().Current?.Log.Comments.Add(
            $"Agent tool {name} (client: {context.ClientId ?? "none"}{(string.IsNullOrWhiteSpace(context.ClientName) ? "" : "; " + context.ClientName.Truncate(100))})");

        var logger = services.GetRequiredService<ILogger<AgentToolInvoker>>();
        logger.Log(error.LogLevel, error, "Agent tool {ToolName} failed in {Duration} ms (client: {ClientId}): {Result}. Arguments: {Arguments}",
            name, 0, context.ClientId, result, "{}");

        var currentUser = services.GetRequiredService<ICurrentUser>();
        var record = new AgentToolCallRecord
        {
            ToolName = name,
            Arguments = "{}",
            Succeeded = false,
            ErrorType = error.GetType().FullName,
            ErrorCode = code,
            ErrorMessage = text,
            StartTime = startTime,
            Duration = TimeSpan.Zero,
            UserId = currentUser.Id,
            UserName = currentUser.UserName,
            TenantId = services.GetRequiredService<ICurrentTenant>().Id,
            ClientId = context.ClientId,
            ClientName = context.ClientName
        };
        foreach (var observer in services.GetServices<IAgentToolCallObserver>())
        {
            try
            {
                await observer.OnCallAsync(record);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Agent tool call observer {Observer} failed for {ToolName}.", observer.GetType().FullName, name);
            }
        }

        return result;
    }

    /// <summary>External agent: no approval support, the OAuth client from the token, the self-reported MCP client.</summary>
    private static AgentToolContext CreateContext<TParams>(RequestContext<TParams> request)
    {
        var services = request.Services!;
        var principal = services.GetRequiredService<ICurrentPrincipalAccessor>().Principal;
        var info = request.Server.ClientInfo;
        // clientInfo is gone after initialize in stateless mode (the endpoint's only mode); then the User-Agent says who it is.
        var client = info != null
            ? $"{info.Name} {info.Version}"
            : services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Request.Headers.UserAgent.ToString();
        // Caller-controlled; flows into audit comments and observer records. Not cut inside a surrogate pair.
        client = client?.Length > 128 ? client[..(char.IsHighSurrogate(client[127]) ? 127 : 128)] : client;

        return new AgentToolContext
        {
            SupportsApproval = false,
            // azp: kept for non-OpenIddict issuers.
            ClientId = principal.FindClientId() ?? principal.FindFirst("azp")?.Value,
            ClientName = string.IsNullOrWhiteSpace(client) ? "MCP external agent" : $"MCP external agent: {client}"
        };
    }
}
