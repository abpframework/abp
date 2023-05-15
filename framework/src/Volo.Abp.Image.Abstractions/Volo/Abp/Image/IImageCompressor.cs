using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Image;

public interface IImageCompressor
{
    Task<Stream> CompressAsync(Stream stream, CancellationToken cancellationToken = default);

    Stream Compress(Stream stream);

    bool CanCompress(IImageFormat imageFormat);
}