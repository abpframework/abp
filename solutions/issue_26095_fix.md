**TL;DR**  
* Skip the lock/manifest read for entities that never touch the blob store.  
* Cache the “has‑file‑property” flag (the descriptor hash) in a local `ConcurrentDictionary` (or a distributed cache if you need cross‑process consistency).  
* Return the DTO that you already have after the mutation instead of doing a full `SingleEntityQuery`.  

Below are the minimal code changes that implement the three points above.  
All snippets are in C# and assume the existing Low‑Code architecture (ABP, EF Core, Redis lock, Azure Blob).

---

## 1.  Cache the “has‑file‑property” flag

```csharp
// File: DynamicEntityDescriptorCache.cs
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

public static class DynamicEntityDescriptorCache
{
    // key: descriptor hash, value: true if the descriptor contains a file/image property
    private static readonly ConcurrentDictionary<string, bool> _hasFilePropsCache
        = new ConcurrentDictionary<string, bool>();

    public static bool HasFileProps(string descriptorJson)
    {
        var hash = ComputeHash(descriptorJson);
        return _hasFilePropsCache.GetOrAdd(hash, _ => ContainsFileProps(descriptorJson));
    }

    private static string ComputeHash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    private static bool ContainsFileProps(string descriptorJson)
    {
        // Very light JSON scan – no full deserialization
        return descriptorJson.Contains("\"FileProperty\":") ||
               descriptorJson.Contains("\"ImageProperty\":");
    }
}
```

> **Why this works** –  
> The descriptor JSON is static for a given entity type.  
> Computing a SHA‑256 hash is cheap, and the `ConcurrentDictionary` guarantees thread‑safe reads/writes.  
> If you need cross‑process consistency, replace the dictionary with `IDistributedCache` and keep the same key.

---

## 2.  Skip lock & manifest read for “file‑free” entities

```csharp
// File: DynamicEntityMutationGate.cs
using Volo.Abp.Threading;

public class DynamicEntityMutationGate
{
    private readonly IDistributedLock _distributedLock;
    private readonly IEntityBlobCleanupManifestStore _manifestStore;
    private readonly IAbpDistributedLockProvider _lockProvider;

    public DynamicEntityMutationGate(
        IDistributedLock distributedLock,
        IEntityBlobCleanupManifestStore manifestStore,
        IAbpDistributedLockProvider lockProvider)
    {
        _distributedLock = distributedLock;
        _manifestStore   = manifestStore;
        _lockProvider    = lockProvider;
    }

    public async Task<T> ExecuteAsync<T>(string descriptorJson, Func<Task<T>> mutation)
    {
        // 1️⃣  Check if the descriptor has any file/image properties
        if (!DynamicEntityDescriptorCache.HasFileProps(descriptorJson))
        {
            // No blob work – just run the mutation
            return await mutation();
        }

        // 2️⃣  Normal path – read manifest & acquire lock
       