using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace Volo.Abp.Imaging;

public class MagickImageCompressorContributor : IImageCompressorContributor, ITransientDependency
{
    protected MagickNetCompressOptions Options { get; }

    protected readonly ImageOptimizer Optimizer;

    public MagickImageCompressorContributor(IOptions<MagickNetCompressOptions> options)
    {
        Options = options.Value;
        Optimizer = new ImageOptimizer
        {
            OptimalCompression = Options.OptimalCompression,
            IgnoreUnsupportedFormats = Options.IgnoreUnsupportedFormats
        };
    }

    public virtual async Task<ImageCompressResult<Stream>> TryCompressAsync(
        Stream stream, 
        [CanBeNull] string mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanCompress(mimeType))
        {
            return new ImageCompressResult<Stream>(stream, ProcessState.Unsupported);
        }

        var memoryStream = await stream.CreateMemoryStreamAsync(cancellationToken: cancellationToken);

        try
        {
            if (!Optimizer.IsSupported(memoryStream))
            {
                return new ImageCompressResult<Stream>(stream, ProcessState.Unsupported);
            }

            if (Compress(memoryStream))
            {
                return new ImageCompressResult<Stream>(memoryStream, ProcessState.Done);
            }

            memoryStream.Dispose();

            return new ImageCompressResult<Stream>(stream, ProcessState.Canceled);
        }
        catch
        {
            memoryStream.Dispose();
            throw;
        }
    }

    public virtual async Task<ImageCompressResult<byte[]>> TryCompressAsync(
        byte[] bytes, 
        [CanBeNull] string mimeType = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(mimeType) && !CanCompress(mimeType))
        {
            return new ImageCompressResult<byte[]>(bytes, ProcessState.Unsupported);
        }

        using var memoryStream = new MemoryStream(bytes);
        var result = await TryCompressAsync(memoryStream, mimeType, cancellationToken);

        if (result.State != ProcessState.Done)
        {
            return new ImageCompressResult<byte[]>(bytes, result.State);
        }

        var newBytes = await result.Result.GetAllBytesAsync(cancellationToken);

        result.Result.Dispose();

        return new ImageCompressResult<byte[]>(newBytes, result.State);
    }
    
    protected virtual bool CanCompress(string mimeType)
    {
        return mimeType switch {
            MimeTypes.Image.Jpeg => true,
            MimeTypes.Image.Png => true,
            MimeTypes.Image.Gif => true,
            _ => false
        };
    }
    
    protected virtual bool Compress(Stream stream)
    {
        return Options.Lossless ? Optimizer.LosslessCompress(stream) : Optimizer.Compress(stream);
    }
}