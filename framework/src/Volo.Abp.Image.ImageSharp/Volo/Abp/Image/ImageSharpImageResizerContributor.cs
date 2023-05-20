using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace Volo.Abp.Image;

public class ImageSharpImageResizerContributor : IImageResizerContributor, ITransientDependency
{
    public async Task<ImageContributorResult<Stream>> TryResizeAsync(Stream stream, ImageResizeArgs resizeArgs,
        string mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanResize(mimeType))
        {
            return new ImageContributorResult<Stream>(stream, false, false);
        }

        var (image, format) = await SixLabors.ImageSharp.Image.LoadWithFormatAsync(stream, cancellationToken);

        mimeType = format.DefaultMimeType;

        if (!CanResize(mimeType))
        {
            return new ImageContributorResult<Stream>(stream, false, false);
        }

        var size = new Size();
        if (resizeArgs.Width > 0)
        {
            size.Width = resizeArgs.Width;
        }

        if (resizeArgs.Height > 0)
        {
            size.Height = resizeArgs.Height;
        }

        var defaultResizeOptions = new ResizeOptions { Size = size };

        MemoryStream ms = null;
        try
        {
            if (ResizeModeMap.TryGetValue(resizeArgs.Mode, out var resizeMode))
            {
                defaultResizeOptions.Mode = resizeMode;
                image.Mutate(x => x.Resize(defaultResizeOptions));
            }
            else
            {
                throw new NotSupportedException("Resize mode " + resizeArgs.Mode + "is not supported!");
            }
            
            ms = new MemoryStream();
            await image.SaveAsync(ms, format, cancellationToken: cancellationToken);
            ms.SetLength(ms.Position);
            ms.Position = 0;

            return new ImageContributorResult<Stream>(ms, true);
        }
        catch (Exception e)
        {
            ms?.Dispose();
            return new ImageContributorResult<Stream>(stream, false, true, e);
        }
    }

    public async Task<ImageContributorResult<byte[]>> TryResizeAsync(byte[] bytes, ImageResizeArgs resizeArgs,
        string mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanResize(mimeType))
        {
            return new ImageContributorResult<byte[]>(bytes, false, false);
        }

        using var ms = new MemoryStream(bytes);
        var result = await TryResizeAsync(ms, resizeArgs, mimeType, cancellationToken);

        if (!result.IsSuccess)
        {
            return new ImageContributorResult<byte[]>(bytes, result.IsSuccess, result.IsSupported, result.Exception);
        }

        var newBytes = await result.Result.GetAllBytesAsync(cancellationToken);
        result.Result.Dispose();

        return new ImageContributorResult<byte[]>(newBytes, true);
    }

    private static bool CanResize(string mimeType)
    {
        return mimeType switch {
            MimeTypes.Image.Jpeg => true,
            MimeTypes.Image.Png => true,
            MimeTypes.Image.Gif => true,
            MimeTypes.Image.Bmp => true,
            MimeTypes.Image.Tiff => true,
            _ => false
        };
    }

    private readonly static Dictionary<ImageResizeMode, ResizeMode> ResizeModeMap = new() {
        { ImageResizeMode.None, ResizeMode.Crop },
        { ImageResizeMode.Stretch, ResizeMode.Stretch },
        { ImageResizeMode.BoxPad, ResizeMode.BoxPad },
        { ImageResizeMode.Min, ResizeMode.Min },
        { ImageResizeMode.Max, ResizeMode.Max },
        { ImageResizeMode.Crop, ResizeMode.Crop },
        { ImageResizeMode.Pad, ResizeMode.Pad }
    };
}