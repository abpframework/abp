# Why Do You Need Distributed Locking in ASP.NET Core

## Introduction

In modern distributed systems, synchronizing access to common resources among numerous instances is a critical problem. Whenever lots of servers or processes concurrently attempt to update the same resource simultaneously, race conditions can lead to data corruption, redundant work, and inconsistent state. Throughout the implementation of the ABP framework, we encountered and overcame this exact same problem with assistance from a stable distributed locking mechanism. In this post, we will present our experience and learnings when implementing this solution, so you can understand when and why you would need distributed locking in your ASP.NET Core applications.

## Problem

Suppose you are running an e-commerce application deployed on multiple servers for high availability. A customer places an order, which kicks off a background job that reserves inventory and charges payment. If not properly synchronized, the following is what can happen:

### Race Conditions in Multi-Instance Deployments

When your ASP.NET Core application is scaled horizontally with multiple instances, each instance works independently. If two instances simultaneously perform the same operation—like deducting inventory, generating invoice numbers, or processing a refund—you can end up with:

- **Duplicate operations**: The same payment processed twice
- **Data inconsistency**: Inventory count becomes negative or incorrect
- **Lost updates**: One instance's changes overwrite another's
- **Sequential ID conflicts**: Two instances generate the same invoice number

### Background Job Processing

Background work libraries like Quartz.NET or Hangfire usually run on multiple workers. Without distributed locking:

- Multiple workers can choose the same task
- Long-running processes can be executed parallel when they should be executed in a sequence
- Jobs that depend on exclusive resource access can corrupt shared data

### Cache Invalidation and Refresh

When distributed caching is employed, there can be multiple instances that simultaneously identify a cache miss and attempt to rebuild the cache, leading to:

- High database load owing to concurrent rebuild cache requests
- Race conditions under which older data overrides newer data
- wasted computational resources

### Rate Limiting and Throttling

Enforcing rate limits across multiple instances of the application requires coordination. If there is no distributed locking, each instance has its own limits, and global rate limits cannot be enforced properly.

The root issue is simple: **the default C# locking APIs (lock, SemaphoreSlim, Monitor) work within a process in isolation**. They will not assist with distributed cases where coordination must take place across servers, containers, or cloud instances.

## Solutions

Several approaches exist for implementing distributed locking in ASP.NET Core applications. Let's explore the most common solutions, their trade-offs, and why we chose our approach for ABP.

### 1. Database-Based Locking

Using your existing database to place locks by inserting or updating rows with distinctive values.

**Pros:**
- No additional infrastructure required
- Works with any relational database
- Transactions provide ACID guarantees

**Cons:**
- Database round-trip performance overhead
- Can lead to database contention under high load
- Must be controlled to prevent orphaned locks
- Not suited for high-frequency locking scenarios

**When to use:** Small-scale applications where you do not wish to add additional infrastructure, and lock operations are low frequency.

### 2. Redis-Based Locking

Redis has atomic operations that make it excellent at distributed locking, using commands such as `SET NX` (set if not exists) with expiration.
**Pros:**

- Low latency and high performance
- Expiration prevents lost locks built-in
- Well-established with tested patterns (Redlock algorithm)
- Works well for high-throughput use cases
**Cons:**

- Requires Redis infrastructure
- Network partitions might be an issue
- One Redis instance is a single point of failure (although Redis Cluster reduces it)
**Resources:**

- [Redis Distributed Locks Documentation](https://redis.io/docs/manual/patterns/distributed-locks/)
- [Redlock Algorithm](https://redis.io/topics/distlock)
**When to use:** Production applications with multiple instances where performance is critical, especially if you are already using Redis as a caching layer.

### 3. Azure Blob Storage Leases

Azure Blob Storage offers lease functionality which can be utilized for distributed locks.

**Pros:**
- Part of Azure, no extra infrastructure
- Lease expiration automatically
- Low-frequency locks are economically viable

**Cons:**
- Azure-specific, not portable
- Latency greater than Redis
- Azure cloud-only projects

**When to use:** Azure-native applications with low-locking frequency where you need to minimize moving parts.

### 4. etcd or ZooKeeper

Distributed coordination services designed from scratch to accommodate consensus and locking.

**Pros:**
- Designed for distributed coordination
- Strong consistency guaranteed
- Robust against network partitions

**Cons:**
- Difficulty in setting up the infrastructure
- Excess baggage for most applications
- Steep learning curve

**Use when:** Large distributed systems with complex coordination require more than basic locking.


### Our Choice: Abstraction with Multiple Implementations

For ABP, we chose to use an **abstraction layer** with support for multibackend. This provides flexibility to the developers so that they can choose the best implementation depending on their infrastructure. Our default implementations include support for:

- **Redis** (recommended for most scenarios)
- **Database-based locking** (for less complicated configurations)
- In-memory single-instance and development locks

We started with Redis because it offers the best tradeoff between ease of operation, reliability, and performance for distributed cases. But abstraction prevents applications from becoming technology-dependent, and it's easier to start simple and expand as needed.

## Implementation

Let's implement a simplified distributed locking mechanism using Redis and StackExchange.Redis. This example shows the core concepts without ABP's framework complexity.

First, install the required package:

```bash
dotnet add package StackExchange.Redis
```

Here's a basic distributed lock implementation:

```csharp
public interface IDistributedLock
{
    Task<IDisposable?> TryAcquireAsync(
        string resource,
        TimeSpan expirationTime,
        CancellationToken cancellationToken = default);
}

public class RedisDistributedLock : IDistributedLock
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<RedisDistributedLock> _logger;

    public RedisDistributedLock(
        IConnectionMultiplexer redis,
        ILogger<RedisDistributedLock> logger)
    {
        _redis = redis;
        _logger = logger;
    }

    public async Task<IDisposable?> TryAcquireAsync(
        string resource,
        TimeSpan expirationTime,
        CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();
        var lockKey = $"lock:{resource}";
        var lockValue = Guid.NewGuid().ToString();

        // Try to acquire the lock using SET NX with expiration
        var acquired = await db.StringSetAsync(
            lockKey,
            lockValue,
            expirationTime,
            When.NotExists);

        if (!acquired)
        {
            _logger.LogDebug(
                "Failed to acquire lock for resource: {Resource}",
                resource);
            return null;
        }

        _logger.LogDebug(
            "Lock acquired for resource: {Resource}",
            resource);

        return new RedisLockHandle(db, lockKey, lockValue, _logger);
    }

    private class RedisLockHandle : IDisposable
    {
        private readonly IDatabase _db;
        private readonly string _lockKey;
        private readonly string _lockValue;
        private readonly ILogger _logger;
        private bool _disposed;

        public RedisLockHandle(
            IDatabase db,
            string lockKey,
            string lockValue,
            ILogger logger)
        {
            _db = db;
            _lockKey = lockKey;
            _lockValue = lockValue;
            _logger = logger;
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                // Only delete if we still own the lock
                var script = @"
                    if redis.call('get', KEYS[1]) == ARGV[1] then
                        return redis.call('del', KEYS[1])
                    else
                        return 0
                    end";

                _db.ScriptEvaluate(
                    script,
                    new RedisKey[] { _lockKey },
                    new RedisValue[] { _lockValue });

                _logger.LogDebug("Lock released for key: {LockKey}", _lockKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error releasing lock for key: {LockKey}",
                    _lockKey);
            }
            finally
            {
                _disposed = true;
            }
        }
    }
}
```

Register the service in your `Program.cs`:

```csharp
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse("localhost:6379");
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddSingleton<IDistributedLock, RedisDistributedLock>();
```

Now you can use distributed locking in your services:

```csharp
public class OrderService
{
    private readonly IDistributedLock _distributedLock;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IDistributedLock distributedLock,
        ILogger<OrderService> logger)
    {
        _distributedLock = distributedLock;
        _logger = logger;
    }

    public async Task ProcessOrderAsync(string orderId)
    {
        var lockResource = $"order:{orderId}";
        
        // Try to acquire the lock with 30-second expiration
        await using var lockHandle = await _distributedLock.TryAcquireAsync(
            lockResource,
            TimeSpan.FromSeconds(30));

        if (lockHandle == null)
        {
            _logger.LogWarning(
                "Could not acquire lock for order {OrderId}. " +
                "Another process might be processing it.",
                orderId);
            return;
        }

        // Critical section - only one instance will execute this
        _logger.LogInformation("Processing order {OrderId}", orderId);
        
        // Your order processing logic here
        await Task.Delay(1000); // Simulating work
        
        _logger.LogInformation(
            "Order {OrderId} processed successfully",
            orderId);
        
        // Lock is automatically released when lockHandle is disposed
    }
}
```

### Key Implementation Details

**Lock Key Uniqueness**: Use hierarchical, descriptive keys (`order:12345`, `inventory:product-456`) to avoid collisions.

**Lock Value**: We use a single distinct GUID as the lock value. This ensures only the lock owner can release it, excluding unintentional deletion by expired locks or other operations.

**Automatic Expiration**: Always provide an expiration time to prevent deadlocks when a process halts with an outstanding lock.

**Lua Script for Release**: Releasing uses a Lua script to atomically check ownership and delete the key. This prevents releasing a lock that has already timed out and is reacquired by another process.

**Disposal Pattern**: With `IDisposable` and `await using`, one ensures that the lock is released regardless of the exception that occurs.

### Handling Lock Acquisition Failures

Depending on your use case, you have several options when lock acquisition fails:

```csharp
// Option 1: Return early (shown above)
if (lockHandle == null)
{
    return;
}

// Option 2: Retry with timeout
var retryCount = 0;
var maxRetries = 3;
IDisposable? lockHandle = null;

while (lockHandle == null && retryCount < maxRetries)
{
    lockHandle = await _distributedLock.TryAcquireAsync(
        lockResource,
        TimeSpan.FromSeconds(30));
    
    if (lockHandle == null)
    {
        retryCount++;
        await Task.Delay(TimeSpan.FromMilliseconds(100 * retryCount));
    }
}

if (lockHandle == null)
{
    throw new InvalidOperationException("Could not acquire lock after retries");
}

// Option 3: Queue for later processing
if (lockHandle == null)
{
    await _queueService.EnqueueForLaterAsync(orderId);
    return;
}
```

This is a good foundation for distributed locking in ASP.NET Core applications. It addresses the most common scenarios and edge cases, but production can call for more sophisticated features like lock re-renewal for long-running operations or more sophisticated retry logic.

## Conclusion

Distributed locking is a necessity for data consistency and prevention of race conditions in new, scalable ASP.NET Core applications. As we've discussed, the problem becomes unavoidable as soon as you move beyond single-instance deployments to horizontally scaled multi-server, container, or background job worker deployments.

We examined several of them, from database-level locks to Redis, Azure Blob Storage leases, and coordination services. Each has its place, but Redis-based locking offers the best balance of performance, reliability, and ease in most situations. The example implementation we provided shows how to implement a well-crafted distributed locking mechanism with minimal dependence on other libraries.

Whether you implement your own solution or utilize a framework like ABP, familiarity with the concepts of distributed locking will help you build more stable and scalable applications. We hope by sharing our experience, we can keep you from falling into typical pitfalls and have distributed locking properly implemented on your own projects.