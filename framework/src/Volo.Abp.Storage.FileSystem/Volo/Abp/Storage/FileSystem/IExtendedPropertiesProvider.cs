using System.Threading.Tasks;
using Volo.Abp.Storage.FileSystem.Internal;

namespace Volo.Abp.Storage.FileSystem
{
    public interface IExtendedPropertiesProvider
    {
        ValueTask<FileExtendedProperties> GetExtendedPropertiesAsync(string storeAbsolutePath,
            IPrivateBlobReference file);

        Task SaveExtendedPropertiesAsync(string storeAbsolutePath, IPrivateBlobReference file,
            FileExtendedProperties extendedProperties);
    }
}