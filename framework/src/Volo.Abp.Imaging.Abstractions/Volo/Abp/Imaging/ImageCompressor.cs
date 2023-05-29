using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Imaging;

public class ImageCompressor : IImageCompressor, ITransientDependency
{
    protected IEnumerable<IImageCompressorContributor> ImageCompressorContributors { get; }
    
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    
    public ImageCompressor(IEnumerable<IImageCompressorContributor> imageCompressorContributors, ICancellationTokenProvider cancellationTokenProvider)
    {
        ImageCompressorContributors = imageCompressorContributors;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public virtual async Task<ImageCompressResult<Stream>> CompressAsync(Stream stream, string mimeType = null, CancellationToken cancellationToken = default)
    {
        foreach (var imageCompressorContributor in ImageCompressorContributors)
        {
            var result = await imageCompressorContributor.TryCompressAsync(stream, mimeType, CancellationTokenProvider.FallbackToProvider(cancellationToken));
            
            if (result.State == ImageProcessState.Unsupported)
            {
                continue;
            }

            return result;
        }
        
        return new ImageCompressResult<Stream>(stream, ImageProcessState.Unsupported);
    }

    public virtual async Task<ImageCompressResult<byte[]>> CompressAsync(byte[] bytes, string mimeType = null, CancellationToken cancellationToken = default)
    {
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
}