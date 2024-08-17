using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace Volo.Abp.Imaging;

public class ImageSharpImageCompressorContributor : IImageCompressorContributor, ITransientDependency
{
    protected ImageSharpCompressOptions Options { get; }

    public ImageSharpImageCompressorContributor(IOptions<ImageSharpCompressOptions> options)
    {
        Options = options.Value;
    }

    public virtual async Task<ImageCompressResult<Stream>> TryCompressAsync(
        Stream stream, 
        string? mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanCompress(mimeType))
        {
            return new ImageCompressResult<Stream>(stream, ImageProcessState.Unsupported);
        }

        var image = await Image.LoadAsync(stream, cancellationToken);

        if (!CanCompress(image.Metadata.DecodedImageFormat!.DefaultMimeType))
        {
            return new ImageCompressResult<Stream>(stream, ImageProcessState.Unsupported);
        }

        var memoryStream = await GetStreamFromImageAsync(image, image.Metadata.DecodedImageFormat, cancellationToken);

        if (memoryStream.Length < stream.Length)
        {
            return new ImageCompressResult<Stream>(memoryStream, ImageProcessState.Done);
        }

        memoryStream.Dispose();
        return new ImageCompressResult<Stream>(stream, ImageProcessState.Canceled);
    }

    public virtual async Task<ImageCompressResult<byte[]>> TryCompressAsync(
        byte[] bytes, 
        string? mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanCompress(mimeType))
        {
            return new ImageCompressResult<byte[]>(bytes, ImageProcessState.Unsupported);
        }

        using var ms = new MemoryStream(bytes);
        var result = await TryCompressAsync(ms, mimeType, cancellationToken);

        if (result.State != ImageProcessState.Done)
        {
            return new ImageCompressResult<byte[]>(bytes, result.State);
        }

        var newBytes = await result.Result.GetAllBytesAsync(cancellationToken);
        result.Result.Dispose();
        return new ImageCompressResult<byte[]>(newBytes, result.State);
    }

    protected virtual bool CanCompress(string? mimeType)
    {
        return mimeType switch {
            MimeTypes.Image.Jpeg => true,
            MimeTypes.Image.Png => true,
            MimeTypes.Image.Webp => true,
            _ => false
        };
    }

    protected virtual async Task<Stream> GetStreamFromImageAsync(
        Image image, 
        IImageFormat format,
        CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();

        try
        {
            await image.SaveAsync(memoryStream, GetEncoder(format), cancellationToken: cancellationToken);

            memoryStream.Position = 0;

            return memoryStream;
        }
        catch
        {
            memoryStream.Dispose();
            throw;
        }
    }

    protected virtual IImageEncoder GetEncoder(IImageFormat format)
    {
        switch (format.DefaultMimeType)
        {
            case MimeTypes.Image.Jpeg:
                return Options.JpegEncoder ?? new JpegEncoder();
            case MimeTypes.Image.Png:
                return Options.PngEncoder ?? new PngEncoder();
            case MimeTypes.Image.Webp:
                return Options.WebpEncoder ?? new WebpEncoder();
            default:
                throw new NotSupportedException($"No encoder available for the given format: {format.Name}");
        }
    }
}