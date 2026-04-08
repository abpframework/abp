# Dynamic Background Jobs and Workers in ABP

> This feature is available since ABP 10.3.

ABP's Background Jobs and Background Workers are two well-established infrastructure pieces. Background jobs handle fire-and-forget async tasks — sending emails, generating reports, processing orders. Background workers handle continuously running periodic tasks — syncing inventory, cleaning up expired data, pushing scheduled notifications.

This works great, but it has one assumption: **you know all your job and worker types at compile time**.

In practice, that assumption breaks down more often than you'd expect:

- You're building a **plugin system** where third-party plugins need to register their own background processing logic at runtime — you can't pre-define an `IBackgroundJob<TArgs>` implementation in the host project for every possible plugin
- Your system needs to execute background tasks based on **external configuration** (database, API responses) — the task types and parameters are entirely unknown at compile time
- Your **multi-tenant SaaS platform** needs different sync intervals for different tenants — some every 30 seconds, some every 5 minutes — and you need to adjust these without restarting the application
- You're building a **low-code/no-code platform** where end users define automation workflows through a visual designer, and those workflows need to run as background jobs or scheduled tasks — the job types and scheduling parameters are entirely determined by end users at runtime, unknowable to developers at compile time

ABP's **Dynamic Background Jobs** (`IDynamicBackgroundJobManager`) and **Dynamic Background Workers** (`IDynamicBackgroundWorkerManager`) are designed for exactly these scenarios. They let you register, enqueue, schedule, and manage background tasks by name at runtime, with no compile-time type binding required.

## Dynamic Background Jobs

`IDynamicBackgroundJobManager` offers two usage patterns, covering different levels of runtime flexibility.

### Enqueue an Existing Typed Job by Name

If you already have a typed background job (say, an `EmailSendingJob` registered via `[BackgroundJobName("emails")]`), you can enqueue it by name without referencing its args type:

```csharp
public class OrderAppService : ApplicationService
{
    private readonly IDynamicBackgroundJobManager _dynamicJobManager;

    public OrderAppService(IDynamicBackgroundJobManager dynamicJobManager)
    {
        _dynamicJobManager = dynamicJobManager;
    }

    public async Task PlaceOrderAsync(PlaceOrderInput input)
    {
        // Business logic...

        // Enqueue a confirmation email — no reference to EmailSendingJobArgs needed
        await _dynamicJobManager.EnqueueAsync("emails", new
        {
            EmailAddress = input.CustomerEmail,
            Subject = "Order Confirmed",
            Body = $"Your order {input.OrderId} has been placed."
        });
    }
}
```

The framework looks up the typed job configuration by name, serializes the anonymous object, deserializes it into the correct args type, and feeds it through the standard typed job pipeline. The caller doesn't need to `using` any specific project namespace.

### Register a Runtime Dynamic Handler

When you don't even have a job type — say a plugin decides at startup what processing logic to register — you can register a handler directly:

```csharp
public override async Task OnApplicationInitializationAsync(
    ApplicationInitializationContext context)
{
    var dynamicJobManager = context.ServiceProvider
        .GetRequiredService<IDynamicBackgroundJobManager>();

    // A plugin registers its own processing logic at startup
    dynamicJobManager.RegisterHandler("SyncExternalCatalog", async (jobContext, ct) =>
    {
        using var doc = JsonDocument.Parse(jobContext.JsonData);
        var catalogUrl = doc.RootElement.GetProperty("url").GetString();

        var httpClient = jobContext.ServiceProvider
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient();

        var catalog = await httpClient.GetStringAsync(catalogUrl, ct);
        // Process catalog data...
    });

    // Now you can enqueue jobs for this handler
    await dynamicJobManager.EnqueueAsync("SyncExternalCatalog", new
    {
        Url = "https://partner-api.example.com/catalog"
    });
}
```

The handler receives a context object containing `JsonData` (the raw JSON string) and `ServiceProvider` (a scoped container). Resolving dependencies from `ServiceProvider` is the recommended approach — avoid capturing external state in the handler closure.

There's one priority rule to keep in mind: **if a name matches both a typed job and a dynamic handler, the typed job wins**. Dynamic handlers never accidentally override existing typed jobs.

> Dynamic jobs ultimately go through the standard typed job pipeline, so they **work with every background job provider** — Default, Hangfire, Quartz, RabbitMQ, TickerQ — without any provider-specific code.

## Dynamic Background Workers

`IDynamicBackgroundWorkerManager` lets you register periodic tasks at runtime and manage their full lifecycle: add, remove, update schedule.

```csharp
public override async Task OnApplicationInitializationAsync(
    ApplicationInitializationContext context)
{
    var workerManager = context.ServiceProvider
        .GetRequiredService<IDynamicBackgroundWorkerManager>();

    await workerManager.AddAsync(
        "InventorySyncWorker",
        new DynamicBackgroundWorkerSchedule
        {
            Period = 30000 // 30 seconds
        },
        async (workerContext, cancellationToken) =>
        {
            var syncService = workerContext.ServiceProvider
                .GetRequiredService<IInventorySyncAppService>();

            await syncService.SyncAsync(cancellationToken);
        }
    );
}
```

If you're using Hangfire or Quartz as your provider, you can use a cron expression instead of a fixed interval:

```csharp
await workerManager.AddAsync(
    "DailyReportWorker",
    new DynamicBackgroundWorkerSchedule
    {
        CronExpression = "0 2 * * *" // Every day at 2:00 AM
    },
    async (workerContext, cancellationToken) =>
    {
        var reportService = workerContext.ServiceProvider
            .GetRequiredService<IReportAppService>();

        await reportService.GenerateDailyReportAsync(cancellationToken);
    }
);
```

### Runtime Schedule Management

Adding a worker is just the beginning. The real value of dynamic workers is that the entire lifecycle is controllable at runtime:

```csharp
// Check if a worker is currently registered
bool exists = workerManager.IsRegistered("InventorySyncWorker");

// A tenant upgrades their plan — speed up sync from 30s to 10s
await workerManager.UpdateScheduleAsync(
    "InventorySyncWorker",
    new DynamicBackgroundWorkerSchedule { Period = 10000 }
);

// Tenant disables the sync feature — remove the worker entirely
await workerManager.RemoveAsync("InventorySyncWorker");
```

`UpdateScheduleAsync` only changes the schedule — the handler itself stays the same. For persistent providers like Hangfire and Quartz, `UpdateScheduleAsync` and `RemoveAsync` can operate on the persistent scheduling record even after an application restart, when the handler is no longer in memory.

### Stopping All Workers

When you need to stop all dynamic workers at once (e.g., as part of a graceful shutdown), call `StopAllAsync`:

```csharp
await workerManager.StopAllAsync(cancellationToken);
```

All registered workers are stopped and cleaned up, and the handler registry is cleared. Calling `AddAsync` or `UpdateScheduleAsync` after this throws `ObjectDisposedException` — this is intentional, preventing new workers from being added during a shutdown sequence.

## Provider Support

Dynamic background jobs and dynamic background workers have different levels of provider support.

**Dynamic background jobs** are compatible with all providers because they reuse the standard typed job pipeline:

| Provider | Supported |
|---|---|
| Default (In-Memory) | ✅ |
| Hangfire | ✅ |
| Quartz | ✅ |
| RabbitMQ | ✅ |
| TickerQ | ✅ |

**Dynamic background workers** have per-provider implementations:

| Provider | AddAsync | RemoveAsync | UpdateScheduleAsync | Period | CronExpression |
|---|---|---|---|---|---|
| Default (In-Memory) | ✅ | ✅ | ✅ | ✅ | ❌ |
| Hangfire | ✅ | ✅ | ✅ | ✅ | ✅ |
| Quartz | ✅ | ✅ | ✅ | ✅ | ✅ |
| TickerQ | ❌ | ❌ | ❌ | — | — |

TickerQ uses `FrozenDictionary` for function registration, which requires all functions to be registered before the application starts. Runtime dynamic registration is not possible.

## Restart Behavior

Dynamic handlers are stored **in memory** and are not persisted across application restarts. This is a deliberate design choice — handlers are code logic (delegates), and code logic is inherently not serializable.

For persistent providers (Hangfire, Quartz), this means: enqueued jobs and recurring job entries survive a restart in the database, but the handlers need to be re-registered. If a handler is not re-registered, the job executor throws an exception (background jobs) or skips the execution with a warning log (background workers).

The recommended approach is to register handlers in `OnApplicationInitializationAsync`, so they are automatically restored on every startup:

```csharp
public override async Task OnApplicationInitializationAsync(
    ApplicationInitializationContext context)
{
    var dynamicJobManager = context.ServiceProvider
        .GetRequiredService<IDynamicBackgroundJobManager>();

    // Re-registered on every startup — persistent jobs will find their handler
    dynamicJobManager.RegisterHandler("SyncExternalCatalog", async (jobContext, ct) =>
    {
        // handler logic...
    });
}
```

## Summary

`IDynamicBackgroundJobManager` lets you enqueue jobs and register handlers by name at runtime, compatible with all background job providers, no compile-time types required. `IDynamicBackgroundWorkerManager` lets you add, remove, and update the schedule of periodic workers at runtime — Hangfire and Quartz providers also support cron expressions. Register handlers in `OnApplicationInitializationAsync` to ensure automatic recovery on every startup.

## References

- [Background Jobs](https://abp.io/docs/latest/framework/infrastructure/background-jobs)
- [Background Workers](https://abp.io/docs/latest/framework/infrastructure/background-workers)
- [Hangfire Background Job Manager](https://abp.io/docs/latest/framework/infrastructure/background-jobs/hangfire)
- [Quartz Background Job Manager](https://abp.io/docs/latest/framework/infrastructure/background-jobs/quartz)
