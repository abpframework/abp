```json
//[doc-seo]
{
    "Description": "Define JavaScript-backed custom REST endpoints in the ABP Low-Code System without writing custom .NET controllers."
}
```

# Custom Endpoints

> **Preview:** Custom endpoint descriptors and scripting helpers are part of the preview Low-Code System. Names, validation rules, and runtime behavior may change before general availability.

Use generated page CRUD APIs for normal dynamic entity pages. Custom endpoints are an advanced option for exposing small model-owned REST APIs that do not map to standard list, get, create, update, delete, export, file, or attachment operations.

Custom endpoints are defined in JSON descriptor files or through the Low-Code Designer. Each endpoint executes server-side JavaScript and is registered as an ASP.NET Core route.

## Defining Endpoints

```json
{
  "endpoints": [
    {
      "name": "GetCampaignStats",
      "route": "/api/custom/campaigns/stats",
      "method": "GET",
      "description": "Get campaign statistics",
      "requireAuthentication": true,
      "requiredPermissions": ["Acme.Campaigns"],
      "javascript": "var count = await db.count('Acme.Campaigns.Campaign');\nreturn ok({ totalCampaigns: count });"
    }
  ]
}
```

## Endpoint Descriptor

| Field | Type | Default | Description |
|-------|------|---------|-------------|
| `name` | string | Required | Unique endpoint identifier |
| `route` | string | Required | URL route pattern; must start with `/` and can contain `{parameters}` |
| `method` | string | `GET` | `GET`, `POST`, `PUT`, `DELETE`, or `PATCH` |
| `javascript` | string | Required | JavaScript handler code |
| `description` | string | null | Optional designer/documentation text |
| `requireAuthentication` | bool | `true` | Whether the caller must be authenticated |
| `useResourceAuthorization` | bool | `false` | Whether execution is granted per endpoint name through ABP resource permissions |
| `requiredPermissions` | string[] | null | Permission names required to call the endpoint |

Authorization is resolved in this order:

1. When `requiredPermissions` contains values, every named permission is required.
2. Otherwise, `useResourceAuthorization: true` requires the endpoint execute resource permission scoped to the endpoint name, with the global Low-Code endpoint permission as fallback.
3. Otherwise, `requireAuthentication` selects authenticated or public access.

Permission checks require an authorized user even when `requireAuthentication` is set to `false`. Keep endpoints authenticated by default and use `requireAuthentication: false` only for intentionally public APIs without named or resource authorization.

## Route and Request Data

Use `{paramName}` syntax for route parameters. Endpoint scripts can read request data through globals:

| Variable | Description |
|----------|-------------|
| `request` | Full request object |
| `route` | Route values, for example `route.id` |
| `params` | Alias for `route` |
| `query` | Query string values, for example `query.q` |
| `body` | Parsed request body |
| `headers` | Selected safe request headers |

```json
{
  "name": "GetCampaignById",
  "route": "/api/custom/campaigns/{id}",
  "method": "GET",
  "javascript": "var campaign = await db.get('Acme.Campaigns.Campaign', route.id);\nif (!campaign) { return notFound('Campaign not found'); }\nreturn ok({ id: campaign.Id, name: campaign.Name });"
}
```

For non-GET requests, `body` is parsed when a body is present and remains subject to the configured request size limit. The `headers` object intentionally contains only selected request headers such as `Content-Type`, `Accept`, `Accept-Language`, and `X-Requested-With`.

## Response Helpers

Endpoint scripts can return plain data, an endpoint response object, or one of the response helpers.

| Function | HTTP status | Response kind |
|----------|-------------|---------------|
| `ok(data, headers?)` | 200 | JSON |
| `okText(text, contentType?)` | 200 | Text |
| `okBinary(base64Data, contentType?)` | 200 | Binary from base64 |
| `created(data, headers?)` | 201 | JSON |
| `noContent()` | 204 | Empty |
| `badRequest(message)` | 400 | JSON error |
| `unauthorized(message)` | 401 | JSON error |
| `forbidden(message)` | 403 | JSON error |
| `notFound(message)` | 404 | JSON error |
| `error(message)` | 500 | JSON error |

For custom status codes or response metadata, return an object with response fields:

```javascript
return {
    statusCode: 202,
    kind: 'json',
    data: { accepted: true },
    headers: { 'x-trace-id': guid() }
};
```

Response kinds are:

| Kind | Description |
|------|-------------|
| `json` | JSON serialization, default |
| `text` | Plain text |
| `binaryBase64` | Base64-encoded binary payload |

## Script Services

Custom endpoint scripts use the same common [Scripting API](scripting-api.md) services as other low-code scripts:

| Service | Example |
|---------|---------|
| `db` | Query or mutate dynamic entities |
| `user` / `currentUser` | Read current user information |
| `tenant` / `currentTenant` | Read tenant information |
| `auth` / `authorization` | Check permissions |
| `settings`, `features`, `config` | Read allowed settings, features, and app configuration |
| `http` | Call allowed outbound HTTP services |
| `email` | Send or queue email |
| `events`, `jobs` | Publish distributed events or enqueue background jobs |
| `files`, `images`, `attachments` | Work with low-code files and record attachments |
| `log`, `logWarning`, `logError` | Write logs |

## Examples

### Statistics

```json
{
  "name": "GetCampaignStats",
  "route": "/api/custom/campaigns/stats",
  "method": "GET",
  "requireAuthentication": true,
  "javascript": "var campaignQuery = await db.query('Acme.Campaigns.Campaign');\nvar total = await campaignQuery.count();\nvar active = await campaignQuery.where(c => c.Status === 1).count();\nreturn ok({ total: total, active: active });"
}
```

### Search with Query Parameters

```json
{
  "name": "SearchCampaigns",
  "route": "/api/custom/campaigns/search",
  "method": "GET",
  "requireAuthentication": true,
  "javascript": "var q = query.q || '';\nvar campaignQuery = await db.query('Acme.Campaigns.Campaign');\nvar rows = await campaignQuery\n  .where(c => c.Name.toLowerCase().includes(q.toLowerCase()))\n  .take(10)\n  .toList();\nreturn ok(rows.map(c => ({ id: c.Id, name: c.Name })));"
}
```

### Create with Validation

```json
{
  "name": "CreateCampaignDraft",
  "route": "/api/custom/campaigns/draft",
  "method": "POST",
  "requireAuthentication": true,
  "requiredPermissions": ["Acme.Campaigns.Create"],
  "javascript": "if (!body.name) { return badRequest('Name is required.'); }\nvar record = await db.insert('Acme.Campaigns.Campaign', { Name: body.name, Status: 0 });\nreturn created({ id: record.Id, name: record.Name });"
}
```

## Testing Endpoint Scripts

The Low-Code Designer endpoint editor includes **Test JavaScript**. Use it to run the current editor content without saving it.

The dry-run request editor lets you provide:

* HTTP method
* Request path
* Route values
* Query values
* Headers
* Body JSON
* Outbound HTTP mocks

Dry-run execution evaluates the endpoint descriptor, request context, script, authentication metadata, and required permissions against the current user. It returns the same response shape that a real endpoint execution would return.

Side effects are captured instead of being sent to external systems:

| Operation | Dry-run behavior |
|-----------|------------------|
| Database writes | Rolled back |
| Email send or queue | Captured under **Captured Side Effects** |
| Event publish | Captured under **Captured Side Effects** |
| Background job enqueue | Captured under **Captured Side Effects** |
| Outbound HTTP | Matched against HTTP mocks |
| File, image, and attachment operations | Captured without persisting files |

If a script calls the `http` helper and no mock matches the method and URL, the result contains a mock miss instead of sending a real HTTP request.

## Response Policy

Dynamic endpoint responses are validated by `LowCode:Scripting:EndpointResponse`.

```json
{
  "LowCode": {
    "Scripting": {
      "EndpointResponse": {
        "MaxBodyBytes": 1048576,
        "AllowedContentTypes": [
          "application/json",
          "text/plain",
          "application/octet-stream"
        ],
        "BlockedHeaders": [
          "Set-Cookie",
          "Content-Length",
          "Content-Type"
        ]
      }
    }
  }
}
```

Default blocked headers also include hop-by-hop headers such as `Connection`, `Transfer-Encoding`, and `Upgrade`.

`Content-Type` is blocked as a custom response header. Choose the response kind or `contentType` field instead of setting a raw `Content-Type` header from script.

## Security Notes

* Prefer authenticated endpoints with explicit `requiredPermissions`.
* Treat endpoints with `requireAuthentication: false`, no `requiredPermissions`, and `useResourceAuthorization: false` as public API surface.
* Keep endpoint scripts small and focused.
* Validate route, query, and body input before using it.
* Use `take()` for list queries.
* Use the configured HTTP, email, file, and response limits for untrusted integrations.

## See Also

* [Scripting API](scripting-api.md)
* [Script Actions](script-actions.md)
* [Interceptors](interceptors.md)
* [Model Descriptor Files](model-json.md)
