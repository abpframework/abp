using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Image;

public class ImageSharpImageResizer : IImageResizer, ITransientDependency
{
    
    public async Task<Stream> ResizeAsync(Stream stream, IImageResizeParameter resizeParameter, CancellationToken cancellationToken = default)
    {
        var newStream = await stream.CreateMemoryStreamAsync(cancellationToken: cancellationToken);
        using var image = await SixLabors.ImageSharp.Image.LoadAsync(newStream, cancellationToken);
        ApplyMode(image, resizeParameter);
        newStream.Position = 0;
        var format = await SixLabors.ImageSharp.Image.DetectFormatAsync(newStream, cancellationToken);
        newStream.Position = 0;
        await image.SaveAsync(newStream, format, cancellationToken: cancellationToken);
        newStream.SetLength(newStream.Position);
        newStream.Position = 0;
        return newStream;
    }

    public Stream Resize(Stream stream, IImageResizeParameter resizeParameter)
    {
        var newStream = stream.CreateMemoryStream();
        using var image = SixLabors.ImageSharp.Image.Load(newStream);
        ApplyMode(image, resizeParameter);
        newStream.Position = 0;
        var format = SixLabors.ImageSharp.Image.DetectFormat(newStream);
        newStream.Position = 0;
        image.Save(newStream, format);
        newStream.SetLength(newStream.Position);
        newStream.Position = 0;
        return newStream;
    }

    public bool CanResize(IImageFormat imageFormat)
    {
        return imageFormat?.MimeType switch
        {
            "image/jpeg" => true,
            "image/png" => true,
            "image/gif" => true,
            "image/bmp" => true,
            "image/tiff" => true,
            _ => false
        };
    }

    private void ApplyMode(SixLabors.ImageSharp.Image image, IImageResizeParameter resizeParameter)
    {
        var width = resizeParameter.Width ?? image.Width;
        var height = resizeParameter.Height ?? image.Height;

        var defaultResizeOptions = new ResizeOptions { Size = new SixLabors.ImageSharp.Size(width, height) };

        switch (resizeParameter.Mode)
        {
            case null:
            case ImageResizeMode.None:
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.Stretch:
                defaultResizeOptions.Mode = ResizeMode.Stretch;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.BoxPad:
                defaultResizeOptions.Mode = ResizeMode.BoxPad;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.Min:
                defaultResizeOptions.Mode = ResizeMode.Min;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.Max:
                defaultResizeOptions.Mode = ResizeMode.Max;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.Crop:
                defaultResizeOptions.Mode = ResizeMode.Crop;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.Pad:
                defaultResizeOptions.Mode = ResizeMode.Pad;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.Fill:
                defaultResizeOptions.Mode = ResizeMode.Stretch;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            default:
                throw new NotSupportedException("Resize mode " + resizeParameter.Mode + "is not supported!");
        }
    }
}