using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace Volo.Abp.Imaging;

public class ImageSharpImageResizerContributor : IImageResizerContributor, ITransientDependency
{
    public virtual async Task<ImageResizeResult<Stream>> TryResizeAsync(
        Stream stream,
        ImageResizeArgs resizeArgs,
        string? mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanResize(mimeType))
        {
            return new ImageResizeResult<Stream>(stream, ImageProcessState.Unsupported);
        }

        var image = await Image.LoadAsync(stream, cancellationToken);

        if (!CanResize(image.Metadata.DecodedImageFormat!.DefaultMimeType))
        {
            return new ImageResizeResult<Stream>(stream, ImageProcessState.Unsupported);
        }

        if (ResizeModeMap.TryGetValue(resizeArgs.Mode, out var resizeMode))
        {
            image.Mutate(x => x.Resize(new ResizeOptions { Size = GetSize(resizeArgs), Mode = resizeMode }));
        }
        else
        {
            throw new NotSupportedException("Resize mode " + resizeArgs.Mode + "is not supported!");
        }

        var memoryStream = new MemoryStream();

        try
        {
            await image.SaveAsync(memoryStream, image.Metadata.DecodedImageFormat, cancellationToken: cancellationToken);
            memoryStream.Position = 0;
            return new ImageResizeResult<Stream>(memoryStream, ImageProcessState.Done);
        }
        catch
        {
            memoryStream.Dispose();
            throw;
        }
    }

    public virtual async Task<ImageResizeResult<byte[]>> TryResizeAsync(
        byte[] bytes, 
        ImageResizeArgs resizeArgs,
        string? mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanResize(mimeType))
        {
            return new ImageResizeResult<byte[]>(bytes, ImageProcessState.Unsupported);
        }

        using var ms = new MemoryStream(bytes);

        var result = await TryResizeAsync(ms, resizeArgs, mimeType, cancellationToken);

        if (result.State != ImageProcessState.Done)
        {
            return new ImageResizeResult<byte[]>(bytes, result.State);
        }

        var newBytes = await result.Result.GetAllBytesAsync(cancellationToken);

        result.Result.Dispose();

        return new ImageResizeResult<byte[]>(newBytes, result.State);
    }

    protected virtual bool CanResize(string? mimeType)
    {
        return mimeType switch {
            MimeTypes.Image.Jpeg => true,
            MimeTypes.Image.Png => true,
            MimeTypes.Image.Gif => true,
            MimeTypes.Image.Bmp => true,
            MimeTypes.Image.Tiff => true,
            MimeTypes.Image.Webp => true,
            _ => false
        };
    }

    protected Dictionary<ImageResizeMode, ResizeMode> ResizeModeMap = new() {
        { ImageResizeMode.None, default },
        { ImageResizeMode.Stretch, ResizeMode.Stretch },
        { ImageResizeMode.BoxPad, ResizeMode.BoxPad },
        { ImageResizeMode.Min, ResizeMode.Min },
        { ImageResizeMode.Max, ResizeMode.Max },
        { ImageResizeMode.Crop, ResizeMode.Crop },
        { ImageResizeMode.Pad, ResizeMode.Pad }
    };

    private static Size GetSize(ImageResizeArgs resizeArgs)
    {
        var size = new Size();
        
        if (resizeArgs.Width > 0)
        {
            size.Width = resizeArgs.Width;
        }

        if (resizeArgs.Height > 0)
        {
            size.Height = resizeArgs.Height;
        }

        return size;
    }
}