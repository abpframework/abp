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
    private const int Min = 1;

    public virtual async Task<ImageResizeResult<Stream>> TryResizeAsync(
        Stream stream,
        ImageResizeArgs resizeArgs,
        string? mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!mimeType.IsNullOrWhiteSpace() && !CanResize(mimeType))
        {
            return new ImageResizeResult<Stream>(stream, ImageProcessState.Unsupported);
        }

        var memoryStream = await stream.CreateMemoryStreamAsync(cancellationToken: cancellationToken);

        try
        {
            using var image = new MagickImage(memoryStream);

            if (mimeType.IsNullOrWhiteSpace() && !CanResize(image.FormatInfo?.MimeType))
            {
                return new ImageResizeResult<Stream>(stream, ImageProcessState.Unsupported);
            }

            Resize(image, resizeArgs);

            memoryStream.Position = 0;
            await image.WriteAsync(memoryStream, cancellationToken);
            memoryStream.SetLength(memoryStream.Position);
            memoryStream.Position = 0;

            return new ImageResizeResult<Stream>(memoryStream, ImageProcessState.Done);
        }
        catch
        {
            memoryStream.Dispose();
            throw;
        }
    }

    public virtual Task<ImageResizeResult<byte[]>> TryResizeAsync(
        byte[] bytes,
        ImageResizeArgs resizeArgs,
        string? mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!mimeType.IsNullOrWhiteSpace() && !CanResize(mimeType))
        {
            return Task.FromResult(new ImageResizeResult<byte[]>(bytes, ImageProcessState.Unsupported));
        }

        using var image = new MagickImage(bytes);

        if (mimeType.IsNullOrWhiteSpace() && !CanResize(image.FormatInfo?.MimeType))
        {
            return Task.FromResult(new ImageResizeResult<byte[]>(bytes, ImageProcessState.Unsupported));
        }

        Resize(image, resizeArgs);

        return Task.FromResult(new ImageResizeResult<byte[]>(image.ToByteArray(), ImageProcessState.Done));
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

    protected virtual void Resize(MagickImage image, ImageResizeArgs resizeArgs)
    {
        ApplyResizeMode(image, resizeArgs);
    }

    protected virtual void ApplyResizeMode(MagickImage image, ImageResizeArgs resizeArgs)
    {
        switch (resizeArgs.Mode)
        {
            case ImageResizeMode.None:
                ResizeModeNone(image, resizeArgs);
                break;
            case ImageResizeMode.Stretch:
                ResizeStretch(image, resizeArgs);
                break;
            case ImageResizeMode.Pad:
                ResizePad(image, resizeArgs);
                break;
            case ImageResizeMode.BoxPad:
                ResizeBoxPad(image, resizeArgs);
                break;
            case ImageResizeMode.Max:
                ResizeMax(image, resizeArgs);
                break;
            case ImageResizeMode.Min:
                ResizeMin(image, resizeArgs);
                break;
            case ImageResizeMode.Crop:
                ResizeCrop(image, resizeArgs);
                break;
            default:
                throw new NotSupportedException("Resize mode " + resizeArgs.Mode + "is not supported!");
        }
    }


    protected virtual int GetTargetHeight(ImageResizeArgs resizeArgs, int min, int sourceWidth, int sourceHeight)
    {
        if (resizeArgs.Height == 0 && resizeArgs.Width > 0)
        {
            return Math.Max(min, (int)Math.Round(sourceHeight * resizeArgs.Width / (float)sourceWidth));
        }

        return resizeArgs.Height;
    }

    protected virtual int GetTargetWidth(ImageResizeArgs resizeArgs, int min, int sourceWidth, int sourceHeight)
    {
        if (resizeArgs.Width == 0 && resizeArgs.Height > 0)
        {
            return Math.Max(min, (int)Math.Round(sourceWidth * resizeArgs.Height / (float)sourceHeight));
        }

        return resizeArgs.Width;
    }

    protected virtual void ResizeModeNone(IMagickImage image, ImageResizeArgs resizeArgs)
    {
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;

        image.Resize(
            GetTargetWidth(resizeArgs, Min, sourceWidth, sourceHeight),
            GetTargetHeight(resizeArgs, Min, sourceWidth, sourceHeight)
        );
    }

    protected virtual void ResizeStretch(IMagickImage image, ImageResizeArgs resizeArgs)
    {
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;

        image.Resize(
            new MagickGeometry(
                GetTargetWidth(resizeArgs, Min, sourceWidth, sourceHeight),
                GetTargetHeight(resizeArgs, Min, sourceWidth, sourceHeight)) { IgnoreAspectRatio = true });
    }

    protected virtual void ResizePad(MagickImage image, ImageResizeArgs resizeArgs)
    {
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;

        var targetWidth = GetTargetWidth(resizeArgs, Min, sourceWidth, sourceHeight);
        var targetHeight = GetTargetHeight(resizeArgs, Min, sourceWidth, sourceHeight);

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

    protected virtual void ResizeBoxPad(MagickImage image, ImageResizeArgs resizeArgs)
    {
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;

        var targetWidth = GetTargetWidth(resizeArgs, Min, sourceWidth, sourceHeight);
        var targetHeight = GetTargetHeight(resizeArgs, Min, sourceWidth, sourceHeight);

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

    protected virtual void ResizeMax(IMagickImage image, ImageResizeArgs resizeArgs)
    {
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;

        var imageRatio = CalculateRatio(sourceWidth, sourceHeight);

        var targetWidth = GetTargetWidth(resizeArgs, Min, sourceWidth, sourceHeight);
        var targetHeight = GetTargetHeight(resizeArgs, Min, sourceWidth, sourceHeight);

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

    protected virtual void ResizeMin(MagickImage image, ImageResizeArgs resizeArgs)
    {
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;

        var imageRatio = CalculateRatio(sourceWidth, sourceHeight);

        var targetWidth = GetTargetWidth(resizeArgs, Min, sourceWidth, sourceHeight);
        var targetHeight = GetTargetHeight(resizeArgs, Min, sourceWidth, sourceHeight);

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

    protected virtual void ResizeCrop(MagickImage image, ImageResizeArgs resizeArgs)
    {
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;

        var targetWidth = GetTargetWidth(resizeArgs, Min, sourceWidth, sourceHeight);
        var targetHeight = GetTargetHeight(resizeArgs, Min, sourceWidth, sourceHeight);

        image.Extent(
            targetWidth,
            targetHeight,
            Gravity.Center,
            MagickColors.Transparent);

        image.Crop(
            new MagickGeometry(
                targetWidth,
                targetHeight) { IgnoreAspectRatio = true },
            Gravity.Center);
    }

    protected virtual float CalculatePercent(int imageHeightOrWidth, int heightOrWidth)
    {
        return heightOrWidth / (float)imageHeightOrWidth;
    }

    protected virtual float CalculateRatio(int width, int height)
    {
        return height / (float)width;
    }
}