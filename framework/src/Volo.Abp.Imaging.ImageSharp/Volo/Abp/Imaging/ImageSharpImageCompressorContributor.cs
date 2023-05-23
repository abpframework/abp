using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
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

    public async Task<ImageContributorResult<Stream>> TryCompressAsync(
        Stream stream, 
        string mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!mimeType.IsNullOrWhiteSpace() && !CanCompress(mimeType))
        {
            return new ImageContributorResult<Stream>(stream, false);
        }

        var (image, format) = await Image.LoadWithFormatAsync(stream, cancellationToken);
        
        if (!CanCompress(format.DefaultMimeType))
        {
            return new ImageContributorResult<Stream>(stream, false);
        }

        return new ImageContributorResult<Stream>(await GetStreamFromImage(stream, cancellationToken, image, format));
    }

    private async Task<Stream> GetStreamFromImage(
        Stream stream, 
        CancellationToken cancellationToken,
        Image image,
        IImageFormat format)
    {
        var memoryStream = new MemoryStream();
        
        try
        {
            await image.SaveAsync(memoryStream, GetEncoder(format), cancellationToken: cancellationToken);

            if (memoryStream.Length >= stream.Length)
            {
                memoryStream.Dispose();
                return stream;
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
        catch
        {
            memoryStream.Dispose();
            throw;
        }
    }

    private IImageEncoder GetEncoder(IImageFormat format)
    {
        switch (format.DefaultMimeType)
        {
            case MimeTypes.Image.Jpeg:
                return Options.JpegEncoder ?? new JpegEncoder();
            case MimeTypes.Image.Png:
                return  Options.PngEncoder ?? new PngEncoder();
            case MimeTypes.Image.Webp:
                return  Options.WebpEncoder ?? new WebpEncoder();
            default:
                throw new NotSupportedException($"No encoder available for the given format: {format.Name}");
        }
    }

    public async Task<ImageContributorResult<byte[]>> TryCompressAsync(byte[] bytes, string mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanCompress(mimeType))
        {
            return new ImageContributorResult<byte[]>(bytes, false, false);
        }

        using var ms = new MemoryStream(bytes);
        var result = await TryCompressAsync(ms, mimeType, cancellationToken);

        if (!result.IsSuccess)
        {
            return new ImageContributorResult<byte[]>(bytes, result.IsSuccess, result.IsSupported, result.Exception);
        }

        var newBytes = await result.Result.GetAllBytesAsync(cancellationToken);
        result.Result.Dispose();
        return new ImageContributorResult<byte[]>(newBytes, true);
    }

    private static bool CanCompress(string mimeType)
    {
        return mimeType switch {
            MimeTypes.Image.Jpeg => true,
            MimeTypes.Image.Png => true,
            MimeTypes.Image.Webp => true,
            _ => false
        };
    }
}