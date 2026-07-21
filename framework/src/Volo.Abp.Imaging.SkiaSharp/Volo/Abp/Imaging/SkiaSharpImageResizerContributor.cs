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

            using var resized = ApplyResize(original, resizeArgs);
            using var image = SKImage.FromBitmap(resized);

            var memoryStream = new MemoryStream();
            try
            {
                using var skData = image.Encode(codec.EncodedFormat, Options.Quality);
                skData.SaveTo(memoryStream);
                memoryStream.Position = 0;
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

    protected virtual SKBitmap ApplyResize(SKBitmap source, ImageResizeArgs resizeArgs)
    {
        var targetWidth = (int)resizeArgs.Width;
        var targetHeight = (int)resizeArgs.Height;

        if (targetWidth <= 0 && targetHeight <= 0)
        {
            return source.Copy();
        }

        if (targetWidth <= 0)
        {
            targetWidth = Math.Max(1, (int)Math.Round((double)source.Width * targetHeight / source.Height));
        }
        else if (targetHeight <= 0)
        {
            targetHeight = Math.Max(1, (int)Math.Round((double)source.Height * targetWidth / source.Width));
        }

        var mode = resizeArgs.Mode == ImageResizeMode.Default ? ImageResizeMode.Stretch : resizeArgs.Mode;

        switch (mode)
        {
            case ImageResizeMode.None:
            case ImageResizeMode.Stretch:
                return source.Resize(new SKImageInfo(targetWidth, targetHeight), Options.SKSamplingOptions);

            case ImageResizeMode.Max:
            {
                var scale = Math.Min((double)targetWidth / source.Width, (double)targetHeight / source.Height);
                var newW = Math.Max(1, (int)Math.Round(source.Width * scale));
                var newH = Math.Max(1, (int)Math.Round(source.Height * scale));
                return source.Resize(new SKImageInfo(newW, newH), Options.SKSamplingOptions);
            }

            case ImageResizeMode.Min:
            {
                var scale = Math.Max((double)targetWidth / source.Width, (double)targetHeight / source.Height);
                var newW = Math.Max(1, (int)Math.Round(source.Width * scale));
                var newH = Math.Max(1, (int)Math.Round(source.Height * scale));
                return source.Resize(new SKImageInfo(newW, newH), Options.SKSamplingOptions);
            }

            case ImageResizeMode.Crop:
            {
                var scale = Math.Max((double)targetWidth / source.Width, (double)targetHeight / source.Height);
                var intermediateW = Math.Max(1, (int)Math.Round(source.Width * scale));
                var intermediateH = Math.Max(1, (int)Math.Round(source.Height * scale));
                using var intermediate = source.Resize(new SKImageInfo(intermediateW, intermediateH), Options.SKSamplingOptions);

                var bitmap = new SKBitmap(targetWidth, targetHeight);
                try
                {
                    using var canvas = new SKCanvas(bitmap);
                    var srcX = (intermediateW - targetWidth) / 2;
                    var srcY = (intermediateH - targetHeight) / 2;
                    canvas.DrawBitmap(
                        intermediate,
                        new SKRect(srcX, srcY, srcX + targetWidth, srcY + targetHeight),
                        new SKRect(0, 0, targetWidth, targetHeight));
                    return bitmap;
                }
                catch
                {
                    bitmap.Dispose();
                    throw;
                }
            }

            case ImageResizeMode.Pad:
            {
                var scale = Math.Min((double)targetWidth / source.Width, (double)targetHeight / source.Height);
                var intermediateW = Math.Max(1, (int)Math.Round(source.Width * scale));
                var intermediateH = Math.Max(1, (int)Math.Round(source.Height * scale));
                using var intermediate = source.Resize(new SKImageInfo(intermediateW, intermediateH), Options.SKSamplingOptions);

                var bitmap = new SKBitmap(targetWidth, targetHeight);
                try
                {
                    using var canvas = new SKCanvas(bitmap);
                    canvas.Clear(SKColors.Transparent);
                    var dstX = (targetWidth - intermediateW) / 2;
                    var dstY = (targetHeight - intermediateH) / 2;
                    canvas.DrawBitmap(intermediate, new SKPoint(dstX, dstY));
                    return bitmap;
                }
                catch
                {
                    bitmap.Dispose();
                    throw;
                }
            }

            case ImageResizeMode.BoxPad:
            {
                SKBitmap? scaled = null;
                try
                {
                    var working = source;
                    if (source.Width > targetWidth || source.Height > targetHeight)
                    {
                        var scale = Math.Min((double)targetWidth / source.Width, (double)targetHeight / source.Height);
                        var newW = Math.Max(1, (int)Math.Round(source.Width * scale));
                        var newH = Math.Max(1, (int)Math.Round(source.Height * scale));
                        scaled = source.Resize(new SKImageInfo(newW, newH), Options.SKSamplingOptions);
                        working = scaled;
                    }

                    var bitmap = new SKBitmap(targetWidth, targetHeight);
                    try
                    {
                        using var canvas = new SKCanvas(bitmap);
                        canvas.Clear(SKColors.Transparent);
                        var dstX = (targetWidth - working.Width) / 2;
                        var dstY = (targetHeight - working.Height) / 2;
                        canvas.DrawBitmap(working, new SKPoint(dstX, dstY));
                        return bitmap;
                    }
                    catch
                    {
                        bitmap.Dispose();
                        throw;
                    }
                }
                finally
                {
                    scaled?.Dispose();
                }
            }

            default:
                throw new NotSupportedException("Resize mode " + resizeArgs.Mode + " is not supported!");
        }
    }
}
