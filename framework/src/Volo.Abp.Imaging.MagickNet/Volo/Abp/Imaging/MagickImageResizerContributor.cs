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
    public virtual async Task<ImageResizeResult<Stream>> TryResizeAsync(Stream stream, ImageResizeArgs resizeArgs,
        string mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!mimeType.IsNullOrWhiteSpace() && !CanResize(mimeType))
        {
            return new ImageResizeResult<Stream>(stream, ProcessState.Unsupported);
        }

        var memoryStream = await stream.CreateMemoryStreamAsync(cancellationToken: cancellationToken);

        try
        {
            using var image = new MagickImage(memoryStream);

            if (mimeType.IsNullOrWhiteSpace() && !CanResize(image.FormatInfo?.MimeType))
            {
                return new ImageResizeResult<Stream>(stream, ProcessState.Unsupported);
            }

            Resize(image, resizeArgs);

            memoryStream.Position = 0;
            await image.WriteAsync(memoryStream, cancellationToken);
            memoryStream.SetLength(memoryStream.Position);
            memoryStream.Position = 0;

            return new ImageResizeResult<Stream>(memoryStream, ProcessState.Done);
        }
        catch
        {
            memoryStream.Dispose();
            throw;
        }
    }

    public virtual Task<ImageResizeResult<byte[]>> TryResizeAsync(byte[] bytes, ImageResizeArgs resizeArgs,
        string mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!mimeType.IsNullOrWhiteSpace() && !CanResize(mimeType))
        {
            return Task.FromResult(new ImageResizeResult<byte[]>(bytes, ProcessState.Unsupported));
        }
        
        using var image = new MagickImage(bytes);

        if (mimeType.IsNullOrWhiteSpace() && !CanResize(image.FormatInfo?.MimeType))
        {
            return Task.FromResult(new ImageResizeResult<byte[]>(bytes, ProcessState.Unsupported));
        }

        Resize(image, resizeArgs);

        return Task.FromResult(new ImageResizeResult<byte[]>(image.ToByteArray(), ProcessState.Done));
    }

    protected virtual bool CanResize(string mimeType)
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

    protected virtual void Resize(MagickImage image, ImageResizeArgs resizeParameter)
    {
        const int min = 1;

        var sourceWidth = image.Width;
        var sourceHeight = image.Height;

        var targetWidth = GetTargetWidth(resizeParameter.Width, resizeParameter.Height, min, sourceWidth, sourceHeight);
        var targetHeight = GetTargetHeight(resizeParameter.Height, targetWidth, min, sourceHeight, sourceWidth);

        ApplyResizeMode(image, resizeParameter, targetWidth, targetHeight);
    }

    protected virtual void ApplyResizeMode(MagickImage image, ImageResizeArgs resizeParameter, int targetWidth,
        int targetHeight)
    {
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


    protected virtual int GetTargetHeight(int targetHeight, int targetWidth, int min, int sourceHeight, int sourceWidth)
    {
        if (targetHeight == 0 && targetWidth > 0)
        {
            targetHeight = Math.Max(min, (int)Math.Round(sourceHeight * targetWidth / (float)sourceWidth));
        }

        return targetHeight;
    }

    protected virtual int GetTargetWidth(int targetWidth, int targetHeight, int min, int sourceWidth, int sourceHeight)
    {
        if (targetWidth == 0 && targetHeight > 0)
        {
            targetWidth = Math.Max(min, (int)Math.Round(sourceWidth * targetHeight / (float)sourceHeight));
        }

        return targetWidth;
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