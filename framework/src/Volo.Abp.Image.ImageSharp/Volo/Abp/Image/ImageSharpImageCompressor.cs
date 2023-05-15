using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Image;

public class ImageSharpImageCompressor : IImageCompressor, ITransientDependency
{
    public async Task<Stream> CompressAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var newStream = await stream.CreateMemoryStreamAsync(cancellationToken: cancellationToken);
        using var image = await SixLabors.ImageSharp.Image.LoadAsync(newStream, cancellationToken);
        newStream.Position = 0;
        var format = await SixLabors.ImageSharp.Image.DetectFormatAsync(newStream, cancellationToken);
        newStream.Position = 0;
        var encoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(format);

        switch (encoder)
        {
            case JpegEncoder jpegEncoder:
                jpegEncoder.Quality = 60;
                break;
            case PngEncoder pngEncoder:
                pngEncoder.CompressionLevel = PngCompressionLevel.BestCompression;
                pngEncoder.IgnoreMetadata = true;
                break;
            case WebpEncoder webPEncoder:
                webPEncoder.Quality = 60;
                webPEncoder.UseAlphaCompression = true;
                break;
            case null:
                throw new NotSupportedException($"No encoder available for provided path: {format.Name}");
        }

        await image.SaveAsync(newStream, encoder, cancellationToken: cancellationToken);
        newStream.SetLength(newStream.Position);
        return newStream;
    }

    public Stream Compress(Stream stream)
    {
        var newStream = stream.CreateMemoryStream();
        using var image = SixLabors.ImageSharp.Image.Load(newStream);
        newStream.Position = 0;
        var format = SixLabors.ImageSharp.Image.DetectFormat(newStream);
        newStream.Position = 0;
        var encoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(format);

        switch (encoder)
        {
            case JpegEncoder jpegEncoder:
                jpegEncoder.Quality = 60;
                break;
            case PngEncoder pngEncoder:
                pngEncoder.CompressionLevel = PngCompressionLevel.BestCompression;
                pngEncoder.IgnoreMetadata = true;
                break;
            case WebpEncoder webPEncoder:
                webPEncoder.Quality = 60;
                webPEncoder.UseAlphaCompression = true;
                break;
            case null:
                throw new NotSupportedException($"No encoder available for provided path: {format.Name}");
        }

        image.Save(newStream, encoder);
        newStream.SetLength(newStream.Position);
        return newStream;
    }


    public bool CanCompress(IImageFormat format)
    {
        return format?.MimeType switch {
            "image/jpeg" => true,
            "image/png" => true,
            "image/webp" => true,
            _ => false
        };
    }
}