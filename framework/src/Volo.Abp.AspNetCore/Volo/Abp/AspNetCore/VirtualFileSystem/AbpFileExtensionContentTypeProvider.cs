using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.VirtualFileSystem;

public class AbpFileExtensionContentTypeProvider : IContentTypeProvider, ITransientDependency
{
    protected AbpAspNetCoreContentOptions Options { get; }

    public AbpFileExtensionContentTypeProvider(IOptions<AbpAspNetCoreContentOptions> abpAspNetCoreContentOptions)
    {
        Options = abpAspNetCoreContentOptions.Value;
    }

    public bool TryGetContentType(string subpath, out string contentType)
    {
        var extension = GetExtension(subpath);
        if (extension == null)
        {
            contentType = null;
            return false;
        }

        return Options.ContentTypeMaps.TryGetValue(extension, out contentType);
    }

    protected virtual string GetExtension(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        var index = path.LastIndexOf('.');
        return index < 0 ? null : path.Substring(index);
    }
}
