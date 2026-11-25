```json
//[doc-seo]
{
    "Description": "Register MCP clients such as Claude Code in the ABP OpenIddict Pro UI with the MCP client presets of the application form."
}
```

# Agent Tools: MCP Client Preset (Pro)

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

The [MCP endpoint](../../framework/infrastructure/artificial-intelligence/agent-tools/mcp.md) is used by OAuth clients that are registered in OpenIddict. [OpenIddict Setup](../../framework/infrastructure/artificial-intelligence/agent-tools/openiddict.md) describes what such a client needs and how to register one from configuration. With the [OpenIddict Pro module](../openiddict-pro.md) you can register it in the UI.

## Using a Preset

In the MVC UI, the **New application** dialog under **Administration** > **OpenId** > **Applications** has a **Preset** dropdown above the form:

- **Custom** (default): the form as usual.
- **MCP client (public, e.g. Claude Code)**
- **MCP client (confidential)**

Choosing an MCP preset fills the form: the client type, the authorization code and refresh token flows, PKCE required, explicit consent, and the `AgentTools` and `offline_access` scopes. The public preset also adds the placeholder redirect URI `http://localhost:<port>/callback`, which cannot be saved as it is: replace `<port>` with the fixed loopback port of the client. Enter the client id and display name (and a secret for the confidential preset), review the form and save. Nothing is created until you save.

The MCP presets are offered only when a scope named `AgentTools` exists. The [scope seed](../../framework/infrastructure/artificial-intelligence/agent-tools/openiddict.md#scope-seed) of the `Volo.Abp.OpenIddict.AgentTools` package creates it with the default `ScopeName`; with a custom scope name the presets are not shown. They are available in the MVC UI only.

## The Resource Permission

The application form has no field for resource permissions. You do not need one: when you save a client with the `AgentTools` scope in a host that loads the `Volo.Abp.OpenIddict.AgentTools` package, it adds [the resource permission](../../framework/infrastructure/artificial-intelligence/agent-tools/openiddict.md#the-resource-permission) for the MCP URL, so Claude Code and other clients that send the `resource` parameter can use it (unless `SyncMcpResourcePermission` is off). In a solution with a separate auth server, the application is saved through the API host, which usually does not load the package: run the `DbMigrator` after you save the client, and its data seed adds the permission.

## Connecting the Client

See [Registering Claude Code](../../framework/infrastructure/artificial-intelligence/agent-tools/openiddict.md#registering-claude-code) and [Registering a Confidential Agent](../../framework/infrastructure/artificial-intelligence/agent-tools/openiddict.md#registering-a-confidential-agent). The [client seed](../../framework/infrastructure/artificial-intelligence/agent-tools/openiddict.md#client-seed) remains an alternative for clients you want to register from configuration.
