#nullable enable
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// A real in-memory provider (not a substitute), so tests can inspect the raw stored bytes.
/// </summary>
public class FakeInMemoryBlobProvider : BlobProviderBase
{
    private readonly ConcurrentDictionary<string, byte[]> _blobs = new ConcurrentDictionary<string, byte[]>();

    public override async Task SaveAsync(BlobProviderSaveArgs args)
    {
        var key = GetKey(args.ContainerName, args.BlobName);

        if (!args.OverrideExisting && _blobs.ContainsKey(key))
        {
            throw new BlobAlreadyExistsException(
                $"Saving BLOB '{args.BlobName}' does already exists in the container '{args.ContainerName}'!");
        }

        using (var memoryStream = new MemoryStream())
        {
            await args.BlobStream.CopyToAsync(memoryStream);
            _blobs[key] = memoryStream.ToArray();
        }
    }

    public override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        return Task.FromResult(_blobs.TryRemove(GetKey(args.ContainerName, args.BlobName), out _));
    }

    public override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        return Task.FromResult(_blobs.ContainsKey(GetKey(args.ContainerName, args.BlobName)));
    }

    public override Task<Stream?> GetOrNullAsync(BlobProviderGetArgs args)
    {
        return Task.FromResult<Stream?>(
            _blobs.TryGetValue(GetKey(args.ContainerName, args.BlobName), out var bytes)
                ? new MemoryStream(bytes)
                : null
        );
    }

    public byte[]? GetRawBytesOrNull(string containerName, string blobName)
    {
        return _blobs.TryGetValue(GetKey(containerName, blobName), out var bytes) ? bytes : null;
    }

    public void SetRawBytes(string containerName, string blobName, byte[] bytes)
    {
        _blobs[GetKey(containerName, blobName)] = bytes;
    }

    private static string GetKey(string containerName, string blobName)
    {
        return containerName + "/" + blobName;
    }
}
