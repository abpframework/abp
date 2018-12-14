using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.FileSystem
{
    public class AbpFileSystemStore : IAbpStore
    {
        private readonly FileSystemStoreOptions _storeOptions;
        private readonly IPublicUrlProvider _publicUrlProvider;
        private readonly IAbpExtendedPropertiesProvider _extendedPropertiesProvider;

        public AbpFileSystemStore(FileSystemStoreOptions storeOptions, IPublicUrlProvider publicUrlProvider, IAbpExtendedPropertiesProvider extendedPropertiesProvider)
        {
            storeOptions.Validate();

            _storeOptions = storeOptions;
            _publicUrlProvider = publicUrlProvider;
            _extendedPropertiesProvider = extendedPropertiesProvider;
        }

        public string Name => _storeOptions.Name;

        internal string AbsolutePath => _storeOptions.AbsolutePath;

        public Task InitAsync()
        {
            if (!Directory.Exists(AbsolutePath))
            {
                Directory.CreateDirectory(AbsolutePath);
            }

            return Task.FromResult(0);
        }

        public async ValueTask<IFileReference[]> ListAsync(string path, bool recursive, bool withMetadata)
        {
            var directoryPath = (string.IsNullOrEmpty(path) || path == "/" || path == "\\") ? AbsolutePath : Path.Combine(AbsolutePath, path);

            var result = new List<IFileReference>();
            
            if (!Directory.Exists(directoryPath)) return result.ToArray();
            
            var allResultPaths = Directory.GetFiles(directoryPath)
                .Select(fp => fp.Replace(AbsolutePath, "").Trim('/', '\\'))
                .ToList();

            foreach (var resultPath in allResultPaths)
            {
                result.Add(await InternalGetAsync(resultPath, withMetadata));
            }

            return result.ToArray();
        }

        public async ValueTask<IFileReference[]> ListAsync(string path, string searchPattern, bool recursive, bool withMetadata)
        {
            var directoryPath = (string.IsNullOrEmpty(path) || path == "/" || path == "\\") ? AbsolutePath : Path.Combine(AbsolutePath, path);

            var result = new List<IFileReference>();
            if (!Directory.Exists(directoryPath)) return result.ToArray();
            
            var matcher = new Microsoft.Extensions.FileSystemGlobbing.Matcher(StringComparison.Ordinal);
            matcher.AddInclude(searchPattern);

            var matches = matcher.Execute(new Microsoft.Extensions.FileSystemGlobbing.Abstractions.DirectoryInfoWrapper(new DirectoryInfo(directoryPath)));
            var allResultPaths = matches.Files
                .Select(match => Path.Combine(path, match.Path).Trim('/', '\\'))
                .ToList();

            foreach (var resultPath in allResultPaths)
            {
                result.Add(await InternalGetAsync(resultPath, withMetadata));
            }

            return result.ToArray();
        }

        public async ValueTask<IFileReference> GetAsync(IPrivateFileReference file, bool withMetadata)
        {
            return await InternalGetAsync(file, withMetadata);
        }

        public async ValueTask<IFileReference> GetAsync(Uri uri, bool withMetadata)
        {
            if (uri.IsAbsoluteUri)
            {
                throw new InvalidOperationException("Cannot resolve an absolute URI with a FileSystem store.");
            }

            return await InternalGetAsync(uri.ToString(), withMetadata);
        }

        public async Task DeleteAsync(IPrivateFileReference file)
        {
            var fileReference = await InternalGetAsync(file);
            await fileReference.DeleteAsync();
        }

        public async ValueTask<Stream> ReadAsync(IPrivateFileReference file)
        {
            var fileReference = await InternalGetAsync(file);
            return await fileReference.ReadAsync();
        }

        public async ValueTask<byte[]> ReadAllBytesAsync(IPrivateFileReference file)
        {
            var fileReference = await InternalGetAsync(file);
            return await fileReference.ReadAllBytesAsync();
        }

        public async ValueTask<string> ReadAllTextAsync(IPrivateFileReference file)
        {
            var fileReference = await InternalGetAsync(file);
            return await fileReference.ReadAllTextAsync();
        }

        public async ValueTask<IFileReference> SaveAsync(byte[] data, IPrivateFileReference file, string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metadata = null)
        {
            using (var stream = new MemoryStream(data, 0, data.Length))
            {
                return await SaveAsync(stream, file, contentType, overwritePolicy);
            }
        }

        public async ValueTask<IFileReference> SaveAsync(Stream data, IPrivateFileReference file, string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metadata = null)
        {
            var fileReference = await InternalGetAsync(file, withMetadata: true, checkIfExists: false);
            var fileExists = File.Exists(fileReference.FileSystemPath);

            if (fileExists)
            {
                if (overwritePolicy == OverwritePolicy.Never)
                {
                    throw new Exceptions.FileAlreadyExistsException(Name, file.Path);
                }
            }

            var properties = fileReference.Properties as FileSystemFileProperties;
            var hashes = ComputeHashes(data);

            if (!fileExists 
                || overwritePolicy == OverwritePolicy.Always
                || (overwritePolicy == OverwritePolicy.IfContentModified && properties.ContentMd5 != hashes.ContentMD5))
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
            {
                foreach (var kvp in metadata)
                {
                    properties.Metadata.Add(kvp.Key, kvp.Value);
                }
            }

            await fileReference.SavePropertiesAsync();

            return fileReference;
        }

        public ValueTask<string> GetSharedAccessSignatureAsync(ISharedAccessPolicy policy)
        {
            throw new NotSupportedException();
        }

        private ValueTask<FileSystemFileReference> InternalGetAsync(IPrivateFileReference file, bool withMetadata = false, bool checkIfExists = true)
        {
            return InternalGetAsync(file.Path, withMetadata, checkIfExists);
        }

        private async ValueTask<FileSystemFileReference> InternalGetAsync(string path, bool withMetadata, bool checkIfExists = true)
        {
            var fullPath = Path.Combine(AbsolutePath, path);
            if (checkIfExists && !File.Exists(fullPath))
            {
                return null;
            }

            FileExtendedProperties extendedProperties = null;
            if (withMetadata)
            {
                if (_extendedPropertiesProvider == null)
                {
                    throw new InvalidOperationException("There is no FileSystem extended properties provider.");
                }

                extendedProperties = await _extendedPropertiesProvider.GetExtendedPropertiesAsync(
                    AbsolutePath,
                    new PrivateFileReference(path));
            }

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
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
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
