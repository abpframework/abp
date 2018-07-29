using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Storage.LocalStorage
{
    public class LocalStorageProvider : IStorageProvider, ITransientDependency
    {
        private readonly string _basePath;

        public LocalStorageProvider(IHostingEnvironment env, LocalStorageProviderOptions options)
        {
            _basePath = string.IsNullOrEmpty(options.BasePath) ? env.ContentRootPath : options.BasePath;
        }

        public void DeleteBlob(string containerName, string blobName)
        {
            var path = Path.Combine(_basePath, containerName, blobName);

            if (!File.Exists(path))
            {
                throw new StorageException(StorageErrorCode.InvalidName.ToStorageError(), null);
            }
            try { File.Delete(path); }
            catch (Exception ex)
            {
                throw ex.ToStorageException();
            }
        }

        public Task DeleteBlobAsync(string containerName, string blobName)
        {
            return Task.Factory.StartNew(() => DeleteBlob(containerName, blobName));
        }

        public void DeleteContainer(string containerName)
        {
            try
            {
                var path = Path.Combine(_basePath, containerName);
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                throw ex.ToStorageException();
            }
        }

        public Task DeleteContainerAsync(string containerName)
        {
            return Task.Run(() => DeleteContainer(containerName));
        }

        public Task CopyBlobAsync(string sourceContainerName, string sourceBlobName, string destinationContainerName,
            string destinationBlobName = null)
        {
            if (string.IsNullOrEmpty(sourceContainerName))
            {
                throw new StorageException(StorageErrorCode.InvalidName, $"Invalid {nameof(sourceContainerName)}");
            }
            if (string.IsNullOrEmpty(sourceBlobName))
            {
                throw new StorageException(StorageErrorCode.InvalidName, $"Invalid {nameof(sourceBlobName)}");
            }
            if (string.IsNullOrEmpty(destinationContainerName))
            {
                throw new StorageException(StorageErrorCode.InvalidName, $"Invalid {nameof(destinationContainerName)}");
            }
            if (destinationBlobName == string.Empty)
            {
                throw new StorageException(StorageErrorCode.InvalidName, $"Invalid {nameof(destinationBlobName)}");
            }

            var sourcePath = Path.Combine(_basePath, sourceContainerName, sourceBlobName);
            var destPath = Path.Combine(_basePath, destinationContainerName, destinationBlobName ?? sourceBlobName);
            
            var destDir = Path.GetDirectoryName(destPath);
            Directory.CreateDirectory(destDir);

            File.Copy(sourcePath, destPath, true);

            return Task.FromResult(true);
        }

        public async Task MoveBlobAsync(string sourceContainerName, string sourceBlobName, string destinationContainerName,
            string destinationBlobName = null)
        {
            await CopyBlobAsync(sourceContainerName, sourceBlobName, destinationContainerName, destinationBlobName);
            await DeleteBlobAsync(sourceContainerName, sourceBlobName);
        }

        public BlobDescriptor GetBlobDescriptor(string containerName, string blobName)
        {
            var path = Path.Combine(_basePath, containerName, blobName);
            
            try
            {
                var info = new FileInfo(path);

                return new BlobDescriptor
                {
                    Container = containerName,
                    ContentMD5 = "",
                    ContentType = info.Extension.GetMimeType(),
                    ETag = "",
                    LastModified = info.LastWriteTimeUtc,
                    Length = info.Length,
                    Name = info.Name,
                    Security = BlobSecurity.Private,
                    Url = info.FullName
                };
            }
            catch (Exception ex)
            {
                throw new StorageException(1001.ToStorageError(), ex);
            }
        }

        public Task<BlobDescriptor> GetBlobDescriptorAsync(string containerName, string blobName)
        {
            return Task.Run(() => GetBlobDescriptor(containerName, blobName));
        }

        public string GetBlobSasUrl(string containerName, string blobName, DateTimeOffset expiry, 
            bool isDownload = false, string fileName = null, string contentType = null, BlobUrlAccess access = BlobUrlAccess.Read)
        {
            return Path.Combine(_basePath, containerName, blobName);
        }

        public Stream GetBlobStream(string containerName, string blobName)
        {
            try
            {
                var path = Path.Combine(_basePath, containerName, blobName);
                return File.OpenRead(path);
            }
            catch (Exception ex)
            {
                throw ex.ToStorageException();
            }
        }

        public async Task<Stream> GetBlobStreamAsync(string containerName, string blobName)
        {
            return await Task.Run(() => GetBlobStream(containerName, blobName));
        }

        public string GetBlobUrl(string containerName, string blobName)
        {
            return Path.Combine(_basePath, containerName, blobName);
        }

        public IList<BlobDescriptor> ListBlobs(string containerName)
        {
            var localFilesInfo = new List<BlobDescriptor>();

            try
            {
                var dir = Path.Combine(_basePath, containerName);
                var dirInfo = new DirectoryInfo(dir);
                var fileInfo = dirInfo.GetFiles("*", SearchOption.AllDirectories);

                foreach (var f in fileInfo)
                {
                    localFilesInfo.Add(new BlobDescriptor
                    {
                        ContentMD5 = "",
                        ETag = "",
                        ContentType = f.Extension.GetMimeType(),
                        Container = containerName,
                        LastModified = f.LastWriteTime,
                        Length = f.Length,
                        Name = f.Name,
                        Url = f.FullName,
                        Security = BlobSecurity.Private,
                    });
                }

                return localFilesInfo;
            }
            catch (Exception ex)
            {
                throw new StorageException(1001.ToStorageError(), ex);
            }
        }

        public Task<IList<BlobDescriptor>> ListBlobsAsync(string containerName)
        {
            return Task.Run(()=> ListBlobs(containerName));
        }

        public void SaveBlobStream(string containerName, string blobName, Stream source, BlobProperties properties = null, bool closeStream = true)
        {
            var dir = Path.Combine(_basePath, containerName);

            try
            {
                var path = Path.Combine(dir, blobName);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var file = File.Create(path))
                {
                    source.CopyTo(file);
                }

                if (closeStream)
                {
                    source.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex.ToStorageException();
            }
        }

        public async Task SaveBlobStreamAsync(string containerName, string blobName, Stream source, BlobProperties properties = null, bool closeStream = true)
        {
            var dir = Path.Combine(_basePath, containerName);

            try
            {
                var path = Path.Combine(dir, blobName);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var file = File.Create(path))
                {
                    await source.CopyToAsync(file);
                }

                if (closeStream)
                {
                    source.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex.ToStorageException();
            }
        }

        public void UpdateBlobProperties(string containerName, string blobName, BlobProperties properties)
        {
            return;
        }

        public Task UpdateBlobPropertiesAsync(string containerName, string blobName, BlobProperties properties)
        {
            return Task.FromResult(0);
        }
    }
}