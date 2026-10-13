# Keeping a Fast-Growing ABP Application Fast: What We Learned from Volobox

At Volosoft we run our own work on [Volobox](https://volobox.io), a workspace where people and AI agents work together. Agents take assigned tasks, answer in chat when someone mentions them, draft documents and run scheduled playbooks. They follow the same permissions as everyone else, and their work goes into the same audit log. The workspace also has projects and tasks, team chat, docs, meetings, support desks and AI coding.

[Volobox](https://volobox.io) is an ABP application with a React front end, and it moves fast: new features ship every few days. Between mid-August and early October, we added video meetings, calendar sync, booking pages and support desks with a customer portal. We also added a choice of coding agents and coding sessions on your own machines. And it got much faster: between two releases, the home page data took about **90% less time** in a warm API replay against production, and the median replay now finishes in under half a second. In local runs, a page refresh shows the home page in 0.8 seconds, about 70% faster.

ABP is a big part of why we can build that fast. Its modules, dependency injection, units of work and conventions handle the plumbing, so we spend our time on features. In a few months [Volobox](https://volobox.io) grew to:

- thousands of C# files and a few hundred application services,
- around a thousand services with interceptors (unit of work, auditing, authorization, ...),
- dozens of background workers,
- one process in production, with Autofac, PostgreSQL and Redis.

At this size, some costs that are too small to notice in a typical application start to add up. We were preparing to deliver [Volobox](https://volobox.io) to customers, so we spent a few rounds on performance. Besides the home page numbers above, startup got 62% faster in container A/B runs, to about 14 seconds.

We kept the same database and did not rewrite anything. Here is how we found the costs that had grown with [Volobox](https://volobox.io), and what we changed.

## How we found the bottlenecks

Timings are noisy. The same request can be fast or slow depending on what else the process is doing. So we built a few signals that tell us where to look, not only that something is slow. Most of them are small and stay in the product.

### A runtime line every minute

A background worker logs one line a minute with what the whole process is doing. A simplified version:

```csharp
var stopwatch = Stopwatch.StartNew();
var pauseBefore = GC.GetTotalPauseDuration();
var allocatedBefore = GC.GetTotalAllocatedBytes();
var gen2Before = GC.CollectionCount(2);

await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);

// Under load the delay can end late, so divide by the time that really passed.
var elapsed = stopwatch.Elapsed;
var info = GC.GetGCMemoryInfo();
Logger.LogInformation(
    "Runtime saturation: threads={Threads} pending={Pending} gcPause={GcPause:P1} " +
    "alloc={AllocMb} MB gen2={Gen2} heap={HeapMb} MB",
    ThreadPool.ThreadCount,
    ThreadPool.PendingWorkItemCount,
    (GC.GetTotalPauseDuration() - pauseBefore) / elapsed,
    (GC.GetTotalAllocatedBytes() - allocatedBefore) / 1024 / 1024,
    GC.CollectionCount(2) - gen2Before,
    info.HeapSizeBytes / 1024 / 1024);
```

Ours also logs database pool waits, slow DNS lookups, heap fragmentation and the container's CPU throttling (from the cgroup `cpu.stat` file). The line does not give the answer, but it tells us which tool to pick up next:

- A high `gcPause` with a low `pending` count points to allocations. More threads will not help, so the next step is an allocation trace.
- Database pool waiters with a low `pending` count point to the connection pool, not the thread pool.
- To tell a leak from churn, compare live objects after full GCs under a similar load. The logged heap size comes from the latest GC and includes fragmentation, so it is only a first hint. If live objects keep growing, follow the references in a heap dump. If they stay flat while `alloc` is high, look for what allocates.

One caveat for `alloc`: on our .NET 10 host with Server GC, the allocation counter could stay flat for many samples and then catch up at once. We check gen 0 GC counts and allocation traces before reading much into a single minute.

### The cost of each request on its log line

The second signal puts three numbers on every request summary line: the GC pause time while the request ran, the thread pool queue length when it started, and how many resolve requests the container served for it. With Serilog's request logging, it is a small middleware that runs inside `UseSerilogRequestLogging`:

```csharp
public async Task InvokeAsync(HttpContext httpContext)
{
    var pendingAtStart = ThreadPool.PendingWorkItemCount;
    var pauseAtStart = GC.GetTotalPauseDuration();

    // Our own counter, described in the next section. It returns null when counting is off.
    using var tally = _resolveCounter.Start(httpContext.RequestServices);
    try
    {
        await _next(httpContext);
    }
    finally
    {
        _diagnosticContext.Set("GcPauseMs", (long)(GC.GetTotalPauseDuration() - pauseAtStart).TotalMilliseconds);
        _diagnosticContext.Set("ThreadPoolPending", pendingAtStart);
        if (tally != null)
        {
            _diagnosticContext.Set("DiResolves", tally.Count);
        }
    }
}
```

These fields tell us what to check for a slow request. `GcPauseMs` is process-wide: it is the GC pause time that overlapped the request. When it is close to the request's duration, an allocation trace is the next thing to check. A high `ThreadPoolPending` shows that the process had queued work when the request started. A common read in [Volobox](https://volobox.io) makes several thousand resolve requests, so hundreds of thousands means a constructor graph has grown and is worth a look.

Skip SignalR and other long-lived connections here, or one request gets the cost of hours of hub calls.

### Counting resolve requests

The resolve count comes from Autofac's own events. For one service in one scope, it takes a few lines:

```csharp
var count = 0;
using var scope = lifetimeScope.BeginLifetimeScope();
scope.ResolveOperationBeginning += (_, operation) =>
{
    operation.ResolveOperation.ResolveRequestBeginning += (_, _) => count++;
};

scope.Resolve<IProjectTaskAppService>();
// count is now the number of resolve requests for this one resolve
```

The counter in the host does the same per request and per background worker run. It also follows the scopes that a request creates through `IServiceScopeFactory`, because those start from the root container, not under the request's scope. A worker run that makes 20,000 or more resolve requests logs one line with its name, duration and item count. We turn the counter on with a configuration switch, because three event subscriptions per scope are not free under load.

### Ranking every component by graph size

To find which service to look at first, we resolved every transient and scoped application component once, each in a fresh child scope, and sorted them by resolve requests. The same event also tells us who asked for each component, which gives the edges of the graph:

```csharp
static Type Owner(Type type) =>
    type.Namespace == "Castle.Proxies" && type.BaseType != null ? type.BaseType : type;

var rows = new List<(long Count, Type Type)>();
var edges = new Dictionary<string, long>();
var failures = new List<(Type Type, string Error)>();

var registrations = root.ComponentRegistry.Registrations
    .Where(r => Owner(r.Activator.LimitType).Namespace?.StartsWith("MyCompany.MyApp") == true
                && !r.Activator.LimitType.IsGenericTypeDefinition
                && (r.Sharing == InstanceSharing.None || r.Lifetime is CurrentScopeLifetime))
    .DistinctBy(r => r.Activator.LimitType)
    .ToArray();

foreach (var registration in registrations)
{
    using var scope = root.BeginLifetimeScope();
    long count = 0;
    var componentEdges = new Dictionary<string, long>();
    scope.ResolveOperationBeginning += (_, operation) =>
        operation.ResolveOperation.ResolveRequestBeginning += (_, request) =>
        {
            count++;
            // While this event runs, the active request is the one that asked for this component.
            var parent = operation.ResolveOperation.ActiveRequestContext;
            if (parent != null)
            {
                var edge = $"{Owner(parent.Registration.Activator.LimitType).FullName} -> " +
                           $"{Owner(request.RequestContext.Registration.Activator.LimitType).FullName}";
                componentEdges[edge] = componentEdges.GetValueOrDefault(edge) + 1;
            }
        };

    try
    {
        // Resolve this registration, not whichever one is the default for its service,
        // through the service pipeline so sharing and decorators still apply.
        var service = registration.Services.First();
        var serviceRegistration = scope.ComponentRegistry.ServiceRegistrationsFor(service)
            .Single(candidate => candidate.Registration.Id == registration.Id);
        scope.ResolveComponent(new ResolveRequest(service, serviceRegistration, Array.Empty<Parameter>()));

        rows.Add((count, Owner(registration.Activator.LimitType)));
        foreach (var edge in componentEdges)
        {
            edges[edge.Key] = edges.GetValueOrDefault(edge.Key) + edge.Value;
        }
    }
    catch (DependencyResolutionException ex)
    {
        // Some components need parameters or runtime state.
        failures.Add((Owner(registration.Activator.LimitType), ex.Message));
    }
}

var rankedRows = rows.OrderByDescending(row => row.Count).ToArray();
var rankedEdges = edges.OrderByDescending(edge => edge.Value).ToArray();
```

Start with the services at the top of the ranking. The failures tell you which components it could not cover. Then check the edge list for the dependencies that make those graphs large. For a single service, resolve each constructor parameter type on its own to see how much it adds, and check which methods really use it before you make it lazy.

### Replaying the real page

To measure the home page, we took the API calls it makes (14 calls in seven groups) from a browser HAR file and replayed them with a small script against production: first all together, the way the browser sends them, then one by one. The script reads the application version first, so every result is tied to a release, and runs ten rounds for a median. Before timing, it calls every API once to warm the connections and the server, so the numbers are warm API replays. They do not include HTML, JavaScript or rendering. We ran it before and after the release with the fix.

### Allocation traces and heap dumps

When the signals point to allocations or memory, the .NET diagnostic tools show the source:

```bash
# What the runtime is doing, live
dotnet-counters monitor -p <pid> --counters System.Runtime

# Which call stacks allocate
dotnet-trace collect -p <pid> --profile gc-verbose --duration 00:00:05:00

# What is alive: take one dump before the load and one during it, then compare
dotnet-dump collect -p <pid> --type Heap -o during.dmp
dotnet-dump analyze during.dmp -c "dumpheap -stat -live" -c exit > during.txt
```

Compare two `dumpheap -stat -live` outputs to see which types grow under load.

### Controlled A/B runs

For changes that are hard to see in production, we ran the old and the new image one after the other in containers with the production CPU and memory limits (`docker run --cpus 3.8 --memory 8g`), the same seed data and the same simulated users. LLM calls were stubbed so the load stayed the same. For each run we recorded the time until the health check passed, the container's CPU time from the cgroup files, and a heap dump during the load.

## The six bottlenecks

We found six bottlenecks:

| # | Where | What we saw | What we changed | Result |
|---|---|---|---|---|
| 1 | Constructor graphs | One application service resolve triggered hundreds of thousands of resolve requests | `Lazy<T>`, keyed services, a budget test | Home page data −90% (production replay), all APIs p50 −30% (production, whole round) |
| 2 | After-commit work | One long operation held thousands of live scopes | Carry values, open a scope when the work runs | Live scopes −99%, heap after full GC −22% (lab, whole round) |
| 3 | Transactions | A page-long connection and a minute-long request held a transaction the whole time | A rule for hub routes, a separate non-transactional unit of work | Preview start peak CPU −87% (local, whole round) |
| 4 | Background workers | Workers built all their services before they knew there was work | Check first, build only when needed | Resolve requests per run −99% (local) |
| 5 | Startup | Cold connections and controllers, around a thousand proxy classes in one dynamic module | Warm up before ready, 16 proxy modules | Startup −62% (lab, whole round), first calls −97% (p50, local, whole round) |
| 6 | Front end | Six startup requests in a row and a large static JavaScript bundle | Inline runtime config, static outline, token reuse, on-demand loading, IndexedDB | Refresh −70% (local) |

"Production replay" is the home page replay above. "Local" means local tests and page replays. "Lab" means the container A/B runs above. "Whole round" means the number compares two builds that differ in more than this one change.

## 1. Constructor graphs

### The problem

Every HTTP request builds the constructor graph of the application service it calls, before the first line of the method runs. A transient service that is reached along ten different paths is built ten times. In a small application this is nothing. With hundreds of services that depend on each other, the cost multiplies.

When features are added quickly, the graph grows in a natural way:

- an application service injects another application service for one method,
- a registry takes `IEnumerable<IProvider>` and builds every provider in its constructor,
- a shared service (push notifications, code host providers, an agent tool catalog) is injected into many classes that use it on one path,
- something that runs for every `DbContext` or every save brings a bigger graph than it needs.

Each of these is a reasonable choice on its own. Together, one `IProjectRequestAppService` resolve had grown to hundreds of thousands of resolve requests, and resolving every [Volobox](https://volobox.io) component once came to tens of millions.

We found it from the runtime line: the GC pause stayed high while only a few requests were running. Allocation traces put most allocations under Autofac activation, and the component ranking gave the list of services and the edges that made them big.

### Resolve dependencies only when they are needed

A dependency that only some methods use becomes `Lazy<T>`. Autofac supports it out of the box, and ABP's `LazyServiceProvider` does the same for base class properties. The example uses small, illustrative types:

```csharp
public class ReportAppService : ApplicationService
{
    private readonly IReportRepository _reportRepository;
    private readonly Lazy<IReportExporter> _exporter;

    public ReportAppService(
        IReportRepository reportRepository,
        Lazy<IReportExporter> exporter)
    {
        _reportRepository = reportRepository;
        _exporter = exporter;
    }

    public virtual async Task<byte[]> ExportAsync(Guid id)
    {
        // Only this method pays for the exporter's graph.
        var report = await _reportRepository.GetAsync(id);
        return await _exporter.Value.ExportAsync(report);
    }
}
```

A registry takes `Lazy<IEnumerable<T>>` and builds its index on first use, or resolves one implementation by key with a keyed service. Enumerating all three code host providers (GitHub, GitLab, Azure DevOps) cost about 2,000 resolve requests per scope. Resolving only the one we need costs a few.

Two more costs came with `DbContext`, and we took a different path for each. In [Volobox](https://volobox.io)'s Identity Pro setup, the compiled-query cache key now comes from a small provider that reads the same values directly, which saved about 570 resolve requests for every `DbContext`. The first save on a `DbContext` built the audit store, even outside an audit scope. Now it is resolved only when a log is written, which saved about 120 resolve requests per `DbContext`.

Resolving every [Volobox](https://volobox.io) component once now takes about 95% fewer resolve requests. In the production replay, the home page data took about 90% less time (median of 10 rounds), the median now finishes in under half a second, and each of its seven groups of calls dropped by about 90%.

The production logs showed the same trend across all API requests. We compared the daytime hours before and after a later release in this round. In those hours, the median (p50) response time dropped by 30% to about 38 ms, and the 90th percentile (p90) dropped by 26%, while the request rate grew by 57%. Resolve requests per request were cut in half, and CPU per request dropped by about 30%. That release also carried other changes, so this is a whole-round number.

To keep it that way, we added a test that resolves the services we call most and fails when a graph grows past its budget:

```csharp
[Theory]
[InlineData(typeof(IProjectTaskAppService), 5_000)]
[InlineData(typeof(IConversationAppService), 10_000)]
[InlineData(typeof(IPullRequestAppService), 6_000)]
public void Resolving_a_busy_service_stays_within_its_graph_budget(Type serviceType, int budget)
{
    CountResolveRequests(serviceType).ShouldBeLessThanOrEqualTo(budget);
}
```

The budgets are well above today's graphs. If a test fails, we look for a new eager dependency that pulls in a large graph, and we catch the cost before it reaches production.

## 2. After-commit work

### The problem

[Volobox](https://volobox.io) leaves some work for after the commit, such as push notifications. A local event handler in ABP runs in its own short scope, which ends when the handler returns, and the unit of work commits later. So an after-commit callback must not rely on services from the handler's scope. This is easy to miss with `Lazy<T>`: on first access, it resolves its value from the scope that created the object holding it:

```csharp
public async Task HandleEventAsync(TaskAssignedEvent eventData)
{
    _unitOfWorkManager.Current.OnCompleted(async () =>
    {
        // Runs after the commit. This handler's scope may be disposed by now.
        await _pushNotifier.Value.SendAsync(eventData.AssigneeId);
    });
}
```

Our first answer was to keep each handler's scope alive until the unit of work ended. Each kept scope holds its own objects, and in a long operation that publishes many events, one request ended up holding thousands of live scopes. We found it by comparing heap dumps taken before and during a load: the number of `LifetimeScope` objects grew with the number of events the operation published.

### Carry values and open a fresh scope

Carry values, not services, and open a fresh scope when the work runs. The event and notifier types here are illustrative:

```csharp
public class AfterCommitScope : ISingletonDependency
{
    private readonly IServiceScopeFactory _scopeFactory;

    public AfterCommitScope(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task RunAsync<TState>(TState state, Func<IServiceProvider, TState, Task> work)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await work(scope.ServiceProvider, state);
    }
}

// In the handler: copy the values first, so the callback holds neither the handler nor the event.
var assigneeId = eventData.AssigneeId;
var afterCommitScope = _afterCommitScope;

_unitOfWorkManager.Current.OnCompleted(() => afterCommitScope.RunAsync(
    assigneeId,
    static (services, id) => services
        .GetRequiredService<PushNotifier>()
        .SendAsync(id)));
```

The new scope does not change the ambient unit of work or tenant. Work that writes to the database starts its own unit of work with `requiresNew: true`. If the handler switched tenants, carry that tenant in the state too.

In the lab A/B run of this round, a heap dump taken while previews were starting held 99% fewer live `LifetimeScope` objects. While all three previews were still running, the heap after a full GC peaked 22% lower.

## 3. Transactions

### The problem

Most [Volobox](https://volobox.io) requests are short API calls. Two kinds are not.

**SignalR hub connections.** [Volobox](https://volobox.io) keeps one SignalR connection per browser tab. In our HTTP/2 setup, a hub connection arrives as a `CONNECT` request that lasts as long as the page, and a transaction opened for it stays open just as long. We found one by listing open transactions during a browser test:

```sql
SELECT pid, state, now() - xact_start AS age, left(query, 80)
FROM pg_stat_activity
WHERE state = 'idle in transaction'
ORDER BY age DESC;
```

**Long-running requests.** Starting a preview environment for a coding session can take minutes, and most of that time is spent waiting for other systems. The request held its transaction the whole time, and a few steps did work that grew with the number of steps (a large setup log write, large file buffers). A local run kept more than one CPU core busy and ran dozens of gen 2 GCs a minute. Its request line showed a long request with a high resolve count, and a `gc-verbose` trace of the same run showed where it allocated.

### A transaction rule for each kind of request

We gave the hub routes an explicit non-transactional rule, separate from ordinary API requests. ABP lets an application replace its transaction behavior provider for this:

```csharp
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IUnitOfWorkTransactionBehaviourProvider), typeof(AspNetCoreUnitOfWorkTransactionBehaviourProvider))]
public class HubUnitOfWorkTransactionBehaviourProvider : AspNetCoreUnitOfWorkTransactionBehaviourProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HubUnitOfWorkTransactionBehaviourProvider(
        IHttpContextAccessor httpContextAccessor,
        IOptions<AspNetCoreUnitOfWorkTransactionBehaviourProviderOptions> options)
        : base(httpContextAccessor, options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override bool? IsTransactional =>
        _httpContextAccessor.HttpContext?.Request.Path.StartsWithSegments("/signalr-hubs") == true
            ? false
            : base.IsTransactional;
}
```

For the preview start, the application service method turns off its automatic unit of work and opens a separate, non-transactional one for the preview flow. A simplified version:

```csharp
[UnitOfWork(IsDisabled = true)]
public virtual async Task<PreviewDto> StartPreviewAsync(Guid sessionId)
{
    using var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
    var result = await StartPreviewCoreAsync(sessionId);
    await uow.CompleteAsync();
    return result;
}
```

The attribute alone only covers this method. The explicit `requiresNew` unit of work is what keeps the long flow out of the request's unit of work. Together with smaller setup log writes and file buffers, the peak CPU of the same local preview start fell by about 87% across that round, with almost no gen 2 GCs.

## 4. Background workers

### The problem

Background workers are easy to forget, because nobody waits for them. In a single process they share the CPU with requests.

Our pull request status worker runs every 15 minutes. It read the newest 200 merged pull requests, loaded their links one by one, and built its notifier and coordinator before it checked whether any task needed to change. Our queue workers sleep until something is written for them, but built their dependencies before they waited.

We found these by searching the worker log lines for runs with high resolve counts.

### Check for work before building anything

We changed the workers to check for work before building their services. The pull request status worker now asks the database one question first ("which merged pull requests still have open linked tasks?"), and builds the heavy services only when the answer is not empty. In a local run with a small test set, that cut its resolve requests by more than 99%. Our issue sync, with 10 issues, made 89% fewer.

The queue workers now wait for their signal first and build their dependencies after it. Across this round of background-work and dependency changes, CPU on an idle local host dropped by 40%.

## 5. Startup

### The problem

A fresh process has cold database connections and has not built its controllers yet, so the first users after a deploy pay for both. Right after a deploy, the runtime line showed many new database connections in the first minute, and the first call to each home page API took much longer than the second.

Startup itself was long too. An EventPipe CPU sampling trace of a starting host showed proxy type generation near the top. Castle DynamicProxy keeps generated classes in a dynamic module, and generating a new class gets slower as that module grows. Generating proxies in batches of 50 showed each batch getting slower than the one before, and [Volobox](https://volobox.io) has around a thousand proxy classes.

### Warm up before ready, and spread the proxies

- **Warm up before reporting ready.** Before the host tells the orchestrator it is ready, it opens a batch of database connections, runs the first queries and builds the controllers the home page needs. Locally, across that round of changes, the first calls to 30 home page APIs got about 97% faster (p50). The host reports ready a bit later, and these first-request costs move into startup.
- **Spread the dynamic proxies.** We spread the proxies over 16 dynamic modules. In the lab, this change alone made the host ready 57% sooner. With the rest of that round, startup got 62% faster and used 57% less CPU, in containers with the same limits.

## 6. The front end

### The problem

A HAR file of a page refresh showed six configuration and sign-in requests running one after another before anything appeared, one of them only to read the runtime configuration. A bundle report from the Vite build showed the first page loading several megabytes of statically imported JavaScript (gzip), and the import chain behind each large module.

### Draw the page first, and fetch less

- The host writes the runtime configuration into the HTML, so the page does not fetch it.
- The page draws the outline of the workbench (top bar, sidebars, home cards) in plain HTML before the React app starts.
- A refresh in the same signed-in tab reuses its valid access token, and the OIDC discovery document is cached, so the sign-in round trips are gone from a normal refresh.
- Pages, editors and language files load on demand. This step cut statically imported JavaScript by about 60% (gzip). The active language loads first, and the English fallback loads in the background.
- Home page data is kept in IndexedDB and shown at once, then refreshed from the server.

A Playwright script with a simulated network delay timed the first paint, the greeting and the first task card for each change. With a 300 ms simulated delay and gzip, a normal refresh now shows the home page in 0.8 seconds, about 70% faster.

## A short checklist

- Log a runtime line every minute, and put GC pause, thread pool queue and resolve count on every request line.
- Count Autofac resolve requests per request and per worker run. Rank components by graph size, and put a budget test on your most-used services.
- Inject only what a class uses. Make a dependency that only some methods use `Lazy<T>`.
- Do not inject `IEnumerable<T>` when a call needs only one implementation. Use keyed services, or `Lazy<IEnumerable<T>>` when only some calls need them all.
- Work left for after the commit carries values and opens its own scope.
- Check the unit of work and transaction behavior for long-lived connections and requests that take minutes.
- Make a background worker check whether there is work before it builds anything.
- Warm up database connections and the controllers the home page needs before reporting ready.

We used production logs to find the problems. The home page numbers come from replaying the home page's API calls against production before and after a release, and the all-API numbers come from production logs before and after a later release. The other numbers come from local and lab A/B runs, so they show the direction and the size of each change, not exact production values.

## About [Volobox](https://volobox.io)

In [Volobox](https://volobox.io), a team plans projects on boards and milestones and assigns tasks to people or agents. People chat, edit docs and sheets together, share files, hold meetings and sync their calendars. Support desks come with a portal where customers follow their tickets.

For coding work, an agent takes a task to a pull request while you watch the session live. Sessions run in isolated sandboxes or on your own machines. You can pick OpenCode, Cursor or Codex, or use Claude Code with your own subscription on your own machine.

Agents use skills, library packs, MCP and custom apps, and run in scheduled and triggered workflows. Sensitive actions wait on approval cards, budgets and rate limits keep usage in check, and the audit log can be exported.

[Volobox](https://volobox.io) Cloud is the default. Business plans can run on dedicated infrastructure, and Enterprise can run on a private cloud or on-premise, with model endpoints inside your own network.

[Volobox](https://volobox.io) is now generally available. Any team can request a demo at [volobox.io](https://volobox.io), get a workspace the same day and use it free for 30 days.
