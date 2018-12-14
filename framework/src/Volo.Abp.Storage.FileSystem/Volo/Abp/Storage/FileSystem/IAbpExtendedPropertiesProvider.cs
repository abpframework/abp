using System.Threading.Tasks;

namespace Volo.Abp.Storage.FileSystem
{
    public interface IAbpExtendedPropertiesProvider
    {
        ValueTask<FileExtendedProperties> GetExtendedPropertiesAsync(string storeAbsolutePath, IPrivateFileReference file);

        Task SaveExtendedPropertiesAsync(string storeAbsolutePath, IPrivateFileReference file, FileExtendedProperties extendedProperties);
    }
}
