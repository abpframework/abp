using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Image;

public class ImageCompressor : IImageCompressor, ITransientDependency
{
    protected IEnumerable<IImageCompressorContributor> ImageCompressorContributors { get; }
    
    public ImageCompressor(IEnumerable<IImageCompressorContributor> imageCompressorContributors)
    {
        ImageCompressorContributors = imageCompressorContributors;
    }

    public virtual async Task<ImageProcessResult<Stream>> CompressAsync(Stream stream, string mimeType = null, bool ignoreExceptions = false, CancellationToken cancellationToken = default)
    {
        foreach (var imageCompressorContributor in ImageCompressorContributors)
        {
            var result = await imageCompressorContributor.TryCompressAsync(stream, mimeType, cancellationToken);
            if (!result.IsSupported)
            {
                continue;
            }
            
            if (!ignoreExceptions && result.Exception != null)
            {
                throw result.Exception;
            }

            if (result.IsSuccess)
            {
                return new ImageProcessResult<Stream>(result.Result, result.IsSuccess);
            }
            
            return new ImageProcessResult<Stream>(stream, false);
        }
        
        return new ImageProcessResult<Stream>(stream, false);
    }

    public virtual async Task<ImageProcessResult<byte[]>> CompressAsync(byte[] bytes, string mimeType = null, bool ignoreExceptions = false, CancellationToken cancellationToken = default)
    {
        foreach (var imageCompressorContributor in ImageCompressorContributors)
        {
            var result = await imageCompressorContributor.TryCompressAsync(bytes, mimeType, cancellationToken);
            if (!result.IsSupported)
            {
                continue;
            }
            
            if (!ignoreExceptions && result.Exception != null)
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