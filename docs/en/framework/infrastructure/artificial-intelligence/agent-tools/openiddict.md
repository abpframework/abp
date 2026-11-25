```json
//[doc-seo]
{
    "Description": "Configure the ABP OpenIddict server for the Agent Tools MCP endpoint and register MCP clients such as Claude Code and confidential server-side agents."
}
```

# Agent Tools: OpenIddict Setup

The `Volo.Abp.OpenIddict.AgentTools` package configures the application's [OpenIddict](../../../../modules/openiddict.md) server for the [MCP endpoint](./mcp.md): the `AgentTools` scope, the MCP URL as a resource, the resource permission of every client with the scope, client seeding and a startup check.

The package depends only on `Volo.Abp.OpenIddict.AspNetCore`, not on `Volo.Abp.AgentTools.Mcp`, so a separate auth server can use it. The scope name and the MCP URL are therefore configured on both sides and must match.

## Installation

Add the package to the host that runs the OpenIddict server (the auth server, or the single application) and to the `DbMigrator`:

```bash
abp add-package Volo.Abp.OpenIddict.AgentTools
```

Then depend on the module in both:

```csharp
[DependsOn(typeof(AbpOpenIddictAgentToolsModule))]
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
| `EnableTokenEntryValidation` | `true` | OpenIddict validation checks the token entry, so a token that is revoked individually is refused at once instead of at expiry (see [Revoked tokens](./security.md#mcp)). It costs one read of the token entry per bearer request, application-wide. With more than one instance, configure a shared [distributed cache](../../../fundamentals/caching.md). `false` leaves the application's own setting unchanged. |
| `SyncMcpResourcePermission` | `true` | Every application that has the scope gets the resource permission for `McpResource`, and loses it when the scope is removed. See [The Resource Permission](#the-resource-permission). |
| `ConfineScopeToMcpEndpoint` | `true` | A token whose only API scope is `ScopeName` is accepted at the MCP endpoint only, whatever its audiences, and the server refuses a token request of such a grant that leaves the scope out. See [Audience and Token Confinement](#audience-and-token-confinement). |

The options are applied when the OpenIddict options are built, so the order in which the application configures OpenIddict does not matter.

### Audience and Token Confinement

ABP puts the resources of the requested scopes into the token's `aud`. The seeded scope's resource is the MCP URL, so an MCP token has `aud = <MCP URL>`. When the host's OpenIddict validation restricts audiences, the module adds `McpResource` to them; when it accepts any audience, nothing is added.

The host's OpenIddict validation accepts a token at the MCP endpoint only, and refuses it with `invalid_token` on any other endpoint, when one of these holds:

- Its only API scope is `AgentTools` (`ScopeName`), whatever its audiences. The standard scopes `openid`, `profile`, `email`, `phone`, `address`, `roles` and `offline_access` do not count as API scopes.
- It has the MCP URL as an audience and no other audience that the host accepts, whatever its scopes. A host that does not restrict audiences accepts any other audience.

Any other token is not affected. For example, a token with the scopes `AgentTools` and `MyProjectName` is accepted at the MCP endpoint and by the API whose audience the second scope gives. So grant an MCP client no other API scope.

Restrict the audiences that the host accepts (`AddAudiences`, as ABP's templates do). A host that accepts any audience also accepts tokens that were not issued for one of its APIs.

The server refuses with `invalid_scope` a token request whose `scope` parameter leaves `AgentTools` out when that is the only API scope of the grant. `ConfineScopeToMcpEndpoint = false` turns this off too.

The check uses the routed endpoint, so authentication must run after `UseRouting`, as in ABP's templates; otherwise such a token is refused everywhere.

The confinement is part of OpenIddict validation. The OpenIddict server's own endpoints, such as the userinfo endpoint, still take the token. An action that allows anonymous callers still answers, as to an anonymous caller.

Do not add the audience of an API to `ScopeResources`. The tokens of MCP clients would then open that API, including the services behind the tools the endpoint hides, in every host that validates with JwtBearer or does not load `AbpOpenIddictAgentToolsModule`.

Set `ConfineScopeToMcpEndpoint` to `false` only when the scope is meant to open an API of the application, which then is one of `ScopeResources`. An MCP client can then call that API as the user, without the rules of the MCP endpoint. The audience rule still applies.

### Startup Check

`AgentToolsOpenIddictStartupValidator` runs when a web host starts (not in the `DbMigrator`, and not in a host started only to migrate the database).

With `SyncMcpResourcePermission` on (the default), the host fails to start with an `AbpException` when the MCP URL is unknown (neither `App:SelfUrl` nor `McpResource` is set), because applications with the scope could not get the resource permission. Otherwise the check only logs. It logs an error when the scope should have the MCP URL as a resource (`ScopeResources` is empty or contains it) but the stored scope does not, for example after a change of `App:SelfUrl`. It logs warnings when:

- `McpResource` is unknown (with `SyncMcpResourcePermission` off), so clients that send `resource=<MCP URL>` are refused;
- the MCP URL is not a registered OpenIddict resource;
- the scope does not exist (the seeder did not run, or the module is not loaded in the `DbMigrator`);
- the scope's resources include none of the audiences the host accepts (checked only when audiences are restricted, and not logged next to the error above).

## Scope Seed

`AgentToolsScopeDataSeedContributor` creates the `AgentTools` scope on the host side, with English and Turkish display names (shown on the consent page) and the resources described above. When the MCP URL is unknown and `ScopeResources` is empty, the scope is not created and a warning is logged: set the URL and seed again.

An existing scope is left unchanged, so after a change of `App:SelfUrl` (or `McpResource`) it keeps its old resources. Update the resources of the scope then: add the new URL and remove the old one. Until you do, a host that restricts audiences refuses a token whose only API scope is `AgentTools`, also at the MCP endpoint. A host that accepts any audience still accepts it at the MCP endpoint, and only there.

After the update, users sign in to their MCP clients again. Revoke the tokens issued before individually, for example with `IOpenIddictTokenManager.TryRevokeAsync`.

## What an MCP Client Needs

Every MCP client registration needs:

- the authorization code and refresh token grants, and the response type `code`;
- the authorization, token and revocation endpoints;
- PKCE required;
- explicit consent (recommended);
- the `AgentTools` scope (the client requests `AgentTools offline_access`);
- a resource permission for the MCP URL, when the client sends the `resource` parameter (Claude Code does). Without it the authorization and token requests that carry `resource` are refused. The module adds it for you (see below).

## The Resource Permission

Granting the `AgentTools` scope to a client makes it an MCP client: the module adds the resource permission for the MCP URL (`McpResource`) to every OpenIddict application that has the scope permission, and removes only that permission when the scope is taken away. It does this on every create and update of an application, whatever saves it: a management UI, the data seeders, or your own code that uses OpenIddict's application manager.

This happens in the hosts that load the module. In a solution with a separate auth server, a management UI usually saves applications through the API host, which does not load it. Run the `DbMigrator` after you save an MCP client there: the data seed contributor below then adds or removes the permission.

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

`AgentToolsMcpClientSeedContributor` creates one OpenIddict application per entry of the `AgentTools:Mcp:Clients` configuration section, on the host side. The key is the client id. Nothing happens without the section, and an existing client id is left unchanged. Each seeded client gets exactly the set above, including the resource permission, with the consent type of its entry.

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

## Registering Clients in a Management UI

A client gets the resource permission when it is saved with the `AgentTools` scope in a host that loads the module, or when data is seeded (see [The Resource Permission](#the-resource-permission)), so a management UI needs no field for it. The [OpenIddict module (Pro)](../../../../modules/openiddict-pro.md) has such a UI, with [presets for MCP clients](../../../../modules/agent-tools/openiddict-preset.md).

## Logging in Production

At the Information level OpenIddict logs the parameters of its requests. Set the `OpenIddict` log category to `Warning` in production hosts, for example in the Serilog configuration of `Program.cs`:

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
