using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.VirtualFileSystem
{
    public interface IDynamicFileProvider : IFileProvider
    {
        void AddOrUpdate(IFileInfo fileInfo);

        bool Delete(string filePath);
    }
}
