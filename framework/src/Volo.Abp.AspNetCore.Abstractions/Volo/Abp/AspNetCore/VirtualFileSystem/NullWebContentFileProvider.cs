using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Volo.Abp.AspNetCore.VirtualFileSystem;

public class NullWebContentFileProvider : IWebContentFileProvider
{
    public virtual IFileInfo GetFileInfo(string subpath)
    {
        return new NotFoundFileInfo(subpath);
    }

    public virtual IDirectoryContents GetDirectoryContents(string subpath)
    {
        return new NotFoundDirectoryContents();
    }

    public virtual IChangeToken Watch(string filter)
    {
        return NullChangeToken.Singleton;
    }
}
