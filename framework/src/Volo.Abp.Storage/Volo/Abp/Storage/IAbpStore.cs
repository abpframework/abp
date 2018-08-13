using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage
{
    public interface IAbpStore
    {
        string Name { get; }

        Task InitAsync();

        ValueTask<IBlobReference> SaveBlobAsync(byte[] source, IPrivateBlobReference file, string contentType,
            OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metasource = null);

        ValueTask<IBlobReference> SaveBlobAsync(Stream source, IPrivateBlobReference file, string contentType,
            OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metasource = null);

        ValueTask<IBlobReference[]> ListBlobAsync(string path, bool recursive, bool withMetadata);

        ValueTask<IBlobReference[]> ListBlobAsync(string path, string searchPattern, bool recursive,
            bool withMetadata);

        Task DeleteBlobAsync(IPrivateBlobReference file);

        Task CopyBlobAsync(string sourceContainerName, string sourceBlobName, string destinationContainerName,
            string destinationBlobName = null);

        ValueTask<IBlobReference> GetBlobAsync(IPrivateBlobReference file, bool withMetadata);

        ValueTask<IBlobReference> GetBlobAsync(Uri file, bool withMetadata);

        ValueTask<Stream> ReadBlobAsync(IPrivateBlobReference file);

        ValueTask<byte[]> ReadBlobBytesAsync(IPrivateBlobReference file);

        ValueTask<string> ReadBlobTextAsync(IPrivateBlobReference file);

        ValueTask<string> GetBlobSasUrlAsync(ISharedAccessPolicy policy);
    }

    public interface IAbpStore<TOptions> : IAbpStore
        where TOptions : class, IAbpStoreOptions, new()
    {
    }
}