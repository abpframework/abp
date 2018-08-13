using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Storage.Internal;

namespace Volo.Abp.Storage
{
    public static class AbpStoreExtensions
    {
        public static ValueTask<IBlobReference[]> ListBlobAsync(this IAbpStore store, string path, bool recursive = false,
            bool withMetadata = false)
        {
            return store.ListBlobAsync(path, recursive, withMetadata);
        }

        public static ValueTask<IBlobReference[]> ListBlobAsync(this IAbpStore store, string path, string searchPattern,
            bool recursive = false, bool withMetadata = false)
        {
            return store.ListBlobAsync(path, searchPattern, recursive, withMetadata);
        }

        public static Task DeleteBlobAsync(this IAbpStore store, string path)
        {
            return store.DeleteBlobAsync(new PrivateBlobReference(path));
        }

        public static ValueTask<IBlobReference> GetBlobAsync(this IAbpStore store, string path, bool withMetadata = false)
        {
            return store.GetBlobAsync(new PrivateBlobReference(path), withMetadata);
        }

        public static ValueTask<Stream> ReadBlobAsync(this IAbpStore store, string path)
        {
            return store.ReadBlobAsync(new PrivateBlobReference(path));
        }

        public static ValueTask<byte[]> ReadBlobBytesAsync(this IAbpStore store, string path)
        {
            return store.ReadBlobBytesAsync(new PrivateBlobReference(path));
        }

        public static ValueTask<string> ReadBlobTextAsync(this IAbpStore store, string path)
        {
            return store.ReadBlobTextAsync(new PrivateBlobReference(path));
        }

        public static ValueTask<IBlobReference> SaveBlobAsync(this IAbpStore store, byte[] data, string path,
            string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always,
            IDictionary<string, string> metadata = null)
        {
            return store.SaveBlobAsync(data, new PrivateBlobReference(path), contentType, overwritePolicy,
                metadata);
        }

        public static ValueTask<IBlobReference> SaveBlobAsync(this IAbpStore store, Stream data, string path,
            string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always,
            IDictionary<string, string> metadata = null)
        {
            return store.SaveBlobAsync(data, new PrivateBlobReference(path), contentType, overwritePolicy,
                metadata);
        }

        //TODO : Add CopyAsync and Move async
    }
}