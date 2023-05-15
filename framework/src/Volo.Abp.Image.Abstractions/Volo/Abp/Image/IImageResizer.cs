using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Image;

public interface IImageResizer
{
    Task<Stream> ResizeAsync(Stream stream, IImageResizeParameter resizeParameter,
        CancellationToken cancellationToken = default);
    
    Stream Resize(Stream stream, IImageResizeParameter resizeParameter);

    bool CanResize(IImageFormat imageFormat);
}