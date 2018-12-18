using System;
using System.IO;
using System.Threading.Tasks;

namespace Volo.Abp.Storage.FileSystem
{
    public class FileSystemFileReference : IFileReference
    {
        private readonly AbpFileSystemStore _store;
        private readonly Lazy<string> _publicUrlLazy;
        private readonly IAbpExtendedPropertiesProvider _extendedPropertiesProvider;

        private bool _withMetadata;
        private Lazy<IFileProperties> _propertiesLazy;

        public FileSystemFileReference(
            string filePath,
            string path,
            AbpFileSystemStore store,
            bool withMetadata,
            FileExtendedProperties extendedProperties,
            IAbpPublicUrlProvider publicUrlProvider,
            IAbpExtendedPropertiesProvider extendedPropertiesProvider)
        {
            FileSystemPath = filePath;
            Path = path.Replace('\\', '/');
            _store = store;
            _extendedPropertiesProvider = extendedPropertiesProvider;
            _withMetadata = withMetadata;

            _propertiesLazy = new Lazy<IFileProperties>(() =>
            {
                if (withMetadata)
                {
                    return new FileSystemFileProperties(FileSystemPath, extendedProperties);
                }

                throw new InvalidOperationException("Metadata are not loaded, please use withMetadata option");
            });

            _publicUrlLazy = new Lazy<string>(() =>
            {
                if (publicUrlProvider != null)
                {
                    return publicUrlProvider.GetPublicUrl(_store.Name, this);
                }

                throw new InvalidOperationException("There is not Server enabled.");
            });
        }

        public string FileSystemPath { get; }

        public string Path { get; }

        public string PublicUrl => _publicUrlLazy.Value;

        public IFileProperties Properties => _propertiesLazy.Value;

        public Task DeleteAsync()
        {
            File.Delete(FileSystemPath);
            return Task.FromResult(true);
        }

        public ValueTask<byte[]> ReadAllBytesAsync()
        {
            return new ValueTask<byte[]>(File.ReadAllBytes(FileSystemPath));
        }

        public ValueTask<string> ReadAllTextAsync()
        {
            return new ValueTask<string>(File.ReadAllText(FileSystemPath));
        }

        public ValueTask<Stream> ReadAsync()
        {
            return new ValueTask<Stream>(File.OpenRead(FileSystemPath));
        }

        public async Task ReadToStreamAsync(Stream targetStream)
        {
            using (var file = File.Open(FileSystemPath, FileMode.Open, FileAccess.Read))
            {
                await file.CopyToAsync(targetStream);
            }
        }

        public async Task UpdateAsync(Stream stream)
        {
            using (var file = File.Open(FileSystemPath, FileMode.Truncate, FileAccess.Write))
            {
                await stream.CopyToAsync(file);
            }
        }

        public Task SavePropertiesAsync()
        {
            return _extendedPropertiesProvider.SaveExtendedPropertiesAsync(
                _store.AbsolutePath,
                this,
                (Properties as FileSystemFileProperties)?.ExtendedProperties);
        }

        public ValueTask<string> GetSharedAccessSignature(ISharedAccessPolicy policy)
        {
            throw new NotSupportedException();
        }

        public async Task FetchProperties()
        {
            if (_withMetadata)
            {
                return;
            }

            var extendedProperties = await _extendedPropertiesProvider.GetExtendedPropertiesAsync(
                _store.AbsolutePath,
                this);

            _propertiesLazy =
                new Lazy<IFileProperties>(() => new FileSystemFileProperties(FileSystemPath, extendedProperties));
            _withMetadata = true;
        }
    }
}