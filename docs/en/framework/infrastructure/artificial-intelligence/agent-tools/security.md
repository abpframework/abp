```json
//[doc-seo]
{
    "Description": "Security notes and known limits of ABP Agent Tools: in-process authorization, redaction, tool policy, MCP tokens and a production checklist."
}
```

# Agent Tools: Security Notes and Known Limits

## Production Checklist

- **Expose little.** Include only the services agents need, and exclude the rest. Review the tool list (`IAgentToolCatalog.Tools`, and `Skipped` for what was left out) before you go live.
- **Keep writes behind approval.** Set `RequiresApproval` to `false` only for writes you accept an agent making without a human, and prefer [patch-style update DTOs](./exposing-tools.md#update-dtos-prefer-patch-style).
- **Grant the MCP scope to MCP clients only**, and keep `AbpAgentToolsOpenIddictOptions.ScopeResources` at its default. See [MCP Endpoint](./mcp.md#authentication-and-the-scope-rule). Granting the scope also gives the client the [resource permission](./openiddict.md#the-resource-permission) for the MCP URL.
- **Restrict the audiences your host accepts** (`AddAudiences`), as ABP's templates do. The token confinement relies on it. See [Audience and Token Confinement](./openiddict.md#audience-and-token-confinement).
- **Set the public MCP URL** (`App:SelfUrl` or `AbpAgentToolsOpenIddictOptions.McpResource`) in the auth server and the `DbMigrator`.
- **Lower the OpenIddict log level.** At the Information level OpenIddict logs the parameters of its requests. See [Logging in Production](./openiddict.md#logging-in-production).
- **Keep client secrets out of `appsettings.json`.** Use user secrets, environment variables or a vault.
- **Configure forwarded headers** behind a proxy, so the MCP metadata names public URLs. See [Behind a Proxy](./mcp.md#behind-a-proxy).
- **Check the startup log for policy errors.** Most policy settings that have no effect are logged as errors and do not stop the application. See [What Is Only Logged](./policy.md#what-is-only-logged).
- **Run authentication after routing**, as ABP's templates do. The routed endpoint tells where a token of an MCP client is accepted; with authentication before routing such a token is refused everywhere. See [Audience and Token Confinement](./openiddict.md#audience-and-token-confinement).
- **Update the MCP scope after a change of `App:SelfUrl`.** The existing scope keeps its old resources, so MCP clients may be refused until you update them. Startup logs an error. See [Scope Seed](./openiddict.md#scope-seed).

## In-Process Authorization

Tools are called in-process through the application service interface, so ABP's interceptors (authorization, validation, features, auditing) run as they do for other in-process calls. The invoker also runs ABP's authorization and feature checks for the controller action and controller class of hand-written controllers.

Application services whose conventional controllers are registered in `PreConfigure<AbpAspNetCoreMvcOptions>` are not proxied by ABP, because MVC filters do the cross-cutting work for HTTP calls. For such services the invoker runs ABP's global feature check, method authorization, feature check, validation and audit logging itself before the call. Limits:

- Global MVC authorization (an `AuthorizeFilter`, the fallback policy, `RequireAuthorization` on endpoints) is not applied in-process. Only attributes are. Protect actions you expose with `[Authorize]` attributes.
- For unproxied services, the entity history interceptor and interceptors that your application adds are not run.
- Permission checks inside method bodies (other than the policy names of ABP's CRUD application services) are enforced when the tool is called, but the tool list cannot see them. Such a tool can be listed and then refused.

## Redaction

Argument values whose names contain `authorization`, `cookie`, `bearer`, `session`, `token`, `secret`, `password`, `passwd`, `passphrase`, `pwd`, `apikey`, `accesskey`, `privatekey`, `credential`, `connectionstring` or `clientsecret` (ignoring case, `_` and `-`; nested values too, also inside string values that hold a JSON object or array) are replaced by `***` in logs and call records. A string that looks like a JSON object or array but is too long or nested too deeply to be checked is shown as `***`. Only a string that is exactly one JSON object or array is looked into; any other text is shown as it is, so do not send secrets in free text. The tool itself gets the original values. Add name parts with `AbpAgentToolOptions.AdditionalSensitiveArgumentNames`:

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
- The [configuration policy](./policy.md) applies to the whole application, the same for the host and every tenant.
- With the [Agent Tools module (Pro)](../../../../modules/agent-tools/policy.md), host and tenant administrators can re-enable a tool that the configuration disables, and turn its approval off. To keep a tool away from everyone, exclude it with `ExcludedServiceTypes` or `[DisableAgentTool]` instead.
- A misspelled section name is not always noticed. See [What Is Not Checked](./policy.md#what-is-not-checked).

## MCP

- **Read-only by default.** MCP has no approval step, so tools that require approval (every write, by default) are not offered.
- **No list change notifications.** The endpoint never sends `notifications/tools/list_changed`; clients see a changed tool list when they list the tools again.
- **This endpoint only.** The checks apply to the Agent Tools endpoint. MCP endpoints that the application maps itself are the application's responsibility. See [Other MCP Servers in the Same Application](./mcp.md#other-mcp-servers-in-the-same-application).
- **Bearer tokens in the header only.** Cookies, query string and form body tokens never authenticate the endpoint.
- **Revoked tokens.** With `AbpAgentToolsOpenIddictOptions.EnableTokenEntryValidation` (the default), a host that loads `AbpOpenIddictAgentToolsModule` and validates tokens with OpenIddict refuses a token at once when it is revoked individually: at the revocation endpoint (RFC 7009) or with `IOpenIddictTokenManager.TryRevokeAsync`. With more than one instance, configure a shared [distributed cache](../../../fundamentals/caching.md). To take the access of a client away, revoke each of its access and refresh tokens. A host that validates with JwtBearer accepts a revoked token until it expires.
- **Deleted clients.** Before you delete an OpenIddict application, revoke its tokens individually with `IOpenIddictTokenManager.TryRevokeAsync`.
- **Deactivated and deleted users.** Enable [dynamic claims](../../../fundamentals/dynamic-claims.md) and the [session management](../../../../modules/identity/session-management.md) of the Identity module (Pro). Deactivating a user in the user management then ends the sessions, and so the tokens, of that user. Without them, also revoke the user's tokens individually when you deactivate or delete a user.
- **Token confinement.** It applies to OpenIddict validation only. The OpenIddict server's own endpoints, such as the userinfo endpoint, still take an MCP token, and an action that allows anonymous callers answers as to an anonymous caller.
- **Remote dynamic claims.** Hosts that enable remote dynamic claims (`WebRemoteDynamicClaimsPrincipalContributorOptions.IsEnabled`, in microservice-style solutions) cannot use MCP-only tokens: the remote claims refresh refuses them and calls end with 401. The default tiered template is not affected, because its dynamic claims are resolved locally.
- **Client name.** The MCP client name in audit logs and call records is the request's `User-Agent` header, which the client controls. Use the OAuth client id to identify clients.

## Discovery and Schemas

- Services with `[RemoteService(false)]` are not in the API description model and cannot be tools.
- The arguments of a tool are read from JSON, so the model fills members of an input class with `[BindNever]` or `[FromServices]` (in a request body also `[FromHeader]`), a `[FromHeader]` parameter of the primary constructor of a record query DTO, and parameters with `[ModelBinder]` or a custom binding source. Mark such an action with `[DisableAgentTool]` if the model must not set the value.
- Stream and file inputs are detected on the parameters and one level into input classes; a stream property inside a nested object of the input is not detected.
- An initializer equal to the type's default (`= 0`, `= false`) cannot be seen, so such a property is required in the schema unless it is nullable or has `[DefaultValue]`.
- In code without nullable reference type annotations, reference type properties count as optional, so the full-replace update warning does not fire there.
- Schema patterns are checked as .NET regular expressions; providers use ECMAScript-like engines.

## Approvals

Approvals work in your own chat client (see [Approvals](./chat-clients.md#approvals)). There is no approval step over MCP yet.
