using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Imaging;

public interface IImageCompressorContributor
{
    Task<ImageCompressResult<Stream>> TryCompressAsync(
        Stream stream, 
        [CanBeNull] string mimeType = null, 
        CancellationToken cancellationToken = default);
    Task<ImageCompressResult<byte[]>> TryCompressAsync(
        byte[] bytes, 
        [CanBeNull] string mimeType = null, 
        CancellationToken cancellationToken = default);
}