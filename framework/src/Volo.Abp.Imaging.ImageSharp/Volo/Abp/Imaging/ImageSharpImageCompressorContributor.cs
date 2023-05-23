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

    public async Task<ImageContributorResult<Stream>> TryCompressAsync(Stream stream, string mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanCompress(mimeType))
        {
            return new ImageContributorResult<Stream>(stream, false, false);
        }

        MemoryStream ms = null;

        try
        {
            var (image, format) = await SixLabors.ImageSharp.Image.LoadWithFormatAsync(stream, cancellationToken);
            var beforeSize = stream.Length;
            mimeType = format.DefaultMimeType;

            if (!CanCompress(mimeType))
            {
                return new ImageContributorResult<Stream>(stream, false, false);
            }
            
            IImageEncoder encoder = null;

            switch (format.DefaultMimeType)
            {
                case MimeTypes.Image.Jpeg:
                    encoder = Options.JpegEncoder ?? new JpegEncoder();
                    break;
                case MimeTypes.Image.Png:
                    encoder = Options.PngEncoder ?? new PngEncoder();
                    break;
                case MimeTypes.Image.Webp:
                    encoder = Options.WebpEncoder ?? new WebpEncoder();
                    break;
                case null:
                    throw new NotSupportedException($"No encoder available for the given format: {format.Name}");
            }

            ms = new MemoryStream();
            await image.SaveAsync(ms, encoder, cancellationToken: cancellationToken);
            
            var afterSize = ms.Length;
            
            if (afterSize >= beforeSize)
            {
                ms.Dispose();
                return new ImageContributorResult<Stream>(stream, false);
            }
            ms.Position = 0;

            return new ImageContributorResult<Stream>(ms, true);
        }
        catch (Exception e)
        {
            ms?.Dispose();

            return new ImageContributorResult<Stream>(stream, false, true, e);
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