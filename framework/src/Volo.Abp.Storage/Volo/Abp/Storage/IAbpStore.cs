using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage
{
    public interface IAbpStore<TOptions> : IAbpStore
        where TOptions : class, IAbpStoreOptions, new()
    {
    }
    
    public interface IAbpStore
    {
        string Name { get; }

        Task InitAsync();

        ValueTask<IFileReference[]> ListAsync(string path, bool recursive, bool withMetadata);

        ValueTask<IFileReference[]> ListAsync(string path, string searchPattern, bool recursive, bool withMetadata);

        ValueTask<IFileReference> GetAsync(IPrivateFileReference file, bool withMetadata);

        ValueTask<IFileReference> GetAsync(Uri file, bool withMetadata);

        Task DeleteAsync(IPrivateFileReference file);

        ValueTask<Stream> ReadAsync(IPrivateFileReference file);

        ValueTask<byte[]> ReadAllBytesAsync(IPrivateFileReference file);

        ValueTask<string> ReadAllTextAsync(IPrivateFileReference file);

        ValueTask<IFileReference> SaveAsync(byte[] data, IPrivateFileReference file, string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metadata = null);

        ValueTask<IFileReference> SaveAsync(Stream data, IPrivateFileReference file, string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metadata = null);

        ValueTask<string> GetSharedAccessSignatureAsync(ISharedAccessPolicy policy);
    }
}
