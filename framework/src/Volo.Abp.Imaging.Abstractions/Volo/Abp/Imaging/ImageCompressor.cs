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
    public ICancellationTokenProvider CancellationTokenProvider { get; }

    public ImageCompressor(IEnumerable<IImageCompressorContributor> imageCompressorContributors, ICancellationTokenProvider cancellationTokenProvider)
    {
        ImageCompressorContributors = imageCompressorContributors;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public virtual async Task<ImageProcessResult<Stream>> CompressAsync(Stream stream, string mimeType = null, CancellationToken cancellationToken = default)
    {
        foreach (var imageCompressorContributor in ImageCompressorContributors)
        {
            var result = await imageCompressorContributor.TryCompressAsync(
                stream, mimeType,
                CancellationTokenProvider.FallbackToProvider(cancellationToken)
            );
            
            if (!result.IsSupported)
            {
                continue;
            }
            
            return new ImageProcessResult<Stream>(result.Result, true);
        }
        
        return new ImageProcessResult<Stream>(stream, false);
    }

    public virtual async Task<ImageProcessResult<byte[]>> CompressAsync(byte[] bytes, string mimeType = null, CancellationToken cancellationToken = default)
    {
        foreach (var imageCompressorContributor in ImageCompressorContributors)
        {
            var result = await imageCompressorContributor.TryCompressAsync(bytes, mimeType, cancellationToken);
            if (!result.IsSupported)
            {
                continue;
            }
            
            if (result.Exception != null)
            {
                throw result.Exception;
            }
            
            if (result.IsSuccess)
            {
                return new ImageProcessResult<byte[]>(result.Result, result.IsSuccess);
            }
            
            return new ImageProcessResult<byte[]>(bytes, false);
        }
        
        return new ImageProcessResult<byte[]>(bytes, false);
    }
}