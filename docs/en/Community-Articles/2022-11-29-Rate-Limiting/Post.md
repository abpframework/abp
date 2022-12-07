# Rate Limiting with ASP.NET Core 7.0

Rate limiting is a way of controlling the traffic that a web application or API receives. In other words, rate limiting helps you control the amount of traffic each user has access to at any given time. This is extremely useful when you want to manage the load on your server or services, avoid going over your monthly data transfer limit and allow the system to continue to function and meet service level agreements, even when an increase in demand places an extreme load on resources. 

In this article, we will look at what rate limiting is, why we need to use it, how the different rate limiting algorithms provided with .NET 7.0 work, and best practices for using rate limiting in your application.

## What is rate limiting?

Whether accidental or intentional, users may exhaust resources in a way that impacts others. When a number of requests are received on to resources for a long time, the server can run out of those resources. These resources can include memory, threads, connections, or anything else that is limited. To avoid this situation, set rate limiters. Rate limiters control the consumption of resources used by an instance of an application, a user, an individual tenant, or an entire service.

## Why do you need to use rate limiting?

A rate limiting system is crucial in any application where you have to control or throttle user requests or traffic. This is especially true in applications running on a cloud hosting platform because the user’s traffic can affect the whole server where the application is hosted. 

Why do you need to implement rate limiting? Here are a few reasons: 

- To ensure that a system continues to meet service level agreements (SLA).
- To prevent a single user, tenant, service, or so on from monopolizing the resources provided by an application.
- To help cost-optimize a system by limiting the maximum resource levels needed to keep it functioning.

## Rate limiter algorithms

The [`RateLimiterOptionsExtensions`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.ratelimiting.ratelimiteroptionsextensions) class provides the following extension methods for rate limiting:

- **[Fixed window](https://devblogs.microsoft.com/dotnet/announcing-rate-limiting-for-dotnet/#fixed-window-limit)**: Fixed-window limits—such as 3,000 requests per hour or 10 requests per day—are easy to state, but they are subject to spikes at the edges of the window, as available quota resets. Consider, for example, a limit of 3,000 requests per hour, which still allows for a spike of all 3,000 requests to be made in the first minute of the hour, which might overwhelm the service.
- [**Sliding window**:](https://devblogs.microsoft.com/dotnet/announcing-rate-limiting-for-dotnet/#sliding-window-limit) Sliding windows have the benefits of a fixed window, but the rolling window of time smoothes out bursts. Systems such as Redis facilitate this technique with expiring keys.
- [**Token bucket**](https://devblogs.microsoft.com/dotnet/announcing-rate-limiting-for-dotnet/#token-bucket-limit): A token bucket maintains a rolling and accumulating budget of usage as a balance of tokens. A token bucket adds tokens at some rate. When a service request is made, the service attempts to withdraw a token (decrementing the token count) to fulfill the request. If there are no tokens in the bucket, the service has reached its limit and responds with backpressure.
- [**Concurrency**](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?preserve-view=true&view=aspnetcore-7.0#concurrency-limiter): A concurrency limiter is the simplest form of rate limiting. It doesn’t look at time, just at number of concurrent requests. 

In order to be a more realistic example, instead of making an example with each rate limiter algorithm, we will implement the following three algorithms in an **ABP-based** application.

1. We will add a `SlidingWindowLimiter` with a partition for all anonymous users.
2. We will add a `TokenBucketRateLimiter` with a partition for each authenticated user.
3. We will add a `ConcurrencyLimiter` with a partition for each Tenant.

**Note:** The following sample isn't meant for production code but is an example of how to use the limiters in ABP-based applications.

### Limiter with `OnRejected`, `RetryAfter`, and `GlobalLimiter`

#### Add rate limiter

Let's create the following method in the `MyProjectNameWebModule.cs` class in the `MyProjectName.Web` project.

**Note:** If the `**.Web` project is not in your application, you can do the same in the project where your application is hosted.

```csharp
private void ConfigureRateLimiters(ServiceConfigurationContext context)
{
    context.Services.AddRateLimiter(limiterOptions =>
    {
        limiterOptions.OnRejected = (context, cancellationToken) =>
        {
            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                context.HttpContext.Response.Headers.RetryAfter =
                    ((int) retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
            }

            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.HttpContext.RequestServices.GetService<ILoggerFactory>()?
                .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
                .LogWarning("OnRejected: {RequestPath}", context.HttpContext.Request.Path);

            return new ValueTask();
        };

        limiterOptions.AddPolicy("UserBasedRateLimiting", context =>
        {
            var currentUser = context.RequestServices.GetService<ICurrentUser>();
            
            if (currentUser is not null && currentUser.IsAuthenticated)
            {
                return RateLimitPartition.GetTokenBucketLimiter(currentUser.UserName, _ => new TokenBucketRateLimiterOptions
                {
                    TokenLimit = 10,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 3,
                    ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                    TokensPerPeriod = 4,
                    AutoReplenishment = true
                });
            }

            return RateLimitPartition.GetSlidingWindowLimiter("anonymous-user",
                _ => new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = 2,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 1,
                    Window = TimeSpan.FromMinutes(1),
                    SegmentsPerWindow = 2
                });
        });

        limiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        {
            var currentTenant = context.RequestServices.GetService<ICurrentTenant>();
            
            if (currentTenant is not null && currentTenant.IsAvailable)
            {
                return RateLimitPartition.GetConcurrencyLimiter(currentTenant!.Name, _ => new ConcurrencyLimiterOptions
                {
                    PermitLimit = 5,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 1
                });
            }

            return RateLimitPartition.GetNoLimiter("host");
        });
    });
}
```

In the above example, the `TokenBucketLimiter` is used for each authenticated user, while the `SlidingWindowLimiter` is used for all anonymous users. Additionally, as a global limiter, the `ConcurrencyLimiter` is used for each tenant, while rate limiting is disabled for the host(tenant is not available). Also, for requests that are rejected when the limit is reached, sets the response status code to [429 Too Many Requests](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/429) and the response mentions when to retry (if available from the rate-limiting metadata).

Let's call the `ConfigureRateLimiters` method that we created in the `ConfigureServices` method.

The final version of the `ConfigureServices` method:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var hostingEnvironment = context.Services.GetHostingEnvironment();
    var configuration = context.Services.GetConfiguration();

    ConfigureBundles();
    ConfigureUrls(configuration);
    ConfigurePages(configuration);
    ConfigureAuthentication(context);
    ConfigureImpersonation(context, configuration);
    ConfigureAutoMapper();
    ConfigureVirtualFileSystem(hostingEnvironment);
    ConfigureNavigationServices();
    ConfigureAutoApiControllers();
    ConfigureSwaggerServices(context.Services);
    ConfigureExternalProviders(context);
    ConfigureHealthChecks(context);
    ConfigureCookieConsent(context);
    ConfigureTheme();

    Configure<PermissionManagementOptions>(options =>
    {
        options.IsDynamicPermissionStoreEnabled = true;
    });
    
    ConfigureRateLimiters(context); // added
}
```

#### Add RateLimiter middleware

Add the following line just before the `app.UseConfiguredEndpoints(...)` line to add the `RateLimiter` middleware to your ASP.NET Core request pipeline:

```csharp
app.UseRateLimiter();
```

#### Use rate limiter for all controllers

Let's edit the `ConfiguredEndpoints` middleware as follows:

```csharp
app.UseConfiguredEndpoints(endpoints =>
{
    endpoints.MapRazorPages()
        .DisableRateLimiting();

    endpoints.MapControllers()
        .RequireRateLimiting("UserBasedRateLimiting");
});
```

- **DisableRateLimiting:** It is used to disable the `ConcurrencyLimiter` for razor pages, which we set globally when the tenant is available.
- **RequireRateLimiting:** We have enabled the rate limiter, which we define according to whether the user is authenticated or not, for all controllers.

## `EnableRateLimiting` and `DisableRateLimiting` attributes

It's kind of unrealistic to always use rate limiting for all controllers or pages. Sometimes, we may want to throttle a particular endpoint or page. In such cases, we can use the `EnableRateLimiting` and `DisableRateLimiting` attributes. The `EnableRateLimiting` and `DisableRateLimiting` attributes can be applied to a controller, action method, or razor rage. Check [here](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?preserve-view=true&view=aspnetcore-7.0#enableratelimiting-and-disableratelimiting-attributes) for more.

## Rate limit an HTTP handler

Rate limiting when sending an HTTP request can be a good practice, especially in service-to-service communication. Because, resources are consumed by apps that rely on them, and when an app makes too many requests for a single resource, it can lead to *resource contention*. Resource contention occurs when a resource is consumed by too many clients, and the resource is unable to serve all of the apps that are requesting it. This can result in a poor user experience, and in some cases, it can even lead to a denial of service (DoS) attack. Since there are similar codes, I will not mention an example in this article, but to avoid such situations, you can write your own HTTP handler as [here](https://learn.microsoft.com/en-us/dotnet/core/extensions/http-ratelimiter#implement-a-delegatinghandler-subclass).

## How does it work?

[System.Threading.RateLimiting](https://www.nuget.org/packages/System.Threading.RateLimiting) provides the primitives for writing rate limiters as well as providing a few commonly used algorithms built-in. The main type is the abstract base class [RateLimiter](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Threading.RateLimiting/src/System/Threading/RateLimiting/RateLimiter.cs).

```csharp
public abstract class RateLimiter : IAsyncDisposable, IDisposable
{
    public abstract int GetAvailablePermits();
    public abstract TimeSpan? IdleDuration { get; }

    public RateLimitLease Acquire(int permitCount = 1);
    public ValueTask<RateLimitLease> WaitAsync(int permitCount = 1, CancellationToken cancellationToken = default);

    public void Dispose();
    public ValueTask DisposeAsync();
}
```

`RateLimiter` contains `Acquire` and `WaitAsync` as the core methods for trying to gain permits for a resource that is being protected. Depending on the application, the protected resource may need to acquire more than 1 permits, so `Acquire` and `WaitAsync` both accept an optional `permitCount` parameter. `Acquire` is a synchronous method that will check if enough permits are available or not and return a `RateLimitLease` which contains information about whether you successfully acquired the permits or not. `WaitAsync` is similar to `Acquire` except that it can support queuing permit requests which can be de-queued at some point in the future when the permits become available, which is why it’s asynchronous and accepts an optional `CancellationToken` to allow canceling the queued request.

`RateLimitLease` has an `IsAcquired` property which is used to see if the permits were acquired. Additionally, the `RateLimitLease` may contain metadata such as a suggested retry-after period if the lease failed. Finally, the `RateLimitLease` is disposable and should be disposed when the code is done using the protected resource. The disposal will let the `RateLimiter` know to update its limits based on how many permits were acquired.

## Limitations

In most cases, the rate-limiting middleware provided with ASP.NET 7.0 will meet your requirements. However, if you would want to return statistics about your limits (e.g. [the way GitHub does](https://docs.github.com/en/rest/overview/resources-in-the-rest-api?apiVersion=2022-11-28#rate-limit-http-headers)), you’ll find out that the ASP.NET rate limiting middleware does not support this. You won’t have access to the “number of requests remaining” or other metadata. Not in `OnRejected`, and definitely not if you want to return this data as headers on every request.

## Best practices for rate limiting

In order to use rate limiting properly, you need to have a solid understanding of the types of limiting available, as well as the data rate and data volume of your service. You also need to have a clear idea of how many users you expect to use your service as well as how they will interact with it. The best practices for rate limiting are as follows: 
- Find the right rate limiter algorithm for your endpoint. I mean, the cost of an endpoint should be considered when selecting a limiter. The cost of an endpoint includes the resources used, for example, time, data access, CPU, and I/O.
- Set realistic limits. Once you’ve figured out all the above, you need to set realistic limits for each service. Then, before deploying an app using rate limiting to production, stress test the app to validate the rate limiters and options used. For example, create a [JMeter script](https://jmeter.apache.org/usermanual/jmeter_proxy_step_by_step.html) with a tool like [BlazeMeter](https://guide.blazemeter.com/hc/articles/207421695-Writing-your-first-JMeter-script) or [Apache JMeter HTTP(S) Test Script Recorder](https://jmeter.apache.org/usermanual/jmeter_proxy_step_by_step.html) and load the script to [Azure Load Testing](https://learn.microsoft.com/en-us/azure/load-testing/overview-what-is-azure-load-testing).
- In response to rate-limiting, intermittent, or non-specific errors, a client should generally retry the request after a delay. It is a best practice for this delay to increase exponentially after each failed request, which is referred to as *exponential backoff*. When many clients might be making schedule-based requests (such as fetching results every hour), additional random time (*jitter*) should be applied to the request timing, the backoff period, or both of them to ensure that these multiple client instances don't become periodic [thundering herd](https://www.wikiwand.com/en/Thundering_herd_problem), and cause a form of DDoS themselves.

## Conclusion

In this article, we’ve covered what rate limiting is, why you need to use it and the best practices for doing so. We’ve also looked at how to use three rate-limiting algorithms that are provided with .NET 7.0 on ABP-based applications and how rate-limiting works. Now that you’re familiar with the concept of rate limiting, it’s time to start implementing rate limiting in your application. This will allow you to control the traffic and ensure that your application is running smoothly without any issues.

## References

- https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?preserve-view=true&view=aspnetcore-7.0
- https://aws.amazon.com/builders-library/timeouts-retries-and-backoff-with-jitter/
- https://blog.maartenballiauw.be/post/2022/09/26/aspnet-core-rate-limiting-middleware.html
- https://learn.microsoft.com/en-us/dotnet/core/extensions/http-ratelimiter
- https://learn.microsoft.com/en-us/azure/architecture/patterns/rate-limiting-pattern
- https://learn.microsoft.com/en-us/azure/architecture/patterns/throttling
- https://devblogs.microsoft.com/dotnet/announcing-rate-limiting-for-dotnet
- https://cloud.google.com/architecture/rate-limiting-strategies-techniques
