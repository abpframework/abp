# ABP Platform 10.3 RC Has Been Released

We are happy to release [ABP](https://abp.io) version **10.3 RC** (Release Candidate). This blog post introduces the new features and important changes in this new version.

Try this version and provide feedback for a more stable version of ABP v10.3! Thanks to you in advance.

## Get Started with the 10.3 RC

You can check the [Get Started page](https://abp.io/get-started) to see how to get started with ABP. You can either download [ABP Studio](https://abp.io/get-started#abp-studio-tab) (**recommended**, if you prefer a user-friendly GUI application - desktop application) or use the [ABP CLI](https://abp.io/docs/latest/cli).

By default, ABP Studio uses stable versions to create solutions. Therefore, if you want to create a solution with a preview version, first you need to create a solution and then switch your solution to the preview version from the ABP Studio UI:

![studio-switch-to-preview.png](studio-switch-to-preview.png)

## Migration Guide

There are no explicitly marked breaking changes in this version. However, there are still some important migration notes for specific scenarios. Please check the migration guide if you are upgrading from v10.2 or earlier: [ABP Version 10.3 Migration Guide](https://abp.io/docs/10.3/release-info/migration-guides/abp-10-3).

## What's New with ABP v10.3?

In this section, I will introduce some major features released in this version.
Here is a brief list of titles explained in the next sections:

- OpenIddict: `private_key_jwt` Client Authentication + `abp generate-jwks`
- Event Bus: String-Based Event Publishing with Dynamic Payload
- Background Jobs/Workers: String-Based Publishing with Dynamic Payload
- API Definition Endpoint: Descriptions and Documentation Support
- Entity Cache: New Batch APIs (`FindMany*` / `GetMany*`)
- Angular: User/Tenant Sharing and Tenant Switch Experience
- Angular: Upgrade to 21.2 + TypeScript 5.9
- Introducing the `Volo.Abp.LuckyPenny.AutoMapper` Provider
- Security Improvements (Account Pro Module)

### OpenIddict: `private_key_jwt` Client Authentication + `abp generate-jwks`

ABP v10.3 introduces end-to-end support for OpenIddict `private_key_jwt` client authentication.  
Instead of using a shared `client_secret`, clients can now authenticate with an asymmetric key pair: keep the private key on the client, and register the public key (JWKS) on the authorization server.

On the open-source side, ABP CLI now includes the `abp generate-jwks` command (and the OpenIddict demo was updated accordingly). On the Pro side, OpenIddict application management now supports storing and validating JWKS for confidential applications.

This is especially useful for machine-to-machine and compliance-focused environments where shared secrets are not preferred.

**Example - Generate a JWKS with ABP CLI:**

```bash
abp generate-jwks --alg RS256 --key-size 2048 -o ./keys -f my-client
```
> See the community article [Secure Client Authentication with private_key_jwt in ABP 10.3](https://abp.io/community/articles/secure-client-authentication-with-privatekeyjwt-in-abp-b2rf18bc) for a full walkthrough.
> This approach is especially useful for Pro solutions that manage confidential clients in the administration UI.

### Event Bus: String-Based Event Publishing with Dynamic Payload

ABP v10.3 adds string-based publishing and subscription APIs for event-driven integrations.

When you do not know event types at compile time, you can now publish and handle events by name without introducing extra wrapper contracts up front. This is especially useful for plugin ecosystems, partner integrations, and metadata-driven application flows.

This is not a separate eventing model. Dynamic events run through the same ABP infrastructure (including outbox/inbox when configured), can be handled through `DynamicEventData`, and can coexist with typed handlers for the same event name. Distributed providers support this approach except Dapr, which requires startup-time topic declarations.

**Example - Publish by event name:**

```csharp
await _distributedEventBus.PublishAsync(
    "OrderPlaced",
    new { OrderId = input.Id, CustomerEmail = input.Email }
);
```

**Example - Subscribe dynamically at runtime:**

```csharp
eventBus.Subscribe("PartnerOrderReceived",
    new PartnerOrderHandler(context.ServiceProvider));

public class PartnerOrderHandler : IDistributedEventHandler<DynamicEventData>
{
    public Task HandleEventAsync(DynamicEventData eventData)
    {
        // eventData.EventName + eventData.Data
        return Task.CompletedTask;
    }
}
```

> See the community article [Dynamic Events in ABP](https://abp.io/community/articles/dynamic-events-in-abp-dukq95m1) for details.

### Background Jobs/Workers: String-Based Publishing with Dynamic Payload

ABP v10.3 introduces **Dynamic Background Jobs** (`IDynamicBackgroundJobManager`) and **Dynamic Background Workers** (`IDynamicBackgroundWorkerManager`) for runtime registration and execution by name.

With these APIs, you can enqueue jobs with dynamic payloads, register handler delegates at startup, and add/update/remove recurring workers at runtime. This is especially useful for plugin architectures, metadata-driven workflows, and tenant-specific scheduling scenarios where task types are not known at compile time.

Dynamic background jobs work through ABP's existing typed job pipeline (including provider integrations), while dynamic workers support runtime schedule management (period/cron depending on provider).

**Example - Enqueue a job by name with dynamic payload:**

```csharp
await _dynamicBackgroundJobManager.EnqueueAsync("emails", new
{
    EmailAddress = input.CustomerEmail,
    Subject = "Order Confirmed",
    Body = $"Your order {input.OrderId} has been placed."
});
```

**Example - Update worker schedule at runtime:**

```csharp
await workerManager.UpdateScheduleAsync(
    "InventorySyncWorker",
    new DynamicBackgroundWorkerSchedule { Period = 10000 } // 10s
);
```

> See [#25059](https://github.com/abpframework/abp/pull/25059) and the community article [Dynamic Background Jobs and Workers in ABP](https://abp.io/community/articles/dynamic-background-jobs-and-workers-in-abp-wfdkdsq9) for details.

### API Definition Endpoint: Descriptions and Documentation Support

The API definition endpoint can now optionally return richer metadata such as summary/description fields for controllers, actions, and parameters.

This is particularly useful for dynamic client generation, API explorers, and tooling that consumes ABP API metadata directly without requiring OpenAPI parsing.

> See [#25022](https://github.com/abpframework/abp/pull/25022) for details.

### Entity Cache: New Batch APIs (`FindMany*` / `GetMany*`)

ABP v10.3 extends `IEntityCache` with batch retrieval APIs so you can resolve multiple entities in a single cache/database flow instead of looping over `FindAsync`/`GetAsync`.

It includes both list-based APIs (`FindManyAsync` / `GetManyAsync`) and dictionary-based APIs (`FindManyAsDictionaryAsync` / `GetManyAsDictionaryAsync`) so you can choose the shape that best matches your access pattern.

**Example - List-based batch retrieval (preserves input order):**

```csharp
var ids = new List<Guid> { id1, id2, id1 };

var products = await _productCache.GetManyAsync(ids);      // throws if any ID is missing
var productsOrNull = await _productCache.FindManyAsync(ids); // null for missing IDs
```

**Example - Dictionary-based batch retrieval (fast lookup by ID):**

```csharp
var productsById = await _productCache.GetManyAsDictionaryAsync(ids);
var nullableProductsById = await _productCache.FindManyAsDictionaryAsync(ids);

if (nullableProductsById.TryGetValue(id1, out var product) && product != null)
{
    // use product
}
```

All of these methods are optimized for bulk scenarios by internally batching cache misses via distributed cache multi-get/multi-add operations.

> See [#25088](https://github.com/abpframework/abp/pull/25088) and [#25090](https://github.com/abpframework/abp/pull/25090) for details.

### Angular: User/Tenant Sharing and Tenant Switch Experience

ABP v10.3 enhances Angular UX for shared-user multi-tenancy scenarios, including invitation flows, tenant switch UX, and related identity/account integrations.

This improves the out-of-the-box experience for applications using tenant user sharing.

> See [#25051](https://github.com/abpframework/abp/pull/25051) for details.

### Angular: Upgrade to 21.2 + TypeScript 5.9

ABP v10.3 upgrades Angular to **21.2** and TypeScript to **5.9**, bringing the Angular UI stack to the latest ABP-supported frontend baseline.

This helps you stay current with the modern Angular and TypeScript ecosystem while benefiting from newer compiler/tooling improvements and maintaining compatibility with the ABP Angular packages in this release.

> See [#25072](https://github.com/abpframework/abp/pull/25072) for details.

### Introducing the `Volo.Abp.LuckyPenny.AutoMapper` Provider

ABP v10.3 introduces `Volo.Abp.LuckyPenny.AutoMapper` as a new optional provider integration for projects that want to use the LuckyPenny-maintained AutoMapper package.

The existing `Volo.Abp.AutoMapper` package remains unchanged, and migration is straightforward: replace `AbpAutoMapperModule` with `AbpLuckyPennyAutoMapperModule` in your module dependencies while keeping the same ABP-facing namespaces and APIs. 

This update also addresses the AutoMapper 14.x vulnerability context ([GHSA-rvv3-g6hj-g44x](https://github.com/advisories/GHSA-rvv3-g6hj-g44x)), and ABP documentation was expanded with installation, usage, and migration guidance. For more information, see the documentation: [LuckyPenny AutoMapper Integration](https://abp.io/docs/10.3/framework/infrastructure/luckypenny-automapper).

### Security Improvements (Account Pro Module)

ABP Commercial v10.3 RC also includes notable account security hardening:

- Optional CAPTCHA for forgot-password flow
- Operation-based rate limiting policies for account confirmation/token operations (including updated/default policies for reset and token endpoints)
- Session revocation after sensitive credential operations (password change/reset/admin reset)
- Stronger profile picture upload validation (allowed extensions, max size, and magic-bytes checks)

These changes are security-focused and are designed to be practical for real projects. Here are the key points and how you can tune them:

- **Forgot-password abuse protection**: You can enable CAPTCHA for forgot-password flows to reduce automated reset attempts.
- **Operation-level rate limiting**: Token/confirmation/reset operations now rely on policy-based limits, so you can centralize and customize limits per operation.
- **Safer session behavior**: Password changes/resets now revoke sessions to reduce risk from stolen or long-lived sessions.
- **Profile picture hardening**: Uploads are checked by extension, size, and file signature (magic bytes), not only by client-provided metadata.

**Example - Tune profile picture upload restrictions:**

```csharp
Configure<AbpProfilePictureOptions>(options =>
{
    options.AllowedFileExtensions = new[] { ".jpg", ".jpeg", ".png" };
    options.MaxFileSizeInBytes = 2 * 1024 * 1024; // 2 MB
});
```

**Example - Override account operation rate-limiting policies:**

```csharp
Configure<AbpOperationRateLimitingOptions>(options =>
{
    options.ConfigurePolicy(
        AbpAccountOperationRateLimitPolicies.SendPasswordResetCode,
        policy =>
        {
            policy.ClearRules();
            policy.PerHour(5);
            policy.PerDay(20);
        });
});
```

> See the community article [Operation Rate Limiting in ABP Framework](https://abp.io/community/articles/operation-rate-limiting-in-abp-framework-f4jtd6sn) for conceptual guidance.

### Other Improvements and Enhancements

- **Permission integration endpoint update**: `PermissionIntegrationController.IsGrantedAsync` now uses `HttpPost` for large payload scenarios ([#25177](https://github.com/abpframework/abp/pull/25177)).
- **OpenIddict dependency update**: Upgraded to OpenIddict 7.3.0 ([#25053](https://github.com/abpframework/abp/pull/25053)).
- **Autofac integration update**: Upgraded `Autofac.Extensions.DependencyInjection` to 11.0.0 ([#25190](https://github.com/abpframework/abp/pull/25190)).
- **MongoDB dependency update**: Bumped MongoDB.Driver to 3.7.1 ([#25114](https://github.com/abpframework/abp/pull/25114)).
- **OIDC auth storage options for Angular UI (pro)**: OIDC auth storage is now configurable.

## Community News

### New ABP Community Articles

As always, exciting articles have been contributed by the ABP community. I will highlight some of them here:

- [Liming Ma](https://abp.io/community/members/maliming) has published 6 new posts:
   - [Dynamic Events in ABP](https://abp.io/community/articles/dynamic-events-in-abp-dukq95m1)
   - [Dynamic Background Jobs and Workers in ABP](https://abp.io/community/articles/dynamic-background-jobs-and-workers-in-abp-wfdkdsq9)
   - [Shared User Accounts in ABP Multi-Tenancy](https://abp.io/community/articles/shared-user-accounts-in-abp-multitenancy-mf3bkg79)
   - [Secure Client Authentication with private_key_jwt in ABP 10.3](https://abp.io/community/articles/secure-client-authentication-with-privatekeyjwt-in-abp-b2rf18bc)
   - [Operation Rate Limiting in ABP Framework](https://abp.io/community/articles/operation-rate-limiting-in-abp-framework-f4jtd6sn)
   - [Resource-Based Authorization in ABP Framework](https://abp.io/community/articles/resourcebased-authorization-in-abp-framework-choku1sn)
- [One Endpoint, Many AI Clients: Turning ABP Workspaces into OpenAI-Compatible Models](https://abp.io/community/articles/turning-abp-workspaces-into-openai-compatible-endpoints-u3ls1gp4) by [Engincan Veske](https://abp.io/community/members/EngincanV)
- [Automatically Validate Your Documentation: How We Built a Tutorial Validator](https://abp.io/community/articles/automatically-validate-your-documentation-m3ozgkhv) by [Mansur Besleney](https://abp.io/community/members/mansur.besleney)
- [Automate Localhost Access for Expo: A Guide to Dynamic Cloudflare Tunnels & Dev Builds](https://abp.io/community/articles/automate-localhost-access-for-expo-a-guide-to-dynamic-7cblqtj3) by [Sumeyye Kurtulus](https://abp.io/community/members/sumeyye.kurtulus)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP related (text or video) content](https://abp.io/community/posts/create) to the ABP Community.

## Conclusion

This version comes with some new features and a lot of enhancements to the existing features. You can see the [Road Map](https://abp.io/docs/10.3/release-info/road-map) documentation to learn about the release schedule and planned features for the next releases. Please try ABP v10.3 RC and provide feedback to help us release a more stable version.

Thanks for being a part of this community!
