using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Image;

public class MagickImageResizer : IImageResizer, ITransientDependency
{
    public async Task<Stream> ResizeAsync(Stream stream, IImageResizeParameter resizeParameter,
        CancellationToken cancellationToken = default)
    {
        var newStream = await stream.CreateMemoryStreamAsync(cancellationToken: cancellationToken);
        try //TODO: Remove try/catch
        {
            using var image = new MagickImage(newStream);
            ApplyMode(image, resizeParameter);
            newStream.Position = 0;
            await image.WriteAsync(newStream, cancellationToken);
            newStream.SetLength(newStream.Position);
            newStream.Position = 0;
        }
        catch
        {
            // ignored
        }

        return newStream;
    }

    public Stream Resize(Stream stream, IImageResizeParameter resizeParameter)
    {
        var newStream =  stream.CreateMemoryStream();
        try
        {
            using var image = new MagickImage(newStream);
            ApplyMode(image, resizeParameter);
            newStream.Position = 0;
            image.Write(newStream);
            newStream.SetLength(newStream.Position);
            newStream.Position = 0;
        }
        catch
        {
            // ignored
        }

        return newStream;
    }

    public bool CanResize(IImageFormat imageFormat)
    {
        return imageFormat?.MimeType switch {
            "image/jpeg" => true,
            "image/png" => true,
            "image/gif" => true,
            "image/bmp" => true,
            "image/tiff" => true,
            _ => false
        };
    }

    private void ApplyMode(IMagickImage image, IImageResizeParameter resizeParameter)
    {
        var width = resizeParameter.Width ?? image.Width;
        var height = resizeParameter.Height ?? image.Height;
        var defaultMagickGeometry = new MagickGeometry(width, height);
        var imageRatio = image.Height / (float)image.Width;
        var percentHeight = Math.Abs(height / (float)image.Height);
        var percentWidth = Math.Abs(width / (float)image.Width);
        var ratio = height / (float)width;
        var newWidth = width;
        var newHeight = height;
        switch (resizeParameter.Mode)
        {
            case null:
            case ImageResizeMode.None:
                image.Resize(defaultMagickGeometry);
                break;
            case ImageResizeMode.Stretch:
                defaultMagickGeometry.IgnoreAspectRatio = true;
                image.Resize(defaultMagickGeometry);
                break;
            case ImageResizeMode.Pad:
                if (percentHeight < percentWidth)
                {
                    newWidth = (int)Math.Round(image.Width * percentHeight);
                }
                else
                {
                    newHeight = (int)Math.Round(image.Height * percentWidth);
                }

                defaultMagickGeometry.IgnoreAspectRatio = true;
                image.Resize(newWidth, newHeight);
                image.Extent(width, height, Gravity.Center);
                break;
            case ImageResizeMode.BoxPad:
                int boxPadWidth = width > 0 ? width : (int)Math.Round(image.Width * percentHeight);
                int boxPadHeight = height > 0 ? height : (int)Math.Round(image.Height * percentWidth);

                if (image.Width < boxPadWidth && image.Height < boxPadHeight)
                {
                    newWidth = boxPadWidth;
                    newHeight = boxPadHeight;
                }

                image.Resize(newWidth, newHeight);
                image.Extent(defaultMagickGeometry, Gravity.Center);
                break;
            case ImageResizeMode.Max:

                if (imageRatio < ratio)
                {
                    newHeight = (int)(image.Height * percentWidth);
                }
                else
                {
                    newWidth = (int)(image.Width * percentHeight);
                }

                image.Resize(newWidth, newHeight);
                break;
            case ImageResizeMode.Min:
                if (width > image.Width || height > image.Height)
                {
                    newWidth = image.Width;
                    newHeight = image.Height;
                }
                else
                {
                    int widthDiff = image.Width - width;
                    int heightDiff = image.Height - height;

                    if (widthDiff > heightDiff)
                    {
                        newWidth = (int)Math.Round(height / imageRatio);
                    }
                    else if (widthDiff < heightDiff)
                    {
                        newHeight = (int)Math.Round(width * imageRatio);
                    }
                    else
                    {
                        if (height > width)
                        {
                            newHeight = (int)Math.Round(image.Height * percentWidth);
                        }
                        else
                        {
                            newHeight = (int)Math.Round(image.Height * percentWidth);
                        }
                    }
                }

                image.Resize(newWidth, newHeight);
                break;
            case ImageResizeMode.Crop:
                defaultMagickGeometry.IgnoreAspectRatio = true;
                image.Crop(width, height, Gravity.Center);
                image.Resize(defaultMagickGeometry);
                break;
            case ImageResizeMode.Distort:
                image.Distort(DistortMethod.Resize, width, height);
                break;
            case ImageResizeMode.Fill:
                defaultMagickGeometry.IgnoreAspectRatio = true;
                defaultMagickGeometry.FillArea = true;
                image.Resize(defaultMagickGeometry);
                break;
            default:
                throw new NotSupportedException("Resize mode " + resizeParameter.Mode + "is not supported!");
        }
    }
}