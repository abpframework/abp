# Best Practices for Designing Backward‑Compatible REST APIs in a Microservice Solution for .NET Developers

## Introduction
With microservice architecture, each service develops and ships independently at its own pace, and clients infrequently update in lockstep. **Backward compatibility** means that when you release new versions, current consumers continue to function without changing code. This article provides a practical, 6–7 minute tutorial specific to **.NET developers**.

---
## What Counts as “Breaking”? (and what doesn’t)
A change is **breaking** if a client that previously conformed can **fail at compile time or runtime**, or exhibit **different business‑critical behavior**, **without** changing that client in any way. In other words: if an old client needs to be altered in order to continue functioning as it did, your change is breaking.

### Examples of breaking changes
- **Deleting or renaming an endpoint** or modifying its URL/route.
- **Making an existing field required** (e.g., requiring `address`).
- **Data type or format changes** (e.g., `price: string` → `price: number`, or date format changes).
- **Altering default behavior or ordering** that clients implicitly depend on (hidden contracts).
- **Changing the error model** or HTTP status codes in a manner that breaks pre-existing error handling.
- **Renaming fields** or **making optional fields required** in requests or responses.
- **Reinterpreting semantics** (e.g., `status="closed"` formerly included archived items, but no longer does).

### Examples of non‑breaking changes
- **Optional fields or query parameters can be added** (clients may disregard them).
- **Adding new enum values** (if the clients default to a safe behavior for unrecognized values).
- **Adding a new endpoint** while leaving the previous one unchanged.
- **Performance enhancements** that leave input/output unchanged.
- **Including metadata** (e.g., pagination links) without changing the current payload shape.

> Golden rule: **Old clients should continue to work exactly as they did before—without any changes.**

---
## Versioning Strategy
Versioning is your master control lever for managing change. Typical methods:

1) **URI Segment** (simplest)
```
GET /api/v1/orders
GET /api/v2/orders
```
Pros: Cache/gateway‑friendly; explicit in docs. Cons: URL noise.

2) **Header‑Based**
```
GET /api/orders
x-api-version: 2.0
```
Pros: Clean URLs; multiple reader support. Cons: Needs proxy/CDN rules.

3) **Media Type**
 Accept: application/json;v=2

 Pros: Semantically accurate. <br> Cons: More complicated to test and implement. <br> **Recommendation:** For the majority of teams, favor **URI segments**, with an optional **`x-api-version`** header for flexibility.

### Quick Setup in ASP.NET Core (Asp.Versioning)
```csharp
// Program.cs
using Asp.Versioning;

builder.Services.AddControllers();
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true; // response header: api-supported-versions
    o.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version")
    );
});

builder.Services.AddVersionedApiExplorer(o =>
{
    o.GroupNameFormat = "'v'VVV"; // v1, v2
    o.SubstituteApiVersionInUrl = true;
});
```
```csharp
// Controller
using Asp.Versioning;

[ApiController]
[Route("api/v{version:apiVersion}/orders")]
public class OrdersController : ControllerBase
{
    [HttpGet]
    [ApiVersion("1.0", Deprecated = true)]
    public IActionResult GetV1() => Ok(new { message = "v1" });

    [HttpGet]
    [MapToApiVersion("2.0")]
    public IActionResult GetV2() => Ok(new { message = "v2", includes = new []{"items"} });
}
```

---
## Schema Evolution Playbook (JSON & DTO)
Obey the following rules for compatibility‑safe evolution:

- **Add‑only changes**: Favor adding **optional** fields; do not remove/rename fields.
- **Maintain defaults**: When the new field is disregarded, the old functionality must not change.
- **Enum extension**: Clients should handle unknown enum values gracefully (default behavior).
- **Deprecation pipeline**: Mark fields/endpoints as deprecated **at least one version** prior to removal and publicize extensively. - **Stability by contract**: Record any unspoken contracts (ordering, casing, formats) that clients depend on.

### Example: adding a non‑breaking field
```csharp
public record OrderDto(
    Guid Id,
    decimal Total,
    string Currency,
    string? SalesChannel // new, optional
);
```

---
## Compatibility‑Safe API Behaviors
- **Error model**: Use a standard structure (e.g., RFC 7807 `ProblemDetails`). Avoid ad‑hoc error shapes on a per-endpoint basis.
- **Versioning/Deprecation communication** through headers:
- `api-supported-versions: 1.0, 2.0`
- `Deprecation: true` (in deprecated endpoints)
- `Sunset: Wed, 01 Oct 2025 00:00:00 GMT` (planned deprecation date)
- **Idempotency**: Use an `Idempotency-Key` header for retry-safe POSTs.
- **Optimistic concurrency**: Utilize `ETag`/`If-Match` to prevent lost updates.
- **Pagination**: Prefer cursor tokens (`nextPageToken`) to protect clients from sorting/index changes.
- **Time**: Employ ISO‑8601 in UTC; record time‑zone semantics and rounding conventions.

---
## Rollout & Deprecation Policy
A good deprecation policy is **announce → coexist → remove**:

1) **Announce**: Release changelog, docs, and comms (mail/Slack) with v2 information and the sunset date.
2) **Coexist**: Operate v1 and v2 side by side. Employ gateway percentage routing for progressive cutover.
3) **Observability**: Monitor errors/latency/usage **by version**. When v1 traffic falls below ~5%, plan for removal. 4) **Remove**: Post sunset date, return **410 (Gone)** with a link to migration documentation.

**Canary & Blue‑Green**: Initialize v2 with a small traffic portion and compare error/latency budgets prior to scaling up.

---
## Contract & Compatibility Testing
- **Consumer‑Driven Contracts**: Write expectations using Pact.NET; verify at provider CI.
- **Golden files / snapshots**: Freeze representative JSON payloads and automatically detect regressions.
- **Version-specific smoke tests**: Maintain separate, minimal test suites for v1 and v2.
- **SemVer discipline**: Minor = backward‑compatible; Major = breaking (avoid when possible).

Minimal example (xUnit + snapshot style):
```csharp
[Fact]
public async Task Orders_v1_contract_should_match_snapshot()
{
    var resp = await _client.GetStringAsync("/api/v1/orders");
    Approvals.VerifyJson(resp); // snapshot comparison
}
```

---
## Tooling & Docs (for .NET)
- **Asp.Versioning (NuGet)**: API versioning + ApiExplorer integration.
- **Swashbuckle / NSwag**: Generate an OpenAPI definition **for every version** (`/swagger/v1/swagger.json`, `/swagger/v2/swagger.json`). Display both in Swagger UI.
- **Polly**: Client‑side retries/fallbacks to handle transient failures and ensure resilience.
- **Serilog + OpenTelemetry**: Collect metrics/logs/traces by version for observability and SLOs.

Swagger UI configuration by group name:
```csharp
app.UseSwagger();
app.UseSwaggerUI(c =>
{
c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
c.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
});
```
---

## Conclusion
Backward compatibility is not a version number—it is **disciplined change management**. When you use add‑only schema evolution, a well‑defined versioning strategy, strict contract testing, and rolling rollout, you maintain microservice independence and safeguard consumer experience.
