using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.Internal
{
    public class GenericStoreProxy<TOptions> : IAbpStore<TOptions>, ITransientDependency
        where TOptions : class, IAbpStoreOptions, new()
    {
        private readonly IAbpStore _abpStore;

        public GenericStoreProxy(IAbpStorageFactory factory, IOptions<TOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options),
                    "Unable to build generic Store. Did you forget to configure your options?");

            _abpStore = factory.GetStore(options.Value.Name, options.Value);
        }

        public string Name => _abpStore.Name;

        public Task InitAsync()
        {
            return _abpStore.InitAsync();
        }

        public Task DeleteBlobAsync(IPrivateBlobReference file)
        {
            return _abpStore.DeleteBlobAsync(file);
        }

        public Task CopyBlobAsync(string sourceContainerName, string sourceBlobName, string destinationContainerName,
            string destinationBlobName = null)
        {
            return _abpStore.CopyBlobAsync(sourceContainerName, sourceBlobName, destinationContainerName,
                destinationBlobName);
        }

        public ValueTask<IBlobReference> GetBlobAsync(Uri file, bool withMetadata)
        {
            return _abpStore.GetBlobAsync(file, withMetadata);
        }

        public ValueTask<IBlobReference> GetBlobAsync(IPrivateBlobReference file, bool withMetadata)
        {
            return _abpStore.GetBlobAsync(file, withMetadata);
        }

        public ValueTask<IBlobReference[]> ListBlobAsync(string path, bool recursive, bool withMetadata)
        {
            return _abpStore.ListBlobAsync(path, recursive, withMetadata);
        }

        public ValueTask<IBlobReference[]> ListBlobAsync(string path, string searchPattern, bool recursive,
            bool withMetadata)
        {
            return _abpStore.ListBlobAsync(path, searchPattern, recursive, withMetadata);
        }

        public ValueTask<byte[]> ReadBlobBytesAsync(IPrivateBlobReference file)
        {
            return _abpStore.ReadBlobBytesAsync(file);
        }

        public ValueTask<string> ReadBlobTextAsync(IPrivateBlobReference file)
        {
            return _abpStore.ReadBlobTextAsync(file);
        }

        public ValueTask<Stream> ReadBlobAsync(IPrivateBlobReference file)
        {
            return _abpStore.ReadBlobAsync(file);
        }

        public ValueTask<IBlobReference> SaveBlobAsync(Stream data, IPrivateBlobReference file, string contentType,
            OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metadata = null)
        {
            return _abpStore.SaveBlobAsync(data, file, contentType, overwritePolicy);
        }

        public ValueTask<IBlobReference> SaveBlobAsync(byte[] data, IPrivateBlobReference file, string contentType,
            OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metadata = null)
        {
            return _abpStore.SaveBlobAsync(data, file, contentType, overwritePolicy);
        }

        public ValueTask<string> GetBlobSasUrlAsync(ISharedAccessPolicy policy)
        {
            return _abpStore.GetBlobSasUrlAsync(policy);
        }
    }
}