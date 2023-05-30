using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Imaging;

public interface IImageResizer
{
    Task<ImageResizeResult<Stream>> ResizeAsync(
        Stream stream,
        ImageResizeArgs resizeArgs,
        [CanBeNull] string mimeType = null,
        CancellationToken cancellationToken = default
    );

    Task<ImageResizeResult<byte[]>> ResizeAsync(
        byte[] bytes,
        ImageResizeArgs resizeArgs,
        [CanBeNull] string mimeType = null,
        CancellationToken cancellationToken = default
    );
}