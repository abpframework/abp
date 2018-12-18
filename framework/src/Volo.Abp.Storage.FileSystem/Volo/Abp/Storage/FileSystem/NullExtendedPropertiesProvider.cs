using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Storage.FileSystem
{
    public class NullExtendedPropertiesProvider : IAbpExtendedPropertiesProvider, ITransientDependency
    {
        public ValueTask<FileExtendedProperties> GetExtendedPropertiesAsync(string storeAbsolutePath, IPrivateFileReference file)
        {
            throw new InvalidOperationException("There is no FileSystem extended properties provider. Add ");
        }

        public Task SaveExtendedPropertiesAsync(string storeAbsolutePath, IPrivateFileReference file, FileExtendedProperties extendedProperties)
        {
            throw new InvalidOperationException("There is no FileSystem extended properties provider.");
        }
    }
}
