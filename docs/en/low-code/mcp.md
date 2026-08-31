```json
//[doc-seo]
{
    "Description": "Connect remote MCP clients to the ABP Low-Code Designer MCP endpoint with OpenIddict client credentials tokens."
}
```

# MCP Integration

> **Preview:** The low-code MCP surface is part of the preview Low-Code System. Endpoint behavior, authorization requirements, and mutation metadata may change before general availability.

The Low-Code Designer exposes a **remote Model Context Protocol (MCP) server** from the running ABP HTTP API host. It is not a local `stdio` tool package. MCP clients connect to your application over HTTP, authenticate with an OpenIddict access token, and work against the same **runtime database-backed** model that the Designer edits in the **Runtime JSON** layer.

Use [Model Descriptor Files](model-json.md) when you need source-controlled `_Dynamic/model/**/*.json` files. Use MCP when an agent or automation needs authenticated, structured runtime access that the Designer can immediately show.

MCP uses the same Low-Code feature model as the Designer. Use [Data Modeling and Page Behavior](data-modeling.md), [Data Import](data-import.md), [Calculated and Rollup Properties](formula-properties.md), and [Model History and Recovery](model-history.md) for feature semantics; this page only covers MCP connection and safe automation concerns.

## Remote Endpoint

The low-code MCP endpoint is hosted by the backend application that includes the Low-Code Designer HTTP API module:

```text
https://localhost:<host-port>/mcp/low-code-designer
```

Connection details:

* **Transport:** Streamable HTTP / HTTP MCP transport
* **Authentication:** `Authorization: Bearer <access-token>`
* **Server name:** `abp-low-code-runtime-designer`
* **Scope:** Runtime database-backed low-code model only

Use the backend host URL, not the React runtime URL and not the Admin Console SPA URL. In tiered solutions, connect to the HTTP API host where the low-code designer API module is loaded.

## Required Permissions

The endpoint requires the Low-Code Designer permission group:

| Permission | Required for |
|------------|--------------|
| `AbpLowCodeDesigner.Default` | Connecting and reading runtime model metadata |
| `AbpLowCodeDesigner.Edit` | Validating and applying runtime model changes |
| `AbpLowCodeDesigner.ScriptTest` | Running script dry-runs through MCP |

Grant these permissions to the **OpenIddict application/client** that will request the token, not only to a user or role. In Admin Console, use the permission action on the OpenIddict application row and grant the required Low-Code Designer permissions for the client.

See the [Permission Management Module](../modules/permission-management.md#permission-management-dialog) for the standard permission dialog behavior.

## Create an Access Token

For unattended MCP automation, create a dedicated OpenIddict application instead of reusing an interactive user token.

1. Open **Admin Console**.
2. Go to **OpenIddict** > **Applications**.
3. Create a new application, for example `LowCodeMcpClient`.
4. Set **Client type** to `Confidential client`.
5. Set a strong **Client secret** and store it securely.
6. Open the **Authorization** tab and enable **Allow client credentials flow**.
7. Open the **Scopes** tab and allow the API scope used by the backend that hosts the low-code APIs, for example `<YourAppName>`.
8. Save the application.
9. From the application row, open **Permissions** and grant the Low-Code Designer permissions required by your MCP scenario.

The OpenIddict application and scope management screens are described in [Creating a Client Credentials Application](../modules/openiddict-pro.md#creating-a-client-credentials-application).

The Admin Console application screen should show the confidential client with **Allow client credentials flow** enabled:

![openiddict-client-credentials-application](../images/openiddict-client-credentials-application.png)

Request an access token from the OpenIddict authority with the saved client id, client secret, and selected API scope. In non-tiered solutions this is usually the same backend host; in tiered solutions it is usually the Auth Server URL:

```bash
curl -X POST "https://localhost:<auth-server-port>/connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=client_credentials" \
  -d "client_id=LowCodeMcpClient" \
  -d "client_secret=<client-secret>" \
  -d "scope=<YourAppName>"
```

Use the returned `access_token` as the bearer token in the MCP client configuration.

```text
Authorization: Bearer <access-token>
```

## Connect From An MCP Client

For ABP AI Management or another MCP client that supports remote HTTP servers, create a server entry with:

| Setting | Value |
|---------|-------|
| Name | `ABP Low-Code Designer` |
| Transport | HTTP / Streamable HTTP |
| Endpoint URL | `https://localhost:<host-port>/mcp/low-code-designer` |
| Authentication | Bearer token |
| Token | The OpenIddict access token created for `LowCodeMcpClient` |

For clients that use JSON configuration, the same connection usually looks like this:

```json
{
  "mcpServers": {
    "abp-low-code-designer": {
      "type": "streamable-http",
      "url": "https://localhost:<host-port>/mcp/low-code-designer",
      "headers": {
        "Authorization": "Bearer <access-token>"
      }
    }
  }
}
```

Client configuration names differ by product. If your client uses `http`, `streamableHttp`, or a separate `authorization` field, keep the same endpoint URL and bearer token.

## Runtime-Only Scope

The MCP surface is **runtime-only**:

* It always reads and writes the database-backed runtime model.
* It does not choose between source-controlled and runtime layers at call time.
* It does not edit source-controlled `_Dynamic/model/**/*.json` descriptor files.
* It does not replace the normal migration workflow for source-controlled model changes.

Runtime MCP writes can extend descriptors that originate from lower layers, but inherited storage details remain restricted. For example, runtime edits can typically change display labels and runtime-owned validators, but cannot change inherited property type, database mapping, required or unique behavior, foreign key settings, or file and image storage options.

## Safe Automation Workflow

After the MCP client connects, let it discover the available capabilities from the server. The normal write workflow should still be conservative:

1. Read the current runtime item before planning a change.
2. Fetch the current mutation metadata and concurrency stamp.
3. Build a small mutation batch that touches only the changed semantic paths.
4. Validate the batch before writing.
5. Apply the validated batch.
6. Re-read the changed item and review [Health](health.md).

Treat validation and health review as part of the normal MCP workflow. They are especially important for forms, dashboard layouts, relation changes, and delete operations that can leave broken references.

## Troubleshooting

| Problem | Check |
|---------|-------|
| `401 Unauthorized` | The MCP client is not sending `Authorization: Bearer <access-token>`, the token expired, or the token was issued by an authority that the backend host does not trust. |
| `403 Forbidden` | The OpenIddict client does not have the required `AbpLowCodeDesigner.*` permissions. |
| Client cannot connect | Verify that the backend host is running and that the URL points to `/mcp/low-code-designer` on the HTTP API host. |
| Writes fail | Grant `AbpLowCodeDesigner.Edit`, refresh the token, and rebuild the change from the latest runtime metadata. |
| Script dry-run fails with authorization | Grant `AbpLowCodeDesigner.ScriptTest` and generate a new token. |

## Designer and MCP Together

Use the Designer when you want interactive editing and visual review. Use MCP when a remote agent or automation needs authenticated, repeatable runtime changes. After MCP-driven changes, reopen the relevant Designer section or review [Health](health.md) before treating the runtime model as ready.

## See Also

* [Low-Code Designer](designer.md)
* [Health](health.md)
* [Model Descriptor Files](model-json.md)
* [OpenIddict Module (Pro)](../modules/openiddict-pro.md#creating-a-client-credentials-application)
* [Permission Management Module](../modules/permission-management.md#permission-management-dialog)
