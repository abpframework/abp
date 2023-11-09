using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Imaging;

public class ImageCompressor : IImageCompressor, ITransientDependency
{
    protected IEnumerable<IImageCompressorContributor> ImageCompressorContributors { get; }
    
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    
    public ImageCompressor(IEnumerable<IImageCompressorContributor> imageCompressorContributors, ICancellationTokenProvider cancellationTokenProvider)
    {
        ImageCompressorContributors = imageCompressorContributors.Reverse();
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public virtual async Task<ImageCompressResult<Stream>> CompressAsync(
        [NotNull] Stream stream,
        string? mimeType = null,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(stream, nameof(stream));
        
        if(!stream.CanRead)
        {
            return new ImageCompressResult<Stream>(stream, ImageProcessState.Unsupported);
        }
        
        if(!stream.CanSeek)
        {
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, CancellationTokenProvider.FallbackToProvider(cancellationToken));
            SeekToBegin(memoryStream);
            stream = memoryStream;
        }

        foreach (var imageCompressorContributor in ImageCompressorContributors)
        {
            var result = await imageCompressorContributor.TryCompressAsync(stream, mimeType, CancellationTokenProvider.FallbackToProvider(cancellationToken));
            
            SeekToBegin(stream);
            
            if (result.State == ImageProcessState.Unsupported)
            {
                continue;
            }

            return result;
        }
        
        return new ImageCompressResult<Stream>(stream, ImageProcessState.Unsupported);
    }

    public virtual async Task<ImageCompressResult<byte[]>> CompressAsync(
        [NotNull] byte[] bytes,
        string? mimeType = null,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(bytes, nameof(bytes));
        
        foreach (var imageCompressorContributor in ImageCompressorContributors)
        {
            var result = await imageCompressorContributor.TryCompressAsync(bytes, mimeType, CancellationTokenProvider.FallbackToProvider(cancellationToken));
            
            if (result.State == ImageProcessState.Unsupported)
            {
                continue;
            }
            
            return result;
        }
        
        return new ImageCompressResult<byte[]>(bytes, ImageProcessState.Unsupported);
    }
    
    protected virtual void SeekToBegin(Stream stream)
    {
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }
    }
}