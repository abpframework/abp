using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Image;

public interface IImageResizer
{
    Task<Stream> ResizeAsync(Stream stream, IImageResizeParameter resizeParameter,
        CancellationToken cancellationToken = default); //TODO: TryResizeAsync...
    
    Stream Resize(Stream stream, IImageResizeParameter resizeParameter); //TODO: Remove

    bool CanResize(IImageFormat imageFormat); //TODO: Discard (merge with TryResizeAsync)
}