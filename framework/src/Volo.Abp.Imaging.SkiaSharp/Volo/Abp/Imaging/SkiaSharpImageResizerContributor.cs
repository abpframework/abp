using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SkiaSharp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace Volo.Abp.Imaging;

public class SkiaSharpImageResizerContributor : IImageResizerContributor, ITransientDependency
{
    protected SkiaSharpResizerOptions Options { get; }

    public SkiaSharpImageResizerContributor(IOptions<SkiaSharpResizerOptions> options)
    {
        Options = options.Value;
    }

    public virtual async Task<ImageResizeResult<byte[]>> TryResizeAsync(byte[] bytes, ImageResizeArgs resizeArgs, string? mimeType = null, CancellationToken cancellationToken = default)
    {
        if (!mimeType.IsNullOrWhiteSpace() && !CanResize(mimeType))
        {
            return new ImageResizeResult<byte[]>(bytes, ImageProcessState.Unsupported);
        }

        using var memoryStream = new MemoryStream(bytes);
        var result = await TryResizeAsync(memoryStream, resizeArgs, mimeType, cancellationToken);

        if (result.State != ImageProcessState.Done)
        {
            return new ImageResizeResult<byte[]>(bytes, result.State);
        }

        var newBytes = await result.Result.GetAllBytesAsync(cancellationToken);

        result.Result.Dispose();

        return new ImageResizeResult<byte[]>(newBytes, result.State);
    }

    public virtual async Task<ImageResizeResult<Stream>> TryResizeAsync(Stream stream, ImageResizeArgs resizeArgs, string? mimeType = null, CancellationToken cancellationToken = default)
    {
        if (!mimeType.IsNullOrWhiteSpace() && !CanResize(mimeType))
        {
            return new ImageResizeResult<Stream>(stream, ImageProcessState.Unsupported);
        }

        var (memoryBitmapStream, memorySkCodecStream) = await CreateMemoryStream(stream, cancellationToken);

        try
        {
            using var codec = SKCodec.Create(memorySkCodecStream);
            if (codec == null || !CanEncodeFormat(codec.EncodedFormat))
            {
                return new ImageResizeResult<Stream>(stream, ImageProcessState.Unsupported);
            }

            using var original = SKBitmap.Decode(memoryBitmapStream);
            if (original == null)
            {
                return new ImageResizeResult<Stream>(stream, ImageProcessState.Unsupported);
            }

            using var resized = original.Resize(new SKImageInfo((int)resizeArgs.Width, (int)resizeArgs.Height), Options.SKSamplingOptions);
            using var image = SKImage.FromBitmap(resized);

            var memoryStream = new MemoryStream();
            try
            {
                using var skData = image.Encode(codec.EncodedFormat, Options.Quality);
                skData.SaveTo(memoryStream);
                return new ImageResizeResult<Stream>(memoryStream, ImageProcessState.Done);
            }
            catch
            {
                memoryStream.Dispose();
                throw;
            }
        }
        finally
        {
            memoryBitmapStream.Dispose();
            memorySkCodecStream.Dispose();
        }
    }

    protected virtual async Task<(MemoryStream, MemoryStream)> CreateMemoryStream(Stream stream, CancellationToken cancellationToken = default)
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

    protected virtual bool CanResize(string? mimeType)
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
}
