using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage
{
    [Dependency(ServiceLifetime.Transient, TryRegister = true)]
    public class AbpStoreProxy<TOptions> : IAbpStoreWithOptions<TOptions>
        where TOptions : class, IAbpStoreOptions, new()
    {
        private readonly IAbpStore _innerStore;

        public AbpStoreProxy(IAbpStorageFactory factory, IOptions<TOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options),
                    "Unable to build generic Store. Did you forget to configure your options?");

            _innerStore = factory.GetStore(options.Value.Name, options.Value);
        }

        public string Name => _innerStore.Name;

        public Task InitAsync()
        {
            return _innerStore.InitAsync();
        }

        public Task DeleteAsync(IPrivateFileReference file)
        {
            return _innerStore.DeleteAsync(file);
        }

        public ValueTask<IFileReference> GetAsync(Uri file, bool withMetadata)
        {
            return _innerStore.GetAsync(file, withMetadata);
        }

        public ValueTask<IFileReference> GetAsync(IPrivateFileReference file, bool withMetadata)
        {
            return _innerStore.GetAsync(file, withMetadata);
        }

        public ValueTask<IFileReference[]> ListAsync(string path, bool recursive, bool withMetadata)
        {
            return _innerStore.ListAsync(path, recursive, withMetadata);
        }

        public ValueTask<IFileReference[]> ListAsync(string path, string searchPattern, bool recursive,
            bool withMetadata)
        {
            return _innerStore.ListAsync(path, searchPattern, recursive, withMetadata);
        }

        public ValueTask<byte[]> ReadAllBytesAsync(IPrivateFileReference file)
        {
            return _innerStore.ReadAllBytesAsync(file);
        }

        public ValueTask<string> ReadAllTextAsync(IPrivateFileReference file)
        {
            return _innerStore.ReadAllTextAsync(file);
        }

        public ValueTask<Stream> ReadAsync(IPrivateFileReference file)
        {
            return _innerStore.ReadAsync(file);
        }

        public ValueTask<IFileReference> SaveAsync(Stream data, IPrivateFileReference file, string contentType,
            OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metadata = null)
        {
            return _innerStore.SaveAsync(data, file, contentType, overwritePolicy);
        }

        public ValueTask<IFileReference> SaveAsync(byte[] data, IPrivateFileReference file, string contentType,
            OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metadata = null)
        {
            return _innerStore.SaveAsync(data, file, contentType, overwritePolicy);
        }

        public ValueTask<string> GetSharedAccessSignatureAsync(ISharedAccessPolicy policy)
        {
            return _innerStore.GetSharedAccessSignatureAsync(policy);
        }
    }
}