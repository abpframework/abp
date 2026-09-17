```json
//[doc-seo]
{
    "Description": "Define event handlers, background jobs, background workers, script code editing, autocomplete, and dry-run testing in the ABP Low-Code Designer."
}
```

# Script Actions

> **Preview:** Script actions are part of the preview Low-Code System. Descriptor fields, designer screens, and script context members may change before general availability.

Use **Actions** in the Low-Code Designer when descriptor metadata and generated CRUD behavior are not enough. Actions can add JavaScript-backed HTTP endpoints, distributed event handlers, background jobs, and scheduled background workers.

Custom HTTP endpoints are documented separately in [Custom Endpoints](custom-endpoints.md). This page focuses on the shared action designer experience and the event handler, background job, and background worker action types.

## Action Types

| Type | Use it for | Runtime trigger |
|------|------------|-----------------|
| Custom endpoint | Expose a small model-owned REST API | HTTP request to the configured route |
| Event handler | React to a named distributed event | `events.publishAsync(...)` or any compatible distributed event publisher |
| Background job | Run named JavaScript work asynchronously | `jobs.enqueueAsync(...)` |
| Background worker | Run recurring JavaScript work | `period` or `cronExpression` |

All action scripts run server-side and use the shared [Scripting API](scripting-api.md). Available services can be enabled or disabled per action type with scripting capability profiles.

## Script Code Editor

The Designer uses a code editor for JavaScript fields in custom endpoints, event handlers, background jobs, background workers, and interceptors.

The editor provides:

* Syntax highlighting for JavaScript
* Type-aware completions for low-code services
* Entity name completions for `db`, file, image, and attachment helpers
* Entity property completions for query lambda parameters and query results
* Enum completions through `enums` and `enumValues`
* File and image field selector completions through `fileFields` and `imageFields`
* An **Available context** list for the services enabled for the selected script type

The `fileFields` and `imageFields` globals are not lists of every entity property. They are selector trees for `File` and `Image` properties used by `files.save(...)`, `files.get(...)`, `images.save(...)`, and `images.get(...)`.

```javascript
await files.save(fileFields.Acme.Campaigns.Campaign.Document, {
    fileName: 'brief.pdf',
    contentType: 'application/pdf',
    base64: base64Content
});

await images.save(imageFields.Acme.Campaigns.Campaign.BannerImage, {
    fileName: 'banner.png',
    contentType: 'image/png',
    base64: base64Image
});
```

Regular entity fields autocomplete from entity records and query lambda parameters:

```javascript
var campaignQuery = await db.query('Acme.Campaigns.Campaign');
var rows = await campaignQuery
    .where(campaign => campaign.Name.includes('Spring'))
    .select(campaign => ({
        id: campaign.Id,
        name: campaign.Name
    }))
    .toList();
```

If the selected model layer is read-only, the Designer shows the JavaScript in a read-only editor. Switch to a writable layer before editing.

## Event Handlers

Event handlers run when a distributed event with the configured name is published.

### Descriptor

| Field | Type | Description |
|-------|------|-------------|
| `name` | string | Unique event handler name |
| `eventName` | string | Distributed event name to handle |
| `javascript` | string | JavaScript handler body |
| `description` | string | Optional documentation text |

### Context

| Global | Description |
|--------|-------------|
| `handler` | Handler runtime metadata |
| `event` | Runtime event metadata with `name` and `data` |
| `eventName` | Event name string |
| `eventData` | Event payload |

### Example

```json
{
  "eventHandlers": [
    {
      "name": "NotifyCampaignCompleted",
      "eventName": "Acme.Campaigns.CampaignCompleted",
      "description": "Logs and notifies when a campaign is completed",
      "javascript": "context.log('Campaign completed: ' + eventData.id);\nawait email.queueAsync('ops@example.com', 'Campaign completed', eventData.id);"
    }
  ]
}
```

Publish the event from another script:

```javascript
await events.publishAsync('Acme.Campaigns.CampaignCompleted', {
    id: campaignId,
    completedAt: new Date().toISOString()
});
```

## Background Jobs

Background jobs define named JavaScript handlers that can be enqueued from scripts.

### Descriptor

| Field | Type | Description |
|-------|------|-------------|
| `name` | string | Unique job name used by `jobs.enqueueAsync(...)` |
| `javascript` | string | JavaScript job body |
| `description` | string | Optional documentation text |

### Context

| Global | Description |
|--------|-------------|
| `job` | Job runtime metadata |
| `jobName` | Job name string |
| `jobData` | Parsed job payload |
| `jobJsonData` | Raw JSON payload |

### Example

```json
{
  "backgroundJobs": [
    {
      "name": "SendCampaignSummary",
      "description": "Sends a summary for one campaign",
      "javascript": "var campaign = await db.get('Acme.Campaigns.Campaign', jobData.campaignId);\nif (!campaign) { userFriendlyError('Campaign not found.'); }\nawait email.queueAsync(jobData.to, 'Campaign summary', campaign.Name);"
    }
  ]
}
```

Enqueue the job from another script:

```javascript
var jobId = await jobs.enqueueAsync('SendCampaignSummary', {
    campaignId: campaignId,
    to: 'ops@example.com'
}, {
    priority: 'Normal',
    delayMs: 60000
});
```

## Background Workers

Background workers run recurring JavaScript work on a schedule.

### Descriptor

| Field | Type | Description |
|-------|------|-------------|
| `name` | string | Unique worker name |
| `period` | number | Period in milliseconds |
| `cronExpression` | string | Cron expression for scheduled execution |
| `javascript` | string | JavaScript worker body |
| `description` | string | Optional documentation text |

Configure either `period` or `cronExpression`.

### Context

| Global | Description |
|--------|-------------|
| `worker` | Worker runtime metadata |
| `workerName` | Worker name string |

### Example

```json
{
  "backgroundWorkers": [
    {
      "name": "CampaignCleanup",
      "period": 3600000,
      "description": "Runs every hour",
      "javascript": "var query = await db.query('Acme.Campaigns.Campaign');\nvar stale = await query.where(c => c.Status === 0).take(100).toList();\ncontext.log('Stale draft count: ' + stale.length);"
    }
  ]
}
```

## Test JavaScript

Where the Designer shows **Test JavaScript**, you can run the current editor content without saving it. The built-in dry-run panel is available for custom endpoints, interceptors, event handlers, background jobs, and background workers.

For custom endpoints, provide request data:

* Method
* Path
* Route values
* Query values
* Headers
* Body JSON

For interceptors, provide the command name, entity name, command data, and an optional record id. For event handlers, provide event data JSON. For background jobs and background workers, provide the job or worker input JSON that the script expects.

When the script uses the HTTP API, you can also define outbound HTTP mocks. If a script calls `http.getAsync(...)`, `http.postAsync(...)`, or another HTTP helper, the dry-run engine returns the matching mock response instead of calling the real URL. If no mock matches, the result includes an HTTP mock miss.

Dry-run behavior:

| Operation | Dry-run behavior |
|-----------|------------------|
| Database writes | Executed in a transaction and rolled back |
| Low-code file/image/attachment operations | Captured as side effects without persisting files |
| Email send or queue | Captured as an `email` side effect; no email is sent |
| Event publish | Captured as an `event` side effect; no event is published |
| Background job enqueue | Captured as a `job` side effect; no job is enqueued |
| Outbound HTTP | Matched against HTTP mocks; no real HTTP call is made |
| Logs | Returned in the test result |
| Errors | Returned with type, message, and diagnostics when available |

Dry-run results can include:

* Endpoint response data
* Execution status and duration
* Logs
* Captured side effects
* Error details

The endpoint dry-run still evaluates the endpoint authentication and permission metadata against the current user. If the test user is not authenticated or does not have the required permission, the dry-run returns the corresponding `401` or `403` endpoint response.

## Operational Guidance

* Prefer metadata and generated CRUD behavior before adding scripts.
* Keep scripts small and focused.
* Use explicit permissions for custom endpoints.
* Use `take()` and specific filters for database queries.
* Treat public unauthenticated endpoints as public API surface.
* Keep outbound HTTP, email, event, job, file, and blob limits enabled for tenant-authored scripts.
* Use capability profiles to disable services that a script type does not need.

## See Also

* [Low-Code Designer](designer.md)
* [Custom Endpoints](custom-endpoints.md)
* [Interceptors](interceptors.md)
* [Scripting API](scripting-api.md)
* [Model Descriptor Files](model-json.md)
