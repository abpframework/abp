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

        using (var memoryStream = new MemoryStream(bytes))
        {
            var result = await TryResizeAsync(memoryStream, resizeArgs, mimeType, cancellationToken);

            if (result.State != ImageProcessState.Done)
            {
                return new ImageResizeResult<byte[]>(bytes, result.State);
            }

            var newBytes = await result.Result.GetAllBytesAsync(cancellationToken);

            result.Result.Dispose();

            return new ImageResizeResult<byte[]>(newBytes, result.State);
        }
    }

    public virtual async Task<ImageResizeResult<Stream>> TryResizeAsync(Stream stream, ImageResizeArgs resizeArgs, string? mimeType = null, CancellationToken cancellationToken = default)
    {
        if (!mimeType.IsNullOrWhiteSpace() && !CanResize(mimeType))
        {
            return new ImageResizeResult<Stream>(stream, ImageProcessState.Unsupported);
        }

        var (memoryBitmapStream, memorySkCodecStream) = await CreateMemoryStream(stream);

        using (var original = SKBitmap.Decode(memoryBitmapStream))
        {
            using (var resized = original.Resize(new SKImageInfo(resizeArgs.Width, resizeArgs.Height), Options.SKFilterQuality))
            {
                using (var image = SKImage.FromBitmap(resized))
                {
                    using (var codec = SKCodec.Create(memorySkCodecStream))
                    {
                        var memoryStream = new MemoryStream();
                        image.Encode(codec.EncodedFormat, Options.Quality).SaveTo(memoryStream);
                        return new ImageResizeResult<Stream>(memoryStream, ImageProcessState.Done);
                    }
                }
            }
        }
    }

    protected virtual async Task<(MemoryStream, MemoryStream)> CreateMemoryStream(Stream stream)
    {
        var streamPosition = stream.Position;

        var memoryBitmapStream = new MemoryStream();
        var memorySkCodecStream = new MemoryStream();

        await stream.CopyToAsync(memoryBitmapStream);
        stream.Position = streamPosition;
        await stream.CopyToAsync(memorySkCodecStream);
        stream.Position = streamPosition;

        memoryBitmapStream.Position = 0;
        memorySkCodecStream.Position = 0;

        return (memoryBitmapStream, memorySkCodecStream);
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
}
