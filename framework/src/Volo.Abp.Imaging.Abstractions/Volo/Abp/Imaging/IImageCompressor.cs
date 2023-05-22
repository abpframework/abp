using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Imaging;

public interface IImageCompressor
{
    Task<ImageProcessResult<Stream>> CompressAsync(
        Stream stream,
        [CanBeNull] string mimeType = null,
        bool ignoreExceptions = false,
        CancellationToken cancellationToken = default);
    
    Task<ImageProcessResult<byte[]>> CompressAsync(
        byte[] bytes,
        [CanBeNull] string mimeType = null,
        bool ignoreExceptions = false,
        CancellationToken cancellationToken = default);
}