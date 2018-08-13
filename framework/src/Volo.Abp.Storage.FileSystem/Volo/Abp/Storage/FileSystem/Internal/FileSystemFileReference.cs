using System;
using System.IO;
using System.Threading.Tasks;

namespace Volo.Abp.Storage.FileSystem.Internal
{
    public class FileSystemFileReference : IBlobReference
    {
        private readonly IExtendedPropertiesProvider _extendedPropertiesProvider;
        private readonly Lazy<string> _publicUrlLazy;
        private readonly AbpFileSystemStore _store;
        private Lazy<IBlobDescriptor> _propertiesLazy;
        private bool _withMetadata;

        public FileSystemFileReference(
            string filePath,
            string path,
            AbpFileSystemStore store,
            bool withMetadata,
            FileExtendedProperties extendedProperties,
            IPublicUrlProvider publicUrlProvider,
            IExtendedPropertiesProvider extendedPropertiesProvider)
        {
            FileSystemPath = filePath;
            Path = path.Replace('\\', '/');
            _store = store;
            _extendedPropertiesProvider = extendedPropertiesProvider;
            _withMetadata = withMetadata;

            _propertiesLazy = new Lazy<IBlobDescriptor>(() =>
            {
                if (withMetadata) return new FileSystemFileProperties(FileSystemPath, extendedProperties);

                throw new InvalidOperationException("Metadata are not loaded, please use withMetadata option");
            });

            _publicUrlLazy = new Lazy<string>(() =>
            {
                if (publicUrlProvider != null) return publicUrlProvider.GetPublicUrl(_store.Name, this);

                throw new InvalidOperationException("There is not FileSystemServer enabled.");
            });
        }

        public string FileSystemPath { get; }

        public string Path { get; }

        public string GetPublicBlobUrl => _publicUrlLazy.Value;

        public IBlobDescriptor BlobDescriptor => _propertiesLazy.Value;

        public Task DeleteBlobAsync()
        {
            File.Delete(FileSystemPath);
            return Task.FromResult(true);
        }

        public ValueTask<byte[]> ReadBlobBytesAsync()
        {
            return new ValueTask<byte[]>(File.ReadAllBytes(FileSystemPath));
        }

        public ValueTask<string> ReadBlobTextAsync()
        {
            return new ValueTask<string>(File.ReadAllText(FileSystemPath));
        }

        public ValueTask<Stream> ReadBlobAsync()
        {
            return new ValueTask<Stream>(File.OpenRead(FileSystemPath));
        }

        public async Task ReadBlobToStreamAsync(Stream targetStream)
        {
            using (var file = File.Open(FileSystemPath, FileMode.Open, FileAccess.Read))
            {
                await file.CopyToAsync(targetStream);
            }
        }

        public async Task UpdateBlobAsync(Stream stream)
        {
            using (var file = File.Open(FileSystemPath, FileMode.Truncate, FileAccess.Write))
            {
                await stream.CopyToAsync(file);
            }
        }

        public Task SaveBlobDescriptorAsync()
        {
            if (_extendedPropertiesProvider == null)
                throw new InvalidOperationException("There is no FileSystem extended properties provider.");

            return _extendedPropertiesProvider.SaveExtendedPropertiesAsync(
                _store.AbsolutePath,
                this,
                (BlobDescriptor as FileSystemFileProperties).ExtendedProperties);
        }

        public ValueTask<string> GetBlobSasUrl(ISharedAccessPolicy policy)
        {
            throw new NotSupportedException();
        }

        public async Task FetchBlobProperties()
        {
            if (_withMetadata) return;

            if (_extendedPropertiesProvider == null)
                throw new InvalidOperationException("There is no FileSystem extended properties provider.");

            var extendedProperties = await _extendedPropertiesProvider.GetExtendedPropertiesAsync(
                _store.AbsolutePath,
                this);

            _propertiesLazy =
                new Lazy<IBlobDescriptor>(() => new FileSystemFileProperties(FileSystemPath, extendedProperties));
            _withMetadata = true;
        }
    }
}