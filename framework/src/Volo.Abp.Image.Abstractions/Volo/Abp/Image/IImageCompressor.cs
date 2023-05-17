using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Image;

public interface IImageCompressor // TODO: RENAME: IImageCompressorContributor
{
    //TODO: new extension method that works with byte arrays
    Task<Stream> CompressAsync(Stream stream, CancellationToken cancellationToken = default); // TODO: TryCompressAsync & remove CanCompress

    Stream Compress(Stream stream);

    bool CanCompress(IImageFormat imageFormat);
}