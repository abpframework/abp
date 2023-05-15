using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Image;

public class MagickImageCompressor : IImageCompressor, ITransientDependency
{
    private readonly ImageOptimizer _optimizer = new();

    public async Task<Stream> CompressAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var newStream = await stream.CreateMemoryStreamAsync(cancellationToken:cancellationToken);
        try
        {
            _optimizer.IsSupported(newStream);
            newStream.Position = 0;
            _optimizer.Compress(newStream);
        }
        catch
        {
            // ignored
        }
        return newStream;
    }

    public Stream Compress(Stream stream)
    {
        var newStream = stream.CreateMemoryStream();
        try
        {
            _optimizer.IsSupported(newStream);
            newStream.Position = 0;
            _optimizer.Compress(newStream);
        }
        catch
        {
            // ignored
        }
        return newStream;
    }

    public bool CanCompress(IImageFormat imageFormat)
    {
        return imageFormat?.MimeType switch
        {
            "image/jpeg" => true,
            "image/png" => true,
            "image/gif" => true,
            "image/bmp" => true,
            "image/tiff" => true,
            _ => false
        };
    }
}