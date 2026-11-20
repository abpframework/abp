```json
//[doc-seo]
{
    "Description": "Configure the ABP OpenIddict server for the Agent Tools MCP endpoint and register MCP clients such as Claude Code and confidential server-side agents."
}
```

# Agent Tools: OpenIddict Setup

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

The `Volo.AgentTools.OpenIddict` package configures the application's [OpenIddict](../openiddict.md) server for the [MCP endpoint](./mcp.md): the `AgentTools` scope, the MCP URL as a resource, the resource permission of every client with the scope, client seeding and a startup check.

The package depends only on `Volo.Abp.OpenIddict.AspNetCore`, not on `Volo.AgentTools.Mcp`, so a separate auth server can use it. The scope name and the MCP URL are therefore configured on both sides and must match.

## Installation

Add the package to the host that runs the OpenIddict server and to the `DbMigrator`, and depend on the module in both:

```csharp
[DependsOn(typeof(AgentToolsOpenIddictModule))]
public class MyProjectNameAuthServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAgentToolsOpenIddictOptions>(options =>
        {
            options.RequireS256CodeChallengeMethod = true; // optional
        });
    }
}
```

The `DbMigrator` needs the module because the scope, the clients and the resource permissions of existing applications are handled by its data seed contributors. Without it nothing is seeded, and the startup check of the server warns about the missing scope. Configure `App:SelfUrl` (or `McpResource`) in the `DbMigrator` too.

## Server Defaults and Options

| `AbpAgentToolsOpenIddictOptions` property | Default | Meaning |
| --- | --- | --- |
| `McpResource` | `App:SelfUrl` + `/mcp` | The MCP endpoint's absolute URL exactly as clients address it. It is registered as an OpenIddict resource, granted as a resource permission to every application with the scope (see [The Resource Permission](#the-resource-permission)), and used as the scope's resource (the token audience). Set it when the MCP path or the public URL differs. When it is `null`, nothing is registered and a web host fails to start (only a warning when `SyncMcpResourcePermission` is `false`). |
| `ScopeName` | `AgentTools` | Must equal `AbpAgentToolsMcpOptions.RequiredScope`. |
| `ScopeResources` | empty | The resources stored on the seeded scope. Empty means `[McpResource]`. Used only when the scope is created. Keep the default (see below). |
| `RequireS256CodeChallengeMethod` | `false` | Removes the `plain` PKCE method, so the server accepts S256 only. Opt-in, because it applies to every client of the application. |
| `EnableTokenEntryValidation` | `true` | OpenIddict validation checks the token entry, so a revoked token is refused at once instead of at expiry. It costs one database read per bearer request, application-wide. `false` leaves the application's own setting unchanged. |
| `SyncMcpResourcePermission` | `true` | Every application that has the scope gets the resource permission for `McpResource`, and loses it when the scope is removed. See [The Resource Permission](#the-resource-permission). |

The options are applied when the OpenIddict options are built, so the order in which the application configures OpenIddict does not matter.

### Audience and Token Confinement

ABP puts the resources of the requested scopes into the token's `aud`. The seeded scope's resource is the MCP URL, so an MCP token has `aud = <MCP URL>`. When the host's OpenIddict validation restricts audiences, the module adds `McpResource` to them; when it accepts any audience, nothing is added.

A token whose only audience is the MCP URL (a client granted only the MCP scope) is accepted at the MCP endpoint only. On any other endpoint, the host's OpenIddict validation refuses it with `invalid_token`. The check uses the routed endpoint, so authentication must run after `UseRouting`, as in ABP's templates; otherwise such a token is refused everywhere.

Do not set `ScopeResources` to the application's API audience: every MCP token would then be a full API token, and the MCP client could call the REST API directly, including the services behind the tools the endpoint hides.

### Startup Check

`AgentToolsOpenIddictStartupValidator` runs when a web host starts (not in the `DbMigrator`, and not in a host started only to migrate the database).

With `SyncMcpResourcePermission` on (the default), the host fails to start with an `AbpException` when the MCP URL is unknown (neither `App:SelfUrl` nor `McpResource` is set), because applications with the scope could not get the resource permission. Otherwise the check only logs warnings, when:

- `McpResource` is unknown (with `SyncMcpResourcePermission` off), so clients that send `resource=<MCP URL>` are refused;
- the MCP URL is not a registered OpenIddict resource;
- the scope does not exist (the seeder did not run, or the module is not loaded in the `DbMigrator`);
- the scope's resources include none of the audiences the host accepts (checked only when audiences are restricted).

## Scope Seed

`AgentToolsScopeDataSeedContributor` creates the `AgentTools` scope on the host side, with English and Turkish display names (shown on the consent page) and the resources described above. An existing scope is left unchanged.

## What an MCP Client Needs

Every MCP client registration needs:

- the authorization code and refresh token grants, and the response type `code`;
- the authorization, token and revocation endpoints;
- PKCE required;
- explicit consent;
- the `AgentTools` scope (the client requests `AgentTools offline_access`);
- a resource permission for the MCP URL, when the client sends the `resource` parameter (Claude Code does). Without it the authorization and token requests that carry `resource` are refused. The module adds it for you (see below).

## The Resource Permission

Granting the `AgentTools` scope to a client makes it an MCP client: the module adds the resource permission for the MCP URL (`McpResource`) to every OpenIddict application that has the scope permission, and removes only that permission when the scope is taken away. It does this on every create and update of an application, whatever saves it: the OpenIddict Pro UI and its application service, the data seeders, or your own code that uses OpenIddict's application manager.

A data seed contributor also fixes existing applications (host side) when data is seeded, for example applications saved before the module was installed, and logs the client id of each application it changes. In a `DbMigrator` that does not know the MCP URL (no `App:SelfUrl` and no `McpResource`), it only logs a warning; set the URL there too and seed again.

When `McpResource` changes, applications get the permission for the new URL when they are saved or seeded; the permission for the old URL is not removed.

To manage resource permissions yourself, turn the synchronization off:

```csharp
Configure<AbpAgentToolsOpenIddictOptions>(options =>
{
    options.SyncMcpResourcePermission = false;
});
```

## Client Seed

`AgentToolsMcpClientSeedContributor` creates one OpenIddict application per entry of the `AgentTools:Mcp:Clients` configuration section, on the host side. The key is the client id. Nothing happens without the section, and an existing client id is left unchanged. Each seeded client gets exactly the set above, including the resource permission.

| Key | Default | Meaning |
| --- | --- | --- |
| `Type` | `public` | `public` (CLI and desktop agents) or `confidential` (server-side agents). |
| `ClientSecret` | | Confidential clients only. Never in `appsettings.json`; use user secrets, an environment variable or a vault. |
| `RedirectUris` | | Absolute URIs, at least one. They are matched exactly, port included. |
| `DisplayName` | the client id | Shown on the consent page. |
| `ConsentType` | `explicit` | `explicit`, `external`, `implicit` or `systematic`. |

A client is skipped with a warning (the others are still seeded) when its `Type` or `ConsentType` is not one of the values above, a confidential client has no secret or an unreplaced `<...>` placeholder, a public client has a secret, or a redirect URI is missing or not absolute. Secrets are never logged.

```json
{
  "AgentTools": {
    "Mcp": {
      "Clients": {
        "claude-code": {
          "DisplayName": "Claude Code",
          "RedirectUris": [ "http://localhost:33418/callback" ]
        },
        "SupportAgent": {
          "DisplayName": "Support agent",
          "Type": "confidential",
          "RedirectUris": [ "https://agent.example.com/oauth/callback" ]
        }
      }
    }
  }
}
```

Set the confidential client's secret outside `appsettings.json`, for example with user secrets in development or an environment variable:

```bash
dotnet user-secrets set "AgentTools:Mcp:Clients:SupportAgent:ClientSecret" "<secret>"
```

```bash
AgentTools__Mcp__Clients__SupportAgent__ClientSecret=<secret>
```

Run the `DbMigrator` (or the data seeder of your host) to create the clients.

## Registering Claude Code

Claude Code is a public client (no secret). It listens for the authorization response on a loopback redirect URI, and OpenIddict matches redirect URIs exactly, including the port. So register a fixed port, such as `http://localhost:33418/callback` above, and give Claude Code the same port:

```bash
claude mcp add --transport http --client-id claude-code --callback-port 33418 my-app https://app.example.com/mcp
```

Claude Code finds the authorization server from the 401 response and the metadata document, opens the browser for the sign-in and the consent page, and runs the authorization code flow with PKCE S256. It requests the scopes `AgentTools offline_access` and sends `resource=<MCP URL>`, which is why it needs the resource permission.

The tools are listed per user: sign in as a tenant user to get that tenant's tools.

## Registering a Confidential Agent

An external server-side agent is a confidential client. Give it a secret from configuration (never `appsettings.json`), its exact callback URL as the redirect URI, and the same grants, PKCE and scopes. It authenticates at the token endpoint with its secret. It does not need to send the `resource` parameter: the scope already gives the token its audience.

## Registering a Client in the OpenIddict Pro UI

With the [OpenIddict Pro module](../openiddict-pro.md) (MVC UI), the **New application** dialog under **Administration** > **OpenId** > **Applications** has a **Preset** dropdown above the form:

- **Custom** (default): the form as usual.
- **MCP client (public, e.g. Claude Code)**
- **MCP client (confidential)**

Choosing an MCP preset fills the form: the client type, the authorization code and refresh token flows, PKCE required, explicit consent, and the `AgentTools` and `offline_access` scopes. The public preset also adds the placeholder redirect URI `http://localhost:<port>/callback`, which cannot be saved as it is: replace `<port>` with the fixed loopback port of the client. Enter the client id and display name (and a secret for the confidential preset), review the form and save. Nothing is created until you save.

The MCP presets are offered only when a scope named `AgentTools` exists (the default `ScopeName`; with a custom scope name the presets are not shown). They are available in the MVC UI; the Blazor and Angular UIs do not have them yet.

The application form has no field for resource permissions. You do not need one: when you save a client with the `AgentTools` scope, the module adds the resource permission for the MCP URL (see [The Resource Permission](#the-resource-permission)), so Claude Code and other clients that send the `resource` parameter can use it. The [client seed](#client-seed) remains an alternative for clients you want to register from configuration.

## Logging in Production

OpenIddict logs requests at the Information level, which writes authorization codes and PKCE verifiers (`code`, `code_verifier`) to the log in clear text. Set the `OpenIddict` log category to `Warning` in production hosts, for example in the Serilog configuration of `Program.cs`:

```csharp
.MinimumLevel.Override("OpenIddict", LogEventLevel.Warning)
```

or in `appsettings.json`:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Override": {
        "OpenIddict": "Warning"
      }
    }
  }
}
```
