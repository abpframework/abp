using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.AspNetCore.VirtualFileSystem
{
    public interface IHybridWebRootFileProvider : IFileProvider
    {
        string GetAbsolutePath(string relativePath);
    }
}
