using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Volo.Abp.Storage.Exceptions;
using Volo.Abp.Storage.FileSystem.Configuration;
using Volo.Abp.Storage.FileSystem.Internal;
using Volo.Abp.Storage.Internal;

namespace Volo.Abp.Storage.FileSystem
{
    public class AbpFileSystemStore : IAbpStore
    {
        private readonly IExtendedPropertiesProvider _extendedPropertiesProvider;
        private readonly IPublicUrlProvider _publicUrlProvider;
        private readonly AbpFileSystemStoreOptions _storeOptions;

        public AbpFileSystemStore(AbpFileSystemStoreOptions storeOptions, IPublicUrlProvider publicUrlProvider,
            IExtendedPropertiesProvider extendedPropertiesProvider)
        {
            storeOptions.Validate();

            _storeOptions = storeOptions;
            _publicUrlProvider = publicUrlProvider;
            _extendedPropertiesProvider = extendedPropertiesProvider;
        }

        internal string AbsolutePath => _storeOptions.AbsolutePath;

        public string Name => _storeOptions.Name;

        public Task InitAsync()
        {
            if (!Directory.Exists(AbsolutePath)) Directory.CreateDirectory(AbsolutePath);

            return Task.FromResult(0);
        }

        public async ValueTask<IBlobReference[]> ListBlobAsync(string path, bool recursive, bool withMetadata)
        {
            var directoryPath = string.IsNullOrEmpty(path) || path == "/" || path == "\\"
                ? AbsolutePath
                : Path.Combine(AbsolutePath, path);

            var result = new List<IBlobReference>();
            if (!Directory.Exists(directoryPath)) return result.ToArray();
            var allResultPaths = Directory.GetFiles(directoryPath)
                .Select(fp => fp.Replace(AbsolutePath, "").Trim('/', '\\'))
                .ToList();

            foreach (var resultPath in allResultPaths) result.Add(await InternalGetAsync(resultPath, withMetadata));

            return result.ToArray();
        }

        public async ValueTask<IBlobReference[]> ListBlobAsync(string path, string searchPattern, bool recursive,
            bool withMetadata)
        {
            var directoryPath = string.IsNullOrEmpty(path) || path == "/" || path == "\\"
                ? AbsolutePath
                : Path.Combine(AbsolutePath, path);

            var result = new List<IBlobReference>();
            if (!Directory.Exists(directoryPath)) return result.ToArray();
            var matcher = new Matcher(StringComparison.Ordinal);
            matcher.AddInclude(searchPattern);

            var matches = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(directoryPath)));
            var allResultPaths = matches.Files
                .Select(match => Path.Combine(path, match.Path).Trim('/', '\\'))
                .ToList();

            foreach (var resultPath in allResultPaths) result.Add(await InternalGetAsync(resultPath, withMetadata));

            return result.ToArray();
        }

        public Task CopyBlobAsync(string sourceContainerName, string sourceBlobName, string destinationContainerName,
            string destinationBlobName = null)
        {
            var sourcePath = Path.Combine(AbsolutePath, sourceContainerName, sourceBlobName);
            var destPath = Path.Combine(AbsolutePath, destinationContainerName, destinationBlobName ?? sourceBlobName);

            var destDir = Path.GetDirectoryName(destPath);
            Directory.CreateDirectory(destDir);

            File.Copy(sourcePath, destPath, true);

            return Task.FromResult(true);
        }

        public async ValueTask<IBlobReference> GetBlobAsync(IPrivateBlobReference file, bool withMetadata)
        {
            return await InternalGetAsync(file, withMetadata);
        }

        public async ValueTask<IBlobReference> GetBlobAsync(Uri uri, bool withMetadata)
        {
            if (uri.IsAbsoluteUri)
                throw new InvalidOperationException("Cannot resolve an absolute URI with a FileSystem store.");

            return await InternalGetAsync(uri.ToString(), withMetadata);
        }

        public async Task DeleteBlobAsync(IPrivateBlobReference file)
        {
            var fileReference = await InternalGetAsync(file);
            await fileReference.DeleteBlobAsync();
        }

        public async ValueTask<Stream> ReadBlobAsync(IPrivateBlobReference file)
        {
            var fileReference = await InternalGetAsync(file);
            return await fileReference.ReadBlobAsync();
        }

        public async ValueTask<byte[]> ReadBlobBytesAsync(IPrivateBlobReference file)
        {
            var fileReference = await InternalGetAsync(file);
            return await fileReference.ReadBlobBytesAsync();
        }

        public async ValueTask<string> ReadBlobTextAsync(IPrivateBlobReference file)
        {
            var fileReference = await InternalGetAsync(file);
            return await fileReference.ReadBlobTextAsync();
        }

        public async ValueTask<IBlobReference> SaveBlobAsync(byte[] data, IPrivateBlobReference file,
            string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always,
            IDictionary<string, string> metadata = null)
        {
            using (var stream = new MemoryStream(data, 0, data.Length))
            {
                return await SaveBlobAsync(stream, file, contentType, overwritePolicy);
            }
        }

        public async ValueTask<IBlobReference> SaveBlobAsync(Stream data, IPrivateBlobReference file,
            string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always,
            IDictionary<string, string> metadata = null)
        {
            var fileReference = await InternalGetAsync(file, true, false);
            var fileExists = File.Exists(fileReference.FileSystemPath);

            if (fileExists)
                if (overwritePolicy == OverwritePolicy.Never)
                    throw new FileAlreadyExistsException(Name, file.Path);

            var properties = fileReference.BlobDescriptor as FileSystemFileProperties;
            var hashes = ComputeHashes(data);

            if (!fileExists
                || overwritePolicy == OverwritePolicy.Always
                || overwritePolicy == OverwritePolicy.IfContentModified && properties.ContentMd5 != hashes.ContentMD5)
            {
                EnsurePathExists(fileReference.FileSystemPath);

                using (var fileStream = File.Open(fileReference.FileSystemPath, FileMode.Create, FileAccess.Write))
                {
                    await data.CopyToAsync(fileStream);
                }
            }

            properties.ContentType = contentType;
            properties.ExtendedProperties.ETag = hashes.ETag;
            properties.ExtendedProperties.ContentMd5 = hashes.ContentMD5;

            if (metadata != null)
                foreach (var kvp in metadata)
                    properties.Metadata.Add(kvp.Key, kvp.Value);

            await fileReference.SaveBlobDescriptorAsync();

            return fileReference;
        }

        public ValueTask<string> GetBlobSasUrlAsync(ISharedAccessPolicy policy)
        {
            throw new NotSupportedException();
        }

        private ValueTask<FileSystemFileReference> InternalGetAsync(IPrivateBlobReference file,
            bool withMetadata = false, bool checkIfExists = true)
        {
            return InternalGetAsync(file.Path, withMetadata, checkIfExists);
        }

        private async ValueTask<FileSystemFileReference> InternalGetAsync(string path, bool withMetadata,
            bool checkIfExists = true)
        {
            var fullPath = Path.Combine(AbsolutePath, path);
            if (checkIfExists && !File.Exists(fullPath)) return null;

            FileExtendedProperties extendedProperties = null;
            if (!withMetadata)
                return new FileSystemFileReference(
                    fullPath,
                    path,
                    this,
                    withMetadata,
                    extendedProperties,
                    _publicUrlProvider,
                    _extendedPropertiesProvider);
            if (_extendedPropertiesProvider == null)
                throw new InvalidOperationException("There is no FileSystem extended properties provider.");

            extendedProperties = await _extendedPropertiesProvider.GetExtendedPropertiesAsync(
                AbsolutePath,
                new PrivateBlobReference(path));

            return new FileSystemFileReference(
                fullPath,
                path,
                this,
                withMetadata,
                extendedProperties,
                _publicUrlProvider,
                _extendedPropertiesProvider);
        }

        private void EnsurePathExists(string path)
        {
            var directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        }

        private static (string ETag, string ContentMD5) ComputeHashes(Stream stream)
        {
            string eTag;
            string contentMd5;

            stream.Seek(0, SeekOrigin.Begin);
            using (var md5 = MD5.Create())
            {
                stream.Seek(0, SeekOrigin.Begin);
                var hash = md5.ComputeHash(stream);
                stream.Seek(0, SeekOrigin.Begin);
                contentMd5 = Convert.ToBase64String(hash);
                var hex = BitConverter.ToString(hash);
                eTag = $"\"{hex.Replace("-", "")}\"";
            }

            return (eTag, contentMd5);
        }
    }
}