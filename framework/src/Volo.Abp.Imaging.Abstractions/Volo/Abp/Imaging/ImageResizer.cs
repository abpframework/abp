using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Imaging;

public class ImageResizer : IImageResizer, ITransientDependency
{
    protected IEnumerable<IImageResizerContributor> ImageResizerContributors { get; }
    
    protected ImageResizeOptions ImageResizeOptions { get; }
    
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    
    public ImageResizer(
        IEnumerable<IImageResizerContributor> imageResizerContributors, 
        IOptions<ImageResizeOptions> imageResizeOptions, 
        ICancellationTokenProvider cancellationTokenProvider)
    {
        ImageResizerContributors = imageResizerContributors;
        CancellationTokenProvider = cancellationTokenProvider;
        ImageResizeOptions = imageResizeOptions.Value;
    }
    
    public virtual async Task<ImageResizeResult<Stream>> ResizeAsync(Stream stream, ImageResizeArgs resizeArgs, string mimeType = null, CancellationToken cancellationToken = default)
    {
        ChangeDefaultResizeMode(resizeArgs);
        
        foreach (var imageResizerContributor in ImageResizerContributors)
        {
            var result = await imageResizerContributor.TryResizeAsync(stream, resizeArgs, mimeType, CancellationTokenProvider.FallbackToProvider(cancellationToken));
            if (result.State == ProcessState.Unsupported)
            {
                continue;
            }

            return result;
        }
        
        return new ImageResizeResult<Stream>(stream, ProcessState.Unsupported);
    }

    public virtual async Task<ImageResizeResult<byte[]>> ResizeAsync(byte[] bytes, ImageResizeArgs resizeArgs, string mimeType = null, CancellationToken cancellationToken = default)
    {
        ChangeDefaultResizeMode(resizeArgs);
        
        foreach (var imageResizerContributor in ImageResizerContributors)
        {
            var result = await imageResizerContributor.TryResizeAsync(bytes, resizeArgs, mimeType, CancellationTokenProvider.FallbackToProvider(cancellationToken));
            if (result.State == ProcessState.Unsupported)
            {
                continue;
            }

            return result;
        }
        
        return new ImageResizeResult<byte[]>(bytes, ProcessState.Unsupported);
    }
    
    protected virtual void ChangeDefaultResizeMode(ImageResizeArgs resizeArgs)
    {
        if (resizeArgs.Mode == ImageResizeMode.Default)
        {
            resizeArgs.Mode = ImageResizeOptions.DefaultResizeMode;
        }
    }
}