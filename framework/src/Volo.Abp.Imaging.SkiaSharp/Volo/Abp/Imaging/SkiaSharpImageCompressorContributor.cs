using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SkiaSharp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace Volo.Abp.Imaging;

public class SkiaSharpImageCompressorContributor : IImageCompressorContributor, ITransientDependency
{
    protected SkiaSharpCompressOptions Options { get; }

    public SkiaSharpImageCompressorContributor(IOptions<SkiaSharpCompressOptions> options)
    {
        Options = options.Value;
    }

    public virtual async Task<ImageCompressResult<Stream>> TryCompressAsync(
        Stream stream,
        string? mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanCompress(mimeType))
        {
            return new ImageCompressResult<Stream>(stream, ImageProcessState.Unsupported);
        }

        var (memoryBitmapStream, memorySkCodecStream) = await CreateMemoryStream(stream, cancellationToken);
        var originalLength = memoryBitmapStream.Length;

        try
        {
            using var codec = SKCodec.Create(memorySkCodecStream);
            if (codec == null || !CanEncodeFormat(codec.EncodedFormat))
            {
                return new ImageCompressResult<Stream>(stream, ImageProcessState.Unsupported);
            }

            using var bitmap = SKBitmap.Decode(memoryBitmapStream);
            if (bitmap == null)
            {
                return new ImageCompressResult<Stream>(stream, ImageProcessState.Unsupported);
            }

            using var image = SKImage.FromBitmap(bitmap);
            using var encoded = image.Encode(codec.EncodedFormat, Options.Quality);

            var output = new MemoryStream();
            try
            {
                encoded.SaveTo(output);
                output.Position = 0;

                if (output.Length < originalLength)
                {
                    return new ImageCompressResult<Stream>(output, ImageProcessState.Done);
                }

                output.Dispose();
                return new ImageCompressResult<Stream>(stream, ImageProcessState.Canceled);
            }
            catch
            {
                output.Dispose();
                throw;
            }
        }
        finally
        {
            memoryBitmapStream.Dispose();
            memorySkCodecStream.Dispose();
        }
    }

    public virtual async Task<ImageCompressResult<byte[]>> TryCompressAsync(
        byte[] bytes,
        string? mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanCompress(mimeType))
        {
            return new ImageCompressResult<byte[]>(bytes, ImageProcessState.Unsupported);
        }

        using var ms = new MemoryStream(bytes);
        var result = await TryCompressAsync(ms, mimeType, cancellationToken);

        if (result.State != ImageProcessState.Done)
        {
            return new ImageCompressResult<byte[]>(bytes, result.State);
        }

        var newBytes = await result.Result.GetAllBytesAsync(cancellationToken);
        result.Result.Dispose();
        return new ImageCompressResult<byte[]>(newBytes, result.State);
    }

    protected virtual bool CanCompress(string? mimeType)
    {
        return mimeType switch {
            MimeTypes.Image.Jpeg => true,
            MimeTypes.Image.Png => true,
            MimeTypes.Image.Webp => true,
            _ => false
        };
    }

    protected virtual bool CanEncodeFormat(SKEncodedImageFormat format)
    {
        return format switch
        {
            SKEncodedImageFormat.Jpeg => true,
            SKEncodedImageFormat.Png => true,
            SKEncodedImageFormat.Webp => true,
            _ => false
        };
    }

    protected virtual async Task<(MemoryStream, MemoryStream)> CreateMemoryStream(Stream stream, CancellationToken cancellationToken)
    {
        var streamPosition = stream.CanSeek ? stream.Position : 0;

        var memoryBitmapStream = new MemoryStream();
        var memorySkCodecStream = new MemoryStream();

        try
        {
            await stream.CopyToAsync(memoryBitmapStream, cancellationToken);

            if (stream.CanSeek)
            {
                stream.Position = streamPosition;
            }

            memoryBitmapStream.Position = 0;
            await memoryBitmapStream.CopyToAsync(memorySkCodecStream, cancellationToken);

            memoryBitmapStream.Position = 0;
            memorySkCodecStream.Position = 0;

            return (memoryBitmapStream, memorySkCodecStream);
        }
        catch
        {
            memoryBitmapStream.Dispose();
            memorySkCodecStream.Dispose();
            throw;
        }
    }
}
