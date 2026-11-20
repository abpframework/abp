```json
//[doc-seo]
{
    "Description": "Security notes and known limits of the ABP Agent Tools module: in-process authorization, redaction, policy visibility, MCP tokens, caching and deployment checklist."
}
```

# Agent Tools: Security Notes and Known Limits

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

## Production Checklist

- **Expose little.** Include only the services agents need, and exclude the rest. Review the tool list on the [management page](./policy.md#management-page) before you go live.
- **Keep writes behind approval.** Turn off **Requires approval** only for writes you accept an agent making without a human, and prefer [patch-style update DTOs](./exposing-tools.md#update-dtos-prefer-patch-style).
- **Grant the MCP scope to MCP clients only**, and keep `AbpAgentToolsOpenIddictOptions.ScopeResources` at its default. See [MCP Endpoint](./mcp.md#authentication-and-the-scope-rule). Granting the scope also gives the client the [resource permission](./openiddict.md#the-resource-permission) for the MCP URL.
- **Set the public MCP URL** (`App:SelfUrl` or `AbpAgentToolsOpenIddictOptions.McpResource`) in the auth server and the `DbMigrator`.
- **Lower the OpenIddict log level.** At the Information level OpenIddict logs authorization codes and PKCE verifiers. See [Logging in Production](./openiddict.md#logging-in-production).
- **Keep client secrets out of `appsettings.json`.** Use user secrets, environment variables or a vault.
- **Configure forwarded headers** behind a proxy, so the MCP metadata names public URLs. See [Behind a Proxy](./mcp.md#behind-a-proxy).
- **Use a shared distributed cache** (for example Redis) with more than one instance, so policy changes reach every instance at once. See [Caching](./policy.md#caching).
- **Run authentication after routing**, as ABP's templates do. MCP-only tokens are confined to the MCP endpoint by the routed endpoint; with authentication before routing they are refused everywhere.

## In-Process Authorization

Tools are called in-process through the application service interface, so ABP's interceptors (authorization, validation, features, auditing) run as they do for other in-process calls. The module also runs ABP's authorization and feature checks for the controller action and controller class of hand-written controllers.

Application services whose conventional controllers are registered in `PreConfigure<AbpAspNetCoreMvcOptions>` are not proxied by ABP, because MVC filters do the cross-cutting work for HTTP calls. For such services the invoker runs ABP's global feature check, method authorization, feature check, validation and audit logging itself before the call. Limits:

- Global MVC authorization (an `AuthorizeFilter`, the fallback policy, `RequireAuthorization` on endpoints) is not applied in-process. Only attributes are. Protect actions you expose with `[Authorize]` attributes.
- For unproxied services, the entity history interceptor and interceptors that your application adds are not run.
- Permission checks inside method bodies (other than the policy names of ABP's CRUD application services) are enforced when the tool is called, but the tool list cannot see them. Such a tool can be listed and then refused.

## Redaction

Argument values whose names contain `authorization`, `cookie`, `bearer`, `session`, `token`, `secret`, `password`, `passwd`, `passphrase`, `pwd`, `apikey`, `accesskey`, `privatekey`, `credential`, `connectionstring` or `clientsecret` (ignoring case, `_` and `-`; nested values too) are replaced by `***` in logs and call records. Add name parts with `AbpAgentToolOptions.AdditionalSensitiveArgumentNames`:

```csharp
Configure<AbpAgentToolOptions>(options =>
{
    options.AdditionalSensitiveArgumentNames.Add("iban");
});
```

Matching is by property name only. Values in name/value lists, such as `headers: [{ "name": "Authorization", "value": "..." }]`, are not redacted.

Error messages returned to the model are what ABP would send an HTTP client: internal exception details are hidden unless `AbpExceptionHandlingOptions.SendExceptionsDetailsToClients` is set, and stack traces are never included.

## Policy

- The tool policy can only narrow what a user may do. A user still needs the permissions of the application service method behind a tool.
- A tenant never sees host values directly; inherited values are reported as **Default**. A tenant can still infer one thing: every tool is enabled in code, so an inherited disabled tool can only have been disabled by the host. The HTTP method and kind also hint at the code default for approval.
- On databases where `NULL` values are distinct in unique indexes (PostgreSQL, SQLite, MySQL), two administrators changing the same tool in the host at the same moment can store two host values. The oldest one is used, and the next change of that tool removes the other.
- After writing policy values, do not read the policy store in the same transactional unit of work. The store reads committed values on a connection of its own and can wait for your open transaction on some databases. The module itself never does this.

## MCP

- **Read-only by default.** MCP has no approval step, so tools that require approval (every write, by default) are not offered.
- **No list change notifications.** The endpoint never sends `notifications/tools/list_changed`; clients see policy changes when they list the tools again.
- **Bearer tokens in the header only.** Cookies, query string and form body tokens never authenticate the endpoint.
- **Revoked tokens.** With `EnableTokenEntryValidation` (the default), a revoked token is refused at once instead of at its expiry.
- **Remote dynamic claims.** Hosts that enable remote dynamic claims (`WebRemoteDynamicClaimsPrincipalContributorOptions.IsEnabled`, in microservice-style solutions) cannot use MCP-only tokens: the remote claims refresh refuses them and calls end with 401. The default tiered template is not affected, because its dynamic claims are resolved locally.
- **Bare metadata document.** The document at `/.well-known/oauth-protected-resource` names the `/mcp` resource, which a strict RFC 9728 client may reject. The path-specific document is correct.
- **Client name.** The MCP client name in audit logs and call records is the request's `User-Agent` header, which the client controls. Use the OAuth client id to identify clients.

## Discovery and Schemas

- Services with `[RemoteService(false)]` are not in the API description model and cannot be tools.
- Stream and file inputs are detected on the parameters and one level into input classes; a stream property inside a nested object of the input is not detected.
- An initializer equal to the type's default (`= 0`, `= false`) cannot be seen, so such a property is required in the schema unless it is nullable or has `[DefaultValue]`.
- In code without nullable reference type annotations, reference type properties count as optional, so the full-replace update warning does not fire there.
- Schema patterns are checked as .NET regular expressions; providers use ECMAScript-like engines.

## User Interface

The management page is available for MVC / Razor Pages. Blazor and Angular pages, and an approval step in AI Management chat and over MCP, are not available yet.
