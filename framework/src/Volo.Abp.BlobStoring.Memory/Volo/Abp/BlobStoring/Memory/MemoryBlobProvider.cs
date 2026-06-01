using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.Memory;

public class MemoryBlobProvider : BlobProviderBase, ITransientDependency
{
    protected ConcurrentDictionary<string, byte[]> MemoryStore { get; }

    protected ICurrentTenant CurrentTenant  { get; }

    public MemoryBlobProvider(ICurrentTenant currentTenant)
    {
        MemoryStore = new ConcurrentDictionary<string, byte[]>();

        CurrentTenant = currentTenant;
    }

    public override async Task SaveAsync(BlobProviderSaveArgs args)
    {
        var cacheKey = GetCacheKey(args);

        using var buffer = new MemoryStream();
        await args.BlobStream.CopyToAsync(buffer);
        var bytes = buffer.ToArray();

        if (!args.OverrideExisting)
        {
            if (!MemoryStore.TryAdd(cacheKey, bytes))
            {
                throw new BlobAlreadyExistsException(
                    $"Saving BLOB '{args.BlobName}' does already exists in the container '{args.ContainerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }
        }
        else
        {
            MemoryStore.AddOrUpdate(cacheKey, bytes, (_, __) => bytes);
        }
    }

    public override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        return Task.FromResult(MemoryStore.TryRemove(GetCacheKey(args), out _));
    }

    public override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        return Task.FromResult(MemoryStore.ContainsKey(GetCacheKey(args)));
    }

    public override Task<Stream?> GetOrNullAsync(BlobProviderGetArgs args)
    {
        return MemoryStore.TryGetValue(GetCacheKey(args), out var bytes)
            ? Task.FromResult<Stream?>(new MemoryStream(bytes, writable: false))
            : Task.FromResult<Stream?>(null);
    }

    protected virtual string GetCacheKey(BlobProviderArgs args)
    {
        return $"{CurrentTenant.Id}_{args.BlobName}_{args.ContainerName}";
    }
}
