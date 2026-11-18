```json
//[doc-seo]
{
    "Description": "Serve the current user's governed agent tools to external agents such as Claude Code over a stateless MCP Streamable HTTP endpoint authenticated by the ABP application's OpenIddict."
}
```

# Agent Tools: MCP Endpoint

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

The `Volo.AgentTools.Mcp` package serves the current user's tools to external agents (Claude Code, a server-side agent, a customer's own agent) over the [Model Context Protocol](https://modelcontextprotocol.io/). The endpoint is a stateless Streamable HTTP server. It is authenticated with a bearer token from the application's OpenIddict server, and every call runs as the token's user in the token's tenant.

## Enabling the Endpoint

Add the `Volo.AgentTools.Mcp` package to the host that runs the application services and depend on its module. The module maps the endpoint itself; there is no `Use...` or `Map...` call to add.

```csharp
[DependsOn(typeof(AgentToolsMcpModule))]
public class MyProjectNameHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAgentToolOptions>(options =>
        {
            options.IncludedRemoteServiceNames.Add("Default"); // which services are tools
        });

        Configure<AbpAgentToolsMcpOptions>(options =>
        {
            options.ServerName = "My Project";
            options.ToolFilter = tool => tool.Kind == AgentToolKind.Read; // optional
        });
    }
}
```

`AbpAgentToolOptions` decides which services are tools (see [Exposing Tools](./exposing-tools.md)); the MCP options can only narrow that. Clients also need an OpenIddict client registration; see [OpenIddict Setup](./openiddict.md).

## Options

| `AbpAgentToolsMcpOptions` property | Default | Meaning |
| --- | --- | --- |
| `Path` | `/mcp` | Endpoint path. |
| `RequiredScope` | `AgentTools` | The scope the token must carry. Keep it equal to `AbpAgentToolsOpenIddictOptions.ScopeName`. |
| `AuthenticationScheme` | `OpenIddict.Validation.AspNetCore` | The only scheme that authenticates the endpoint. It must be a bearer scheme: the application does not start when the scheme is not registered, is a cookie scheme or a policy scheme. |
| `ServerName`, `ServerVersion` | `null` | MCP `serverInfo`. `null` keeps the MCP SDK's default; with a name set, the version defaults to `1.0.0`. The name is also the `resource_name` of the metadata document. |
| `AuthorizationServer` | `null` | The issuer named in the metadata document. `null` uses the request's own `{scheme}://{host}{pathBase}/`. |
| `ToolFilter` | `null` | A `Func<AgentToolDefinition, bool>` that selects the tools MCP may offer, on top of the policy and the permissions. `null` offers all. |

## What Is Offered

- `tools/list` returns the current user's tools that are enabled, permitted and do not require approval, passed through `ToolFilter`. Names, descriptions, input schemas and the annotations `readOnlyHint`, `destructiveHint` and `idempotentHint` come from the tool definition.
- MCP has no approval step here, so tools that require approval are not listed and a call to one is refused. Every write requires approval by default, so **the endpoint is read-only by default**. To offer a write tool, turn its **Requires approval** switch off on the [management page](./policy.md#management-page), or use `[AgentTool(RequiresApproval = false)]`.
- With the `AgentTools.Enable` feature off for the tenant, the list is empty and every call is refused.
- `tools/call` goes through the [invoker](./chat-clients.md#calling-a-tool-by-name). Refusals and failures are MCP tool results with `isError` set and the text `ERROR <type>: <message>`, not protocol errors: the model reads the reason and the connection stays usable. A tool left out by `ToolFilter` is answered exactly like an unknown tool.
- The endpoint never sends `notifications/tools/list_changed`. Clients get policy changes when they list the tools again.

## Authentication and the Scope Rule

Requests are checked in this order. Each refusal has a JSON body with `error` and `error_description`:

1. A bearer token in the `Authorization` header is required, validated by `AuthenticationScheme` only. Cookies never authenticate the endpoint, and tokens in the query string or form body are not used. No token gives 401 with `WWW-Authenticate: Bearer resource_metadata="<metadata URL>"`; an invalid token gives 401 with `WWW-Authenticate: Bearer error="invalid_token", resource_metadata="<metadata URL>"`.
2. The token must carry `RequiredScope`, otherwise 403 with `error="insufficient_scope"`.
3. The token must have a user. A client credentials token gives 403.
4. The token's tenant must exist and be active, otherwise 403.
5. The tenant that ABP's tenant resolvers find in the request (header, query string, cookie, route or domain) must be the token's tenant; a host token's request must name no tenant. Otherwise 403.

Then the request runs as the token's user in the token's tenant, and the audit log entry of the request gets that user, tenant and client. With ABP's [dynamic claims](../../framework/fundamentals/dynamic-claims.md) enabled, the principal is refreshed, and a revoked session ends with 401.

**Why a dedicated scope:** ABP builds a token's audience (`aud`) from the resources of the requested scopes, not from the `resource` parameter that MCP clients send. The audience therefore cannot tell an MCP token from a normal API token, but a dedicated scope can. Grant the `AgentTools` scope only to MCP clients, and a normal API token never opens the endpoint. Granting the `AgentTools` scope makes a client an MCP client: with `Volo.AgentTools.OpenIddict`, it also gets the resource permission for the MCP URL (see [OpenIddict Setup](./openiddict.md#the-resource-permission)). With `Volo.AgentTools.OpenIddict`, a token that only carries the MCP scope is also refused everywhere except the MCP endpoint (see [OpenIddict Setup](./openiddict.md#audience-and-token-confinement)), so an MCP client cannot call the REST API behind the tools it is not offered.

## Protected Resource Metadata

The endpoint publishes [RFC 9728](https://www.rfc-editor.org/rfc/rfc9728) metadata documents, anonymously, at `/.well-known/oauth-protected-resource/mcp` (the document named in the 401 response) and at `/.well-known/oauth-protected-resource`:

```json
{
  "resource": "https://app.example.com/mcp",
  "authorization_servers": [ "https://app.example.com/" ],
  "scopes_supported": [ "AgentTools", "offline_access" ],
  "bearer_methods_supported": [ "header" ]
}
```

`resource_name` is added when `ServerName` is set. Clients find the authorization server here and request the `AgentTools` and `offline_access` scopes.

The document at the bare well-known path also names the `/mcp` resource. A client that strictly applies RFC 9728 may reject it; the path-specific document is the one to use.

## Behind a Proxy

The URLs in the metadata and in the `resource_metadata` hint are built from the request. Behind a reverse proxy or load balancer, configure the [forwarded headers](../../deployment/forwarded-headers.md) (scheme and host) and `AllowedHosts`, or clients are sent to internal addresses. With a `PathBase`, the well-known paths are under it (`<PathBase>/.well-known/oauth-protected-resource/mcp`). Set `AuthorizationServer` when the issuer is not the application itself.

## Applications That Validate Tokens with JwtBearer

The default `AuthenticationScheme` is OpenIddict validation, which applications that host their own OpenIddict server use. An API host that validates tokens of a separate auth server with JwtBearer (as the tiered templates do) gives the endpoint a JwtBearer scheme of its own, whose audience is the MCP URL:

```csharp
var configuration = context.Services.GetConfiguration();

context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddAbpJwtBearer(options =>                       // the application's API scheme, as generated
    {
        options.Authority = configuration["AuthServer:Authority"];
        options.RequireHttpsMetadata = configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata");
        options.Audience = "MyProjectName";
    })
    .AddJwtBearer("AgentToolsMcp", options =>         // the MCP endpoint's scheme
    {
        options.Authority = configuration["AuthServer:Authority"];
        options.RequireHttpsMetadata = configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata");
        options.Audience = configuration["App:SelfUrl"]!.TrimEnd('/') + "/mcp";
    });

Configure<AbpAgentToolsMcpOptions>(options =>
{
    options.AuthenticationScheme = "AgentToolsMcp";
    options.AuthorizationServer = new Uri(configuration["AuthServer:Authority"]!);
});
```

- The MCP scheme's audience must be exactly the auth server's `AbpAgentToolsOpenIddictOptions.McpResource`. On the auth server, set `McpResource` to the API host's MCP URL when `App:SelfUrl` there is the auth server's own URL.
- Never point `AuthenticationScheme` at the application's API scheme, and do not give the MCP scope the API audience instead: every MCP token would then be a full API token, and the client could call the REST API directly.
- Use plain `AddJwtBearer` for the second scheme. Calling `AddAbpJwtBearer` twice breaks the dynamic claims refresh URL.
- Keep the MCP scheme's other options, including `MapInboundClaims`, equal to the API scheme's, so the endpoint reads the user, tenant and roles with the same claim types.
- JwtBearer cannot read encrypted access tokens. ABP's OpenIddict module disables access token encryption; keep it so.
- Both sides need the same scope name and MCP URL.

## Audit and Observers

MCP calls carry the token's `client_id` as the client id and `MCP external agent: <User-Agent>` as the client name. The audit log comment is `Agent tool <name> (client: <client id>; MCP external agent: <User-Agent>)`, and [call observers](./chat-clients.md#observing-calls) get the same values. The endpoint is stateless, so the client's MCP `clientInfo` is not kept; the `User-Agent` header identifies the client software.

## Connecting a Client

Register the client in OpenIddict and connect it to `https://<your-app>/mcp`. [OpenIddict Setup](./openiddict.md) shows the registration and the `claude mcp add` command for Claude Code.
