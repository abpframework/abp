using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage
{
    public class GenericStoreProxy<TOptions> : IAbpStore<TOptions>
        where TOptions : class, IAbpStoreOptions, new()
    {
        private readonly IAbpStore _innerStore;

        public GenericStoreProxy(IAbpStorageFactory factory, IOptions<TOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), "Unable to build generic Store. Did you forget to configure your options?");
            }

            _innerStore = factory.GetStore(options.Value.Name, options.Value);
        }

        public string Name => _innerStore.Name;

        public Task InitAsync() => _innerStore.InitAsync();

        public Task DeleteAsync(IPrivateFileReference file) => _innerStore.DeleteAsync(file);

        public ValueTask<IFileReference> GetAsync(Uri file, bool withMetadata) => _innerStore.GetAsync(file, withMetadata);

        public ValueTask<IFileReference> GetAsync(IPrivateFileReference file, bool withMetadata) => _innerStore.GetAsync(file, withMetadata);

        public ValueTask<IFileReference[]> ListAsync(string path, bool recursive, bool withMetadata) => _innerStore.ListAsync(path, recursive, withMetadata);

        public ValueTask<IFileReference[]> ListAsync(string path, string searchPattern, bool recursive, bool withMetadata) => _innerStore.ListAsync(path, searchPattern, recursive, withMetadata);

        public ValueTask<byte[]> ReadAllBytesAsync(IPrivateFileReference file) => _innerStore.ReadAllBytesAsync(file);

        public ValueTask<string> ReadAllTextAsync(IPrivateFileReference file) => _innerStore.ReadAllTextAsync(file);

        public ValueTask<Stream> ReadAsync(IPrivateFileReference file) => _innerStore.ReadAsync(file);

        public ValueTask<IFileReference> SaveAsync(Stream data, IPrivateFileReference file, string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metadata = null) => _innerStore.SaveAsync(data, file, contentType, overwritePolicy);

        public ValueTask<IFileReference> SaveAsync(byte[] data, IPrivateFileReference file, string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metadata = null) => _innerStore.SaveAsync(data, file, contentType, overwritePolicy);

        public ValueTask<string> GetSharedAccessSignatureAsync(ISharedAccessPolicy policy) => _innerStore.GetSharedAccessSignatureAsync(policy);
    }
}
