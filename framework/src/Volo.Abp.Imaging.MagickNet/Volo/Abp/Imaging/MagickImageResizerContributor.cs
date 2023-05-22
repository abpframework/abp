using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace Volo.Abp.Imaging;

public class MagickImageResizerContributor : IImageResizerContributor, ITransientDependency
{
    public async Task<ImageContributorResult<Stream>> TryResizeAsync(Stream stream, ImageResizeArgs resizeArgs,
        string mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanResize(mimeType))
        {
            return new ImageContributorResult<Stream>(stream, false, false);
        }

        MemoryStream ms = null;
        try
        {
            ms = await stream.CreateMemoryStreamAsync(cancellationToken: cancellationToken);

            using var image = new MagickImage(ms);

            if (string.IsNullOrWhiteSpace(mimeType))
            {
                var format = image.FormatInfo;
                mimeType = format?.MimeType;

                if (!CanResize(mimeType))
                {
                    return new ImageContributorResult<Stream>(stream, false, false);
                }
            }

            Resize(image, resizeArgs);

            ms.Position = 0;
            await image.WriteAsync(ms, cancellationToken);
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

    public Task<ImageContributorResult<byte[]>> TryResizeAsync(byte[] bytes, ImageResizeArgs resizeArgs,
        string mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanResize(mimeType))
        {
            return Task.FromResult(new ImageContributorResult<byte[]>(bytes, false, false));
        }

        try
        {
            using var image = new MagickImage(bytes);

            if (string.IsNullOrWhiteSpace(mimeType))
            {
                var format = image.FormatInfo;
                mimeType = format?.MimeType;

                if (!CanResize(mimeType))
                {
                    return Task.FromResult(new ImageContributorResult<byte[]>(bytes, false, false));
                }
            }

            Resize(image, resizeArgs);

            return Task.FromResult(new ImageContributorResult<byte[]>(image.ToByteArray(), true));
        }
        catch (Exception e)
        {
            return Task.FromResult(new ImageContributorResult<byte[]>(bytes, false, true, e));
        }
    }
    
    protected virtual bool CanResize(string mimeType)
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

    protected virtual void Resize(MagickImage image, ImageResizeArgs resizeParameter)
    {
        const int min = 1;
        int targetWidth = resizeParameter.Width, targetHeight = resizeParameter.Height;
        
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;

        if (targetWidth == 0 && targetHeight > 0)
        {
            targetWidth = Math.Max(min, (int)Math.Round(sourceWidth * targetHeight / (float)sourceHeight));
        }

        if (targetHeight == 0 && targetWidth > 0)
        {
            targetHeight = Math.Max(min, (int)Math.Round(sourceHeight * targetWidth / (float)sourceWidth));
        }

        switch (resizeParameter.Mode)
        {
            case ImageResizeMode.None:
                ResizeModeNone(image, targetWidth, targetHeight);
                break;
            case ImageResizeMode.Stretch:
                ResizeStretch(image, targetWidth, targetHeight);
                break;
            case ImageResizeMode.Pad:
                ResizePad(image, targetWidth, targetHeight);
                break;
            case ImageResizeMode.BoxPad:
                ResizeBoxPad(image, targetWidth, targetHeight);
                break;
            case ImageResizeMode.Max:
                ResizeMax(image, targetWidth, targetHeight);
                break;
            case ImageResizeMode.Min:
                ResizeMin(image, targetWidth, targetHeight);
                break;
            case ImageResizeMode.Crop:
                ResizeCrop(image, targetWidth, targetHeight);
                break;
            default:
                throw new NotSupportedException("Resize mode " + resizeParameter.Mode + "is not supported!");
        }
    }
    
    protected virtual void ResizeCrop(MagickImage image, int targetWidth, int targetHeight)
    {
        var defaultMagickGeometry = new MagickGeometry(targetWidth, targetHeight) { IgnoreAspectRatio = true };
        image.Crop(defaultMagickGeometry, Gravity.Center);
    }

    protected virtual void ResizeMin(MagickImage image, int targetWidth, int targetHeight)
    {
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;
        
        var imageRatio = CalculateRatio(sourceWidth, sourceHeight);
        
        var percentWidth = CalculatePercent(sourceWidth, targetWidth);

        if (targetWidth > sourceWidth || targetHeight > sourceHeight)
        {
            targetWidth = sourceWidth;
            targetHeight = sourceHeight;
        }
        else
        {
            var widthDiff = sourceWidth - targetWidth;
            var heightDiff = sourceHeight - targetHeight;

            if (widthDiff > heightDiff)
            {
                targetWidth = (int)Math.Round(targetHeight / imageRatio);
            }
            else if (widthDiff < heightDiff)
            {
                targetHeight = (int)Math.Round(targetWidth * imageRatio);
            }
            else
            {
                if (targetHeight > targetWidth)
                {
                    targetWidth = (int)Math.Round(sourceHeight * percentWidth);
                }
                else
                {
                    targetHeight = (int)Math.Round(sourceHeight * percentWidth);
                }
            }
        }

        image.Resize(targetWidth, targetHeight);
    }

    protected virtual void ResizeMax(IMagickImage image, int targetWidth, int targetHeight)
    {
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;
        
        var imageRatio = CalculateRatio(sourceWidth, sourceHeight);
        var ratio = CalculateRatio(targetWidth, targetHeight);

        var percentHeight = CalculatePercent(sourceHeight, targetHeight);
        var percentWidth = CalculatePercent(sourceWidth, targetWidth);

        if (imageRatio < ratio)
        {
            targetHeight = (int)(sourceHeight * percentWidth);
        }
        else
        {
            targetWidth = (int)(sourceWidth * percentHeight);
        }

        image.Resize(targetWidth, targetHeight);
    }

    protected virtual void ResizeBoxPad(MagickImage image, int targetWidth, int targetHeight)
    {
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;
        
        var percentHeight = CalculatePercent(sourceHeight, targetHeight);
        var percentWidth = CalculatePercent(sourceWidth, targetWidth);

        var newWidth = targetWidth;
        var newHeight = targetHeight;

        var boxPadWidth = targetWidth > 0 ? targetWidth : (int)Math.Round(sourceWidth * percentHeight);
        var boxPadHeight = targetHeight > 0 ? targetHeight : (int)Math.Round(sourceHeight * percentWidth);

        if (sourceWidth < boxPadWidth && sourceHeight < boxPadHeight)
        {
            newWidth = boxPadWidth;
            newHeight = boxPadHeight;
        }
        
        image.Resize(newWidth, newHeight);
        image.Extent(targetWidth, targetHeight, Gravity.Center, MagickColors.Transparent);
    }

    protected virtual void ResizePad(MagickImage image, int targetWidth, int targetHeight)
    {
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;
        
        var percentHeight = CalculatePercent(sourceHeight, targetHeight);
        var percentWidth = CalculatePercent(sourceWidth, targetWidth);
        
        var newWidth = targetWidth;
        var newHeight = targetHeight;

        if (percentHeight < percentWidth)
        {
            newWidth = (int)Math.Round(sourceWidth * percentHeight);
        }
        else
        {
            newHeight = (int)Math.Round(sourceHeight * percentWidth);
        }

        image.Resize(newWidth, newHeight);
        image.Extent(targetWidth, targetHeight, Gravity.Center, MagickColors.Transparent);
    }
    
    protected virtual float CalculatePercent(int imageHeightOrWidth, int heightOrWidth)
    {
        return heightOrWidth / (float)imageHeightOrWidth;
    }

    protected virtual float CalculateRatio(int width, int height)
    {
        return height / (float)width;
    }

    protected virtual void ResizeStretch(IMagickImage image, int targetWidth, int targetHeight)
    {
        image.Resize(new MagickGeometry(targetWidth, targetHeight) { IgnoreAspectRatio = true });
    }

    protected virtual void ResizeModeNone(IMagickImage image, int targetWidth, int targetHeight)
    {
        image.Resize(targetWidth, targetHeight);
    }
}