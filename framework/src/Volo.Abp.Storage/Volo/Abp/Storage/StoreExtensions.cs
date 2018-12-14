using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Volo.Abp.Storage
{
    public static class StoreExtensions
    {
        public static ValueTask<IFileReference[]> ListAsync(this IAbpStore store, string path, bool recursive = false,
            bool withMetadata = false)
        {
            return store.ListAsync(path, recursive, withMetadata);
        }

        public static ValueTask<IFileReference[]> ListAsync(this IAbpStore store, string path, string searchPattern,
            bool recursive = false, bool withMetadata = false)
        {
            return store.ListAsync(path, searchPattern, recursive, withMetadata);
        }

        public static Task DeleteAsync(this IAbpStore store, string path)
        {
            return store.DeleteAsync(new PrivateFileReference(path));
        }

        public static ValueTask<IFileReference> GetAsync(this IAbpStore store, string path, bool withMetadata = false)
        {
            return store.GetAsync(new PrivateFileReference(path), withMetadata);
        }

        public static ValueTask<Stream> ReadAsync(this IAbpStore store, string path)
        {
            return store.ReadAsync(new PrivateFileReference(path));
        }

        public static ValueTask<byte[]> ReadAllBytesAsync(this IAbpStore store, string path)
        {
            return store.ReadAllBytesAsync(new PrivateFileReference(path));
        }

        public static ValueTask<string> ReadAllTextAsync(this IAbpStore store, string path)
        {
            return store.ReadAllTextAsync(new PrivateFileReference(path));
        }

        public static ValueTask<IFileReference> SaveAsync(this IAbpStore store, byte[] data, string path,
            string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always,
            IDictionary<string, string> metadata = null)
        {
            return store.SaveAsync(data, new PrivateFileReference(path), contentType, overwritePolicy, metadata);
        }

        public static ValueTask<IFileReference> SaveAsync(this IAbpStore store, Stream data, string path,
            string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always,
            IDictionary<string, string> metadata = null)
        {
            return store.SaveAsync(data, new PrivateFileReference(path), contentType, overwritePolicy, metadata);
        }
    }
}