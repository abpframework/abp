using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace Volo.Abp.Imaging;

public class MagickImageCompressorContributor : IImageCompressorContributor, ITransientDependency
{
    protected MagickNetCompressOptions Options { get; }

    private readonly ImageOptimizer _optimizer;

    public MagickImageCompressorContributor(IOptions<MagickNetCompressOptions> options)
    {
        Options = options.Value;
        _optimizer = new ImageOptimizer {
            OptimalCompression = Options.OptimalCompression, IgnoreUnsupportedFormats = Options.IgnoreUnsupportedFormats
        };
    }

    private static bool CanCompress(string mimeType)
    {
        return mimeType switch {
            MimeTypes.Image.Jpeg => true,
            MimeTypes.Image.Png => true,
            MimeTypes.Image.Gif => true,
            _ => false
        };
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
            ms = await stream.CreateMemoryStreamAsync(cancellationToken: cancellationToken);
            
            if (!_optimizer.IsSupported(ms))
            {
                return new ImageContributorResult<Stream>(stream, false, false);
            }
            
            Func<Stream, bool> compressFunc;

            if (Options.Lossless)
            {
                compressFunc = _optimizer.LosslessCompress;
            }
            else
            {
                compressFunc = _optimizer.Compress;
            }

            if (compressFunc(ms))
            {
                return new ImageContributorResult<Stream>(ms, true);
            }

            ms.Dispose();
            return new ImageContributorResult<Stream>(stream, false);
        }
        catch (Exception e)
        {
            ms?.Dispose();
            return new ImageContributorResult<Stream>(stream, false, false, e);
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
            result.Result.Dispose();
            return new ImageContributorResult<byte[]>(bytes, result.IsSuccess, result.IsSupported, result.Exception);
        }

        var newBytes = await result.Result.GetAllBytesAsync(cancellationToken);
        result.Result.Dispose();
        return new ImageContributorResult<byte[]>(newBytes, true);
    }
}