using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Volo.Abp.Storage.FileSystem.ExtendedProperties
{
    public class AbpExtendedPropertiesProvider : IAbpExtendedPropertiesProvider
    {
        private readonly FileSystemExtendedPropertiesOptions _options;

        public AbpExtendedPropertiesProvider(
            IOptions<FileSystemExtendedPropertiesOptions> options)
        {
            _options = options.Value;
        }

        public ValueTask<FileExtendedProperties> GetExtendedPropertiesAsync(string storeAbsolutePath,
            IPrivateFileReference file)
        {
            var extendedPropertiesPath = GetExtendedPropertiesPath(storeAbsolutePath, file);
            if (!File.Exists(extendedPropertiesPath))
                return new ValueTask<FileExtendedProperties>(new FileExtendedProperties());

            var content = File.ReadAllText(extendedPropertiesPath);
            return new ValueTask<FileExtendedProperties>(
                JsonConvert.DeserializeObject<FileExtendedProperties>(content));
        }

        public Task SaveExtendedPropertiesAsync(string storeAbsolutePath, IPrivateFileReference file,
            FileExtendedProperties extendedProperties)
        {
            var extendedPropertiesPath = GetExtendedPropertiesPath(storeAbsolutePath, file);
            var toStore = JsonConvert.SerializeObject(extendedProperties);
            File.WriteAllText(extendedPropertiesPath, toStore);
            return Task.FromResult(0);
        }

        private string GetExtendedPropertiesPath(string storeAbsolutePath, IPrivateFileReference file)
        {
            var fullPath = Path.GetFullPath(storeAbsolutePath).TrimEnd(Path.DirectorySeparatorChar);
            var rootPath = Path.GetDirectoryName(fullPath);
            var storeName = Path.GetFileName(fullPath);

            var extendedPropertiesPath = Path.Combine(rootPath, string.Format(_options.FolderNameFormat, storeName),
                file.Path + ".json");
            EnsurePathExists(extendedPropertiesPath);
            return extendedPropertiesPath;
        }

        private static void EnsurePathExists(string path)
        {
            var directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        }
    }
}